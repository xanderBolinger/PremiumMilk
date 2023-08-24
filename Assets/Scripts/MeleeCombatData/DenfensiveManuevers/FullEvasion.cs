using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullEvasion : IDefensiveManuever
{

    public DefensiveManuevers.DefensiveManueverType manueverType { get; } = DefensiveManuevers.DefensiveManueverType.FULLEVASION;

    public string GetManeuverName()
    {
        return "Full Evasion";
    }

    public int GetTargetNumber(Combatant defender)
    {
        return 4;
    }

    public void ResolveDefensiveManeuver(Exchange exchange) {
        exchange.SetNoInitative();
        exchange.reachWinnder = null;
        exchange.bout.onPause = true;
    }

    public int GetActivationCost() { return 1; }
    public bool RequiresShield() { return false; }

    public bool SimultaneousAttack()
    {
        return false;
    }

    public void SetSimultaneousAttack(SelectManuever selectManuever, MeleeHitLocation.MeleeDamageType meleeDamageType, int dice, int targetZone)
    {
        throw new System.NotImplementedException();
    }
}
