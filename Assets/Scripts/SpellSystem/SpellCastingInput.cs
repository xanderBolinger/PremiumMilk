using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using static MagicManager;

public class SpellCastingInput : MonoBehaviour {

    public void SelectMagicMissile() {
        var sp = GetComponent<SpellCastingMode>();

        Debug.Log("Entering into SpellCastingMode");
        sp.ActivateSpellMode();

    }
}
