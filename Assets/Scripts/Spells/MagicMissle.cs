using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MagicMissle : ISpellSystem{

    [SerializeField] GameObject MagicEffect;

    public bool CanCast(){
        return false;
    }

    public void Cast(){
    }

    public bool CastFailed(){
        return false;
    }

    public bool CastSuccessful() {
        return false;
    }
}
