using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularMode : MonoBehaviour {

    [SerializeField] GameObject MeleeCombatUI;
    [SerializeField] GameObject SpellUI;

    SpellCastingMode sm;

    private void Awake() {
        sm = GetComponent<SpellCastingMode>();
    }

    void Start() {
        ActivateRegularMode();
    }

    public void ActivateRegularMode() {
        sm.DeactivateSpellMode();
        MeleeCombatUI.SetActive(true);
        SpellUI.SetActive(true);
    }

    public void DeactivateRegularMode() {
        MeleeCombatUI.SetActive(false);
        SpellUI.SetActive(false);
    }
}
