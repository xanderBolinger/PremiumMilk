using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.GraphicsBuffer;

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
            var gridMover = character.GetComponent<GridMover>();
            NetworkIdentity opponentIdentity = character.GetComponent<NetworkIdentity>();
            if(opponentIdentity.connectionToClient != null)
                gridMover.RpcSetMoveDestination(opponentIdentity.connectionToClient);
            
        }
    
    }

}
