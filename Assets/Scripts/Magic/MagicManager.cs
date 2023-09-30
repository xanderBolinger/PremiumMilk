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

    [HideInInspector]
    public static MagicManager magicManager;

    private void Awake()
    {
        magicManager = this;
    }

    public void SpawnSpellEffect() {
        var obj = Instantiate(spellEffect, spellStartPos.position, Quaternion.identity);

        obj.GetComponent<Seeker>().Launch(seekerTarget);

        NetworkServer.Spawn(obj);
    }

    public void CastSpell(Spell spell, string targetName="", Transform spellStartPos=null) {

        if(targetName!="")
            seekerTarget = CharacterController.GetCharacterObject(targetName);
        if (spellStartPos != null)
            this.spellStartPos = spellStartPos;

        ISpellSystem spellSystem = GetSpell(spell);

        spellSystem.Cast();

        SpawnSpellEffect();
    }

    public void CastSpellTest() {
        CastSpell(testSpell);
    }

    public ISpellSystem GetSpell(Spell spell) {
        switch (spell)
        {
            case Spell.MAGIC_MISSILE:
                spellEffect = Resources.Load<GameObject>("Prefabs/Effects/Magic/MagicMissile");
                return new MagicMissle();
            default:
                throw new System.Exception("Spell not found for spell: " + spell);

        }
    }


}
