using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

[RequireComponent(typeof(MagicDamage))]
public class MagicManager : NetworkBehaviour
{
    public enum Spell {
        MAGIC_MISSILE,LIGHT_SPELL
    }

    [SerializeField] GameObject spellEffect;

    [SerializeField] Transform spellStartPos;

    [SerializeField] GameObject seekerTarget;

    [SerializeField] string casterName;
    [SerializeField] string targetName;

    [SerializeField] Spell testSpell;

    [HideInInspector]
    public static MagicManager magicManager;

    public MagicDamage magicDamage;

    private void Awake()
    {
        magicManager = this;
        magicDamage = GetComponent<MagicDamage>();
    }

    public void SpawnSpellEffect() {
        var obj = Instantiate(spellEffect, spellStartPos.position, Quaternion.identity);

        var seeker = obj.GetComponent<Seeker>();

        if(seeker!= null)
            seeker.Launch(seekerTarget, casterName, targetName);

        NetworkServer.Spawn(obj);
    }

    public void CastSpell(Spell spell, string casterName = "", string targetName="", Transform spellStartPos=null) {

        if (targetName != "") { 
            seekerTarget = CharacterController.GetCharacterObject(targetName);
            this.targetName = targetName;
        }
        if (spellStartPos != null)
            this.spellStartPos = spellStartPos;
        if (casterName != "")
            this.casterName = casterName;

        

        ISpellSystem spellSystem = GetSpell(spell);
        var cs = CharacterController.GetCharacter(casterName);
        spellSystem.Cast(cs);
        GameObject.Find("FatiguePoints").GetComponent<TextMeshProUGUI>().text
                    = "Fatigue Points: " + cs.fatigueSystem.fatiguePoints;

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
            case Spell.LIGHT_SPELL:
                spellEffect = Resources.Load<GameObject>("Prefabs/Effects/Magic/LightSpell");
                return new LightSpell();
            default:
                throw new System.Exception("Spell not found for spell: " + spell);

        }
    }


}
