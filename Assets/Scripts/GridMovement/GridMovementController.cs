using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{

    public static void SetCharacterDestinations() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            var gridMover = character.GetComponent<GridMover>();
            gridMover.SetMoveDestination(gridMover.path[0], character.GetComponent<CharacterGridInfo>());
        }
    
    }

}
