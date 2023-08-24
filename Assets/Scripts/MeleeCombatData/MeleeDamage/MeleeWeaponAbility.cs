using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponAbility
{

    public MeleeWeaponAbilityType abilityType;

    public enum MeleeWeaponAbilityType
    {
        TwoHandedShock,LighteningBolt
    }

    public MeleeWeaponAbility() { }

    public static void ResolveSpecialFeature(MeleeWeaponAbilityType specialFeatureType)
    {

    }

    public static List<MeleeWeaponAbility> CreateMeleeWeaponAbilities(string weaponFeatures)
    {
        var abilityStrings = ExtractValues(weaponFeatures);
        List<MeleeWeaponAbility> meleeWeaponAbilities = new List<MeleeWeaponAbility>();

        foreach (string abilityString in abilityStrings)
        {
            if (Enum.TryParse<MeleeWeaponAbilityType>(abilityString, out MeleeWeaponAbilityType abType))
            {
                MeleeWeaponAbility ability = new MeleeWeaponAbility
                {
                    abilityType = abType
                };

                meleeWeaponAbilities.Add(ability);
            }
            else
            {
                throw new Exception($"Invalid ability string: {abilityString}");
            }
        }

        return meleeWeaponAbilities;
    }

    private static List<string> ExtractValues(string inputString)
    {
        List<string> values = new List<string>();

        string[] valueArray = inputString.Split(';');

        foreach (string value in valueArray)
        {
            // Trim leading and trailing whitespace from each value
            string trimmedValue = value.Trim();

            // Skip empty values
            if (!string.IsNullOrEmpty(trimmedValue))
            {
                values.Add(trimmedValue);
            }
        }

        return values;
    }


    

    
}
