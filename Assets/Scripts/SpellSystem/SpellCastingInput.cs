using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using static MagicManager;

public class SpellCastingInput : MonoBehaviour {

    SpellCastingMode sp;

    private void Awake()
    {
        sp = GetComponent<SpellCastingMode>();
    }


    public void SelectMagicMissile() {
        sp.selectedSpell = Spell.MAGIC_MISSILE;
        Debug.Log("Entering into SpellCastingMode");
        sp.ActivateSpellMode();

    }

    public void SelectLightSpell()
    {
        sp.selectedSpell = Spell.LIGHT_SPELL;
        //Debug.Log("Entering into SpellCastingMode");
        sp.ActivateSpellMode();
        sp.Cast();

        StartCoroutine(DelayedDeactivate());
        
        //sp.DeactivateSpellMode();
    }

    IEnumerator DelayedDeactivate() {

        yield return new WaitForSecondsRealtime(2f);


        sp.DeactivateSpellMode();
    }

}
