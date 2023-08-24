using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : IDefensiveManuever {

    public DefensiveManuevers.DefensiveManueverType manueverType { get; } = DefensiveManuevers.DefensiveManueverType.DODGE;

    public string GetManeuverName()
    {
        return "Dodge";
    }

    public int GetTargetNumber(Combatant defender)
    {
        return 7;
    }

    public void ResolveDefensiveManeuver(Exchange exchange) {
        // compare successes
        //if success do nothing
        exchange.SetDefenderInitative();
        exchange.reachWinnder = exchange.attacker;
    }

    public int GetActivationCost() { return 0; }
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
