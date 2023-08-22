using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    public static GameManager Instance;

    [SyncVar]
    public bool gridMovement;
    [SyncVar]
    public bool playGridMovment;
    [SyncVar]
    public bool turnPaused;


    public void Start()
    {
        Instance = this;
        gridMovement = true;
        turnPaused = true;
        playGridMovment = false;
    }

    public void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Character").Length < 1)
            return;

        if (MovementOver())
        {
            turnPaused = true;
            playGridMovment = false;
        }
        
        if(PlayGridMovement()) { 
            turnPaused= false;
            playGridMovment = true;
        }


    }

    public bool PlayGridMovement() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) { 
            if(!CharacterReady(character)) 
                return false;
        }

        return true;
    }


    public bool MovementOver() {
        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) { 
            var gridMover = character.GetComponent<GridMover>();
            if (gridMover.Moving()) 
                return false;
        }

        return true;
    }

    public bool CharacterReady(GameObject character) {
        var gridMover = character.GetComponent<GridMover>();
        return gridMover.Moving();
    }

}
