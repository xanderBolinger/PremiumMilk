using Mirror;
using UnityEngine;
using static MeleeCombatController;
using static MeleeCombatManager;
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
        if (GameObject.FindGameObjectsWithTag("Character").Length < 1 || !isServer || !turnBasedMovement
            || MapManager.Instance == null || MapManager.Instance.map?.Count < 1)
            return;

        if (CharactersReady() && turnPaused &&
            ((meleeCombatController != null && meleeCombatManager != null && meleeCombatManager.bouts != null)
            && (meleeCombatController.meleeCombatResolved 
            || meleeCombatManager.bouts.Count == 0)))
        {
            playGridMovment = true;
            turnPaused = false;
            TakeTurn();
        }
        else if(CharactersNotMoving())
        {
            playGridMovment = false;
            turnPaused = true;
        }

    }

    public void TakeTurn() {
        GridMovementController.SetCharacterDestinations();
        StartCoroutine(GridMovementController.MoveCharacterOneTile());
        meleeCombatController.meleeCombatResolved = false;
        meleeCombatController.TryAdvance();

        RefreshCharacters();

    }

    public void RefreshCharacters() {
        foreach (var character in GameObject.FindGameObjectsWithTag("Character"))
        {
            var magic = character.GetComponent<CharacterMagic>();
            var animator = character.GetComponent<CharacterAnimator>();
            magic.castedSpell = false;
        }
    }

    public bool CharactersReady() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            if (!character.GetComponent<CharacterController>().player || InCombat(character))
                continue;
            if(!CharacterReady(character)) 
                return false;
        }

        return true;
    }

    public bool CharactersNotMoving() {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (InCombat(character))
                continue;
            if (CharacterReady(character))
                return false;
            if (character.GetComponent<GridMover>().Moving())
                return false;
            
        }

        return true;
    }

    public bool CharacterReady(GameObject character) {
        var gridMover = character.GetComponent<GridMover>();
        var magic = character.GetComponent<CharacterMagic>();
        return gridMover.movementReady || magic.castedSpell;
    }

    public static bool InCombat(GameObject character) {
        if (MeleeCombatManager.meleeCombatManager == null || MeleeCombatManager.meleeCombatManager.bouts == null)
            return false; 

        string name = character.GetComponent<CharacterNetwork>().characterName;

        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts) {
            if (bout.combatantA.characterSheet.name == name 
                || bout.combatantB.characterSheet.name == name)
                return true;
        }

        return false; 
    }

}
