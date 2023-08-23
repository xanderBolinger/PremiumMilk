using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GridMovementController : MonoBehaviour
{

    public static IEnumerator MoveCharacterOneTile() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            var gridMover = character.GetComponent<GridMover>();
            gridMover.movementTurn = true;
            yield return new WaitUntil(() => !gridMover.movementReady);
        }


    }

    public static void SetCharacterDestinations() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            var gridMover = character.GetComponent<GridMover>();
            gridMover.SetMoveDestination(gridMover.path[0], character.GetComponent<CharacterGridInfo>());
        }
    
    }

}
