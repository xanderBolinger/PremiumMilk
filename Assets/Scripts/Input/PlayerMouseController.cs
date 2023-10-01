using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterCombatController))]
[RequireComponent(typeof(GridMover))]
public class PlayerMouseController : NetworkBehaviour
{
    private GridMover gridMover;
    private CharacterCombatController characterCombatController;
    CharacterMagic characterMagic;

    void Start() { 
        gridMover = GetComponent<GridMover>();
        characterCombatController = GetComponent<CharacterCombatController>();
        characterMagic = GetComponent<CharacterMagic>();
    }

    Tile lastTile;

    private void Update()
    {
        var gm = GameManager.Instance;
        var tile = gridMover.GetHitTile();
        var target = PlayerMouseController.GetClickedCharacter();

        if (!isLocalPlayer || (gm.turnBasedMovement && !gm.turnPaused))
            return;
        else if (tile != null && (tile != lastTile || Input.GetMouseButtonDown(0)) && !GameManager.InCombat(gameObject) && !MouseOverUi() && !SpellCastingMode.instance.casting)
            Move(tile);
        else if (target != null && gameObject != target && !SpellCastingMode.instance.casting && !MouseOverUi()) {
            characterCombatController.EnterCombat(target);
        } else if (target != null && gameObject != target && SpellCastingMode.instance.casting
            && target.tag != "Dead" && !characterMagic.castedSpell) {
            SpellCastingMode.instance.Cast(target.GetComponent<CharacterNetwork>()
                .GetCharacterSheet().name);
            SpellCastingMode.instance.DeactivateSpellMode();
        } else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            && SpellCastingMode.instance.casting) {
            SpellCastingMode.instance.DeactivateSpellMode();
        }
    }

    private bool MouseOverUi() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void Move(Tile tile)
    {
        lastTile = tile;
        List<Tile> newPath;
        if (!SomeoneStandingOnTile(tile))
            newPath = gridMover.GetNewPath(tile);
        else
            newPath = new List<Tile>();

        gridMover.PlotPath(newPath);
        gridMover.ConfirmNewPath(newPath);
        //Debug.Log("New Path Length: "+newPath.Count+", Tile: "+(tile == null ? "null" : tile.x+", "+tile.y));
    }

    bool SomeoneStandingOnTile(Tile tile) {

        foreach (var character in FindObjectsOfType<CharacterGridInfo>()) {
            if (tile == character.standingOnTile)
                return true;
        }

        return false;
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
