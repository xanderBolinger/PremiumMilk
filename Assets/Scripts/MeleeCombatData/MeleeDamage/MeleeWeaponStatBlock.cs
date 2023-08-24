using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Character.Species;

public class MeleeWeaponStatBlock
{
    public enum MeleeWeaponType { 
        LIGHT,MEDIUM,HEAVY
    }

    public string weaponName;
    public MeleeWeaponType weaponType;
    public int atnCut;
    public int atnThrust;
    public int dtn;
    public int cutMod;
    public int thrustMod;
    public int bluntMod;
    public int reach;
    public bool oneHandedOnly;
    public bool twoHandedOnly;
    public bool bluntAP;
    public bool thrustAP;
    public bool cutAP;
    public List<MeleeWeaponAbility> specialAbilities;

    /* weapon reach values: 
     * 1: elbow, VS 
     * 2: fist, dagger, S
     * 3: sword, M
     * 4: spear/long ax, L
     * 5: long pole arm. VL
     * 6: pike, EL 
     */

    public MeleeWeaponStatBlock() { }

    public MeleeWeaponStatBlock(string weaponName, string weaponType,
        int atnCut, int atnThrust,
        int dtn, int cutMod, int thrustMod, int bluntMod, int reach, bool oneHandedOnly, bool twoHandedOnly,
        bool bluntAP, bool thrustAP, bool cutAP, List<MeleeWeaponAbility> specialAbilities)
    {
        this.weaponName = weaponName;
        this.weaponType = GetMeleeWeaponType(weaponType);
        this.atnCut = atnCut;
        this.atnThrust = atnThrust;
        this.dtn = dtn;
        this.cutMod = cutMod;
        this.thrustMod = thrustMod;
        this.bluntMod = bluntMod;
        this.reach = reach;
        this.oneHandedOnly = oneHandedOnly;
        this.twoHandedOnly = twoHandedOnly;
        this.bluntAP = bluntAP;
        this.thrustAP = thrustAP;
        this.cutAP = cutAP;
        this.specialAbilities = specialAbilities;
    }

    public MeleeWeaponType GetMeleeWeaponType(string weaponType) {
        switch (weaponType) {
            case "LIGHT":
                return MeleeWeaponType.LIGHT;
            case "MEDIUM":
                return MeleeWeaponType.MEDIUM;
            case "HEAVY":
                return MeleeWeaponType.HEAVY;
            default:
                throw new Exception("Weapon type not found for weapon type: "+weaponType);
        }
    }

}

   

