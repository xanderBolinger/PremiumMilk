using Mirror;
using System.Collections;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{

    public static IEnumerator MoveCharacterOneTile() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            if (GameManager.InCombat(character))
                continue;

            var gridMover = character.GetComponent<GridMover>();
            NetworkIdentity opponentIdentity = character.GetComponent<NetworkIdentity>();
            if (opponentIdentity.connectionToClient != null)
                gridMover.RpcSetMovementTurn(opponentIdentity.connectionToClient);
            gridMover.movementTurn = true;
            yield return new WaitUntil(() => !gridMover.movementReady);
        }


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
