using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MagicManager;

public class SpellCastingMode : MonoBehaviour {

    RegularMode rm;

    [SerializeField] GameObject MeleeCombatUI;
    [SerializeField] GameObject SpellUI;

    [HideInInspector]
    public bool casting;
    [HideInInspector]
    public Spell selectedSpell;

    public static SpellCastingMode instance;

    private void Awake() {
        instance = this;
        casting = false;
        rm = GetComponent<RegularMode>();
    }

    public void Cast(string targetName) {
        var characterMagic = NetworkClient.localPlayer.gameObject.GetComponent<CharacterMagic>();
        characterMagic.SetVariables(selectedSpell, targetName);
        characterMagic.CastSpell();
    }

    public void ActivateSpellMode() {
        casting = true;
        rm.DeactivateRegularMode();
        SpellUI.SetActive(false);
        MeleeCombatUI.SetActive(false);
    }

    public void DeactivateSpellMode() {
        casting = false;
        SpellUI.SetActive(true);
        MeleeCombatUI.SetActive(true);
    }

}
