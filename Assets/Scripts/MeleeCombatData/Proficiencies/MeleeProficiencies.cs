using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefensiveManuevers;
using static OffensiveManuevers;

public class MeleeProficiencies {

    public enum MeleeProfType { 
        CutAndThrust,MassWeaponAndShield
    }

    public static Proficiency CutNThrust = new Proficiency("Cut and Thrust",
        new List<OffensiveManueverType> { OffensiveManueverType.CUT, OffensiveManueverType.THRUST, OffensiveManueverType.BASH },
        new List<DefensiveManueverType> { DefensiveManueverType.PARRY, },
        new Dictionary<MeleeProfType, int>() {
            {MeleeProfType.MassWeaponAndShield, -4 }
        });

    public static Proficiency MassWeaponAndShield = new Proficiency("Mass Weapon and Shield",
        new List<OffensiveManueverType> { OffensiveManueverType.CUT, OffensiveManueverType.THRUST, 
            OffensiveManueverType.BASH, OffensiveManueverType.SIMULTANEOUS_BLOCK_AND_STRIKE, 
            OffensiveManueverType.BIND_AND_STRIKE },
        new List<DefensiveManueverType> { DefensiveManueverType.PARRY, DefensiveManueverType.BLOCK, 
            DefensiveManueverType.BLOCK_AND_OPEN_STRIKE },
        new Dictionary<MeleeProfType, int>() {
            {MeleeProfType.CutAndThrust, -4 }
        });

    public static Proficiency GetProfByType(MeleeProfType type) {

        switch (type) {

            case MeleeProfType.CutAndThrust:
                return CutNThrust;
            case MeleeProfType.MassWeaponAndShield:
                return MassWeaponAndShield;
        }

        throw new System.Exception("Melee prof type not found for type: "+type);

    }

}
