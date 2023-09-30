using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SpellSystem {

    public void Cast();
    public bool CastSuccessful();
    public bool CastFailed();

}
