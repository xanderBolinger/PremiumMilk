using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveManuevers {
    public enum DefensiveManueverType {
        BLOCK, PARRY, DODGE, DUCKANDWEAVE, FULLEVASION, BLOCK_AND_OPEN_STRIKE
    }

    static List<IDefensiveManuever> manuevers = new List<IDefensiveManuever> { new BlockAndOpenStrike(), 
        new Parry(), new Dodge(), new DuckWeave(), new FullEvasion(), new Block() };

    public static IDefensiveManuever GetManuever(DefensiveManueverType manueverType) {

        foreach (var item in manuevers) {
            if (item.manueverType == manueverType)
                return item;
        }

        throw new System.Exception("Defensive maneuver not found for maneuverType: " + manueverType);
    }

}
