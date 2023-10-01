using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
public class MagicMissle : ISpellSystem {

    [SerializeField] GameObject MagicEffect;

    public bool CanCast(){
        return false;
    }

    public void Cast(CharacterSheet characterSheet) {
        characterSheet.fatigueSystem.AddWork(0.5f, 60);
    }

    public bool CastFailed(){
        return false;
    }

    public bool CastSuccessful() {
        return false;
    }
}
