using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MagicManager : NetworkBehaviour
{
    public enum Spell { 
        MAGIC_MISSILE
    }

    [SerializeField] GameObject spellEffect;
    [SerializeField] Transform spellStartPos;
    [SerializeField] GameObject seekerTarget;
    [SerializeField] Spell testSpell;

    public void SpawnSpellEffect() {
        var obj = Instantiate(spellEffect, spellStartPos.position, Quaternion.identity);

        obj.GetComponent<Seeker>().Launch(seekerTarget);

        NetworkServer.Spawn(obj);
    }

    public void CastSpell(Spell spell) { 
        
        ISpellSystem spellSystem = GetSpell(spell);

        spellSystem.Cast();

        SpawnSpellEffect();
    }

    public void CastSpellTest() {
        CastSpell(testSpell);
    }

    public ISpellSystem GetSpell(Spell spell) { 
        switch(spell)
        {
            case Spell.MAGIC_MISSILE:
                return new MagicMissle();
            default:
                throw new System.Exception("Spell not found for spell: "+spell);

        }
    }


}
