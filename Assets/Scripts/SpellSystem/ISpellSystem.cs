using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellSystem {

    public void Cast(CharacterSheet characterSheet);
    public bool CanCast();
    public bool CastSuccessful();
    public bool CastFailed();

}
