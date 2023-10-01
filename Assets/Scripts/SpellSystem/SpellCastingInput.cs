using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using static MagicManager;

public class SpellCastingInput : MonoBehaviour {

    public void SelectMagicMissile() {
        var sp = GetComponent<SpellCastingMode>();
        sp.selectedSpell = Spell.MAGIC_MISSILE;
        Debug.Log("Entering into SpellCastingMode");
        sp.ActivateSpellMode();

    }

    public void SelectLightSpell()
    {
        var sp = GetComponent<SpellCastingMode>();
        sp.selectedSpell = Spell.LIGHT_SPELL;
        Debug.Log("Entering into SpellCastingMode");
        sp.ActivateSpellMode();
        sp.Cast();
        sp.DeactivateSpellMode();
    }

}
