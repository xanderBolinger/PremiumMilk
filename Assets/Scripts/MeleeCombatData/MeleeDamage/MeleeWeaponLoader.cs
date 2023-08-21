using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeleeWeaponLoader
{

    const int NAME = 0; //name 0
    const int ATN_CUT = 1; //atn cut 1
    const int ATN_THRUST = 2; //atn thrust 2
    const int DTN = 3; //dtn 3
    const int CUT_MOD = 4; //cut mod 4 
    const int THRUST_MOD = 5; //thrust mod 5
    const int BLUNT_MOD = 6; //blunt mod 6 
    const int REACH = 7; //reach 7
    const int BLUNT_AP = 8; //blunt ap 8
    const int THRUST_AP = 9; //thrust ap 9
    const int CUT_AP = 10; //cut ap 10
    const int ONE_HANDED_ONLY = 11; //one handed only 11
    const int TWO_HANDED_ONLY = 12; //two handed only 12
    const int WEAPON_TYPE = 13; //weapon type 13
    const int SPECIAL_FEATURES = 14; //special features 14

    public static MeleeWeaponStatBlock GetWeaponByName(string weaponName)
    {
        string fileName = "MeleeWeaponData";

        TextAsset damageLevelData = Resources.Load<TextAsset>(fileName);

        string[] data = damageLevelData.text.Split(new char[] { '\n' });

        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (weaponName == row[NAME])
            {
                string name = row[NAME];
                int.TryParse(row[ATN_THRUST], out int atnThrust);
                int.TryParse(row[ATN_CUT], out int atnCut);
                int.TryParse(row[DTN], out int dtn);
                int.TryParse(row[CUT_MOD], out int cutMod);
                int.TryParse(row[THRUST_MOD], out int thrustMod);
                int.TryParse(row[BLUNT_MOD], out int bluntMod);
                int.TryParse(row[REACH], out int reach);
                Boolean.TryParse(row[BLUNT_AP], out bool bluntAP);
                Boolean.TryParse(row[THRUST_AP], out bool thrustAP);
                Boolean.TryParse(row[CUT_AP], out bool cutAP);
                Boolean.TryParse(row[ONE_HANDED_ONLY], out bool oneHandedOnly);
                Boolean.TryParse(row[TWO_HANDED_ONLY], out bool twoHandedOnly);
                string weaponType = row[WEAPON_TYPE];
                string specialFeatures = row[SPECIAL_FEATURES];

                return new MeleeWeaponStatBlock(name, weaponType, atnCut, atnThrust,
                    dtn, cutMod, thrustMod, bluntMod, reach, oneHandedOnly, twoHandedOnly, bluntAP,
                    thrustAP, cutAP, MeleeWeaponAbility.CreateMeleeWeaponAbilities(specialFeatures));

            }

        }

        throw new Exception("Melee Weapon Not found for weapon name: " + weaponName);
    }

    public static List<string> GetWeaponNames() {
        List<string> weaponNames = new List<string>();
        string fileName = "MeleeWeaponData";

        TextAsset damageLevelData = Resources.Load<TextAsset>(fileName);

        string[] data = damageLevelData.text.Split(new char[] { '\n' });

        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            weaponNames.Add(row[NAME]);
        }

        return weaponNames;
    }

}


