using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    public static GameManager Instance;

    [SyncVar]
    public bool turnBasedMovement;
    [SyncVar]
    public bool playGridMovment;
    [SyncVar]
    public bool turnPaused;

    public void Start()
    {
        Instance = this;
        //SetSimultaneousMovement();
        SetSequentialMovement();
    }


    public void SetSimultaneousMovement() {
        turnBasedMovement = false;
        playGridMovment = true;
        turnPaused = false;
    }

    public void SetSequentialMovement() {
        turnBasedMovement = true; 
        playGridMovment = false;
        turnPaused = true;
    }

    public void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Character").Length < 1 || !isServer || !turnBasedMovement)
            return;

        if (CharactersReadyOrMoving() && turnPaused)
        {
            playGridMovment = true;
            turnPaused = false;
            GridMovementController.SetCharacterDestinations();
            StartCoroutine(GridMovementController.MoveCharacterOneTile());
        }
        else
        {
            playGridMovment = false;
            turnPaused = true;
        }

    }

    public bool CharactersReadyOrMoving() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) { 
            if(!CharacterReady(character)) 
                return false;
        }

        return true;
    }

    public bool CharacterReady(GameObject character) {
        var gridMover = character.GetComponent<GridMover>();
        return gridMover.movementReady;
    }

}
