using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveManuevers
{
    public enum OffensiveManueverType { 
        CUT,THRUST,BASH,SIMULTANEOUS_BLOCK_AND_STRIKE,BIND_AND_STRIKE,SHIELD_BEAT
    }

    static List<IOffensiveManuever> manuevers = new List<IOffensiveManuever> { new Cut(), new Thrust(), new Bash(),
    new SimultaneousBlockAndStrike(), new BindAndStrike(), new ShieldBeat()};

    public static IOffensiveManuever GetManuever(OffensiveManueverType manueverType) {

        foreach (var item in manuevers) {
            if (item.manueverType == manueverType)
                return item;
        }

        throw new System.Exception("Offensive maneuver not found for maneuverType: "+manueverType);
    }

}
