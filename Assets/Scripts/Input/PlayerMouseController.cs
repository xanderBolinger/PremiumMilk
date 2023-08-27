using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterCombatController))]
[RequireComponent(typeof(GridMover))]
public class PlayerMouseController : NetworkBehaviour
{
    private GridMover gridMover;
    private CharacterCombatController characterCombatController;

    void Start() { 
        gridMover = GetComponent<GridMover>();
        characterCombatController = GetComponent<CharacterCombatController>();
    }

    private void Update()
    {
        var gm = GameManager.Instance;
        var tile = gridMover.GetHitTile();
        var target = PlayerMouseController.GetClickedCharacter();

        if (!isLocalPlayer || (gm.turnBasedMovement && !gm.turnPaused))
            return;
        else if (tile != null && !GameManager.InCombat(gameObject))
            Move(tile);
        else if (target != null) { 
            characterCombatController.EnterCombat(target);
            
        }
    }

    private void Move(Tile tile)
    {
        var newPath = gridMover.GetNewPath(tile);
        gridMover.PlotPath(newPath);
        gridMover.ConfirmNewPath(newPath);
    }

    public static GameObject GetClickedCharacter() {
        if (Input.GetMouseButtonDown(0) == false)
            return null;

        return GetHoveredCharacter();
    }

    public static GameObject GetHoveredCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 100) || hit.transform.gameObject.tag != "Character")
        {
            return null;
        }

        return hit.transform.gameObject;
    }


}
