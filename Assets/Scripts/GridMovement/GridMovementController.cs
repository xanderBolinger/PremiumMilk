using Mirror;
using System.Collections;
using UnityEngine;
using TMPro;

public class GridMovementController : MonoBehaviour
{

    public static IEnumerator MoveCharacterOneTile() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            if (GameManager.InCombat(character))
                continue;

            var gridMover = character.GetComponent<GridMover>();
            var cn = character.GetComponent<CharacterNetwork>();
            var characterController = character.GetComponent<CharacterController>();
            var cs = cn.GetCharacterSheet();
            var fs = cs.fatigueSystem;

            var player = character.GetComponent<CharacterController>().player;
            if (gridMover.path.Count > 0 && player)
            {
                //fs.AddWork(0.5f);
            }
            else if(player)
                fs.AddRecoveryTime(0.5f);

            if (player)
                GameObject.Find("FatiguePoints").GetComponent<TextMeshProUGUI>().text 
                    = "Fatigue Points: " + cs.fatigueSystem.fatiguePoints;


            fs.LogStats();

            //cn.UpdateCharacterSheet(cs);

            NetworkIdentity opponentIdentity = character.GetComponent<NetworkIdentity>();
            if (opponentIdentity.connectionToClient != null)
                gridMover.RpcSetMovementTurn(opponentIdentity.connectionToClient);
            gridMover.movementTurn = true;
            yield return new WaitUntil(() => !gridMover.movementReady);


        }

        CombatNetworkController.combatNetworkController.UpdateCharacters();


    }

    public static void SetCharacterDestinations() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            var player = character.GetComponent<CharacterController>().player;
            var gridMover = character.GetComponent<GridMover>();

            if (player)
            {
                NetworkIdentity opponentIdentity = character.GetComponent<NetworkIdentity>();
                if (opponentIdentity.connectionToClient != null)
                    gridMover.RpcSetMoveDestination(opponentIdentity.connectionToClient);
            }
            else { 
                var npcGridMovement = character.GetComponent<NpcGridMovement>();
                npcGridMovement.SetDestination();
            }
           


            
        }
    
    }

}
