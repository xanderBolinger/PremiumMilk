using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefensiveManuevers;
using static MeleeProficiencies;
using static OffensiveManuevers;

public class Proficiency
{
    public List<IOffensiveManuever> subOffensiveManuevers { get; } = new List<IOffensiveManuever>();
    public List<IDefensiveManuever> subDefensiveManuevers { get; } = new List<IDefensiveManuever>();
    public string name { get; }
    public Dictionary<MeleeProfType, int> defaults;
    public MeleeProfType meleeProfType;

    public Proficiency() { }

    public bool ContainsManeuver(OffensiveManueverType offensiveManueverType)
    {
        foreach (var manuever in subOffensiveManuevers)
            if (manuever.manueverType == offensiveManueverType)
                return true;

        return false; 
    }

    public bool ContainsManeuver(DefensiveManueverType defensiveManueverType)
    {
        foreach (var manuever in subDefensiveManuevers)
            if (manuever.manueverType == defensiveManueverType)
                return true;

        return false;
    }

    public Proficiency(string name, List<OffensiveManueverType> subOffensiveManuevers, List<DefensiveManueverType> subDefensiveManuevers,
        Dictionary<MeleeProfType, int> defaults) {
        this.name = name;
        
        foreach (OffensiveManueverType o in subOffensiveManuevers) {
            this.subOffensiveManuevers.Add(OffensiveManuevers.GetManuever(o));
        }

        foreach(DefensiveManueverType d in subDefensiveManuevers) {
            this.subDefensiveManuevers.Add(DefensiveManuevers.GetManuever(d));
        }

        this.defaults = defaults;
    }

    public int GetDefaultValue(MeleeProfType meleeProfType) {
        return defaults[meleeProfType];
    }

}
