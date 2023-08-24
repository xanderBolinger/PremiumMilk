using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckWeave : IDefensiveManuever
{

    public DefensiveManuevers.DefensiveManueverType manueverType { get; } = DefensiveManuevers.DefensiveManueverType.DUCKANDWEAVE;

    public string GetManeuverName()
    {
        return "Duck & Weave";
    }

    public int GetTargetNumber(Combatant defender)
    {
        return 8;
    }

    public void ResolveDefensiveManeuver(Exchange exchange) {
        exchange.SetDefenderInitative();
        exchange.reachWinnder = exchange.defender;
    }

    public int GetActivationCost() { return 0; }
    public bool RequiresShield() { return false; }

    public bool SimultaneousAttack()
    {
        return false;
    }

    public void ohno()
    {
        Debug.Log("This is not a bug");
    }

    public void SetSimultaneousAttack(SelectManuever selectManuever, MeleeHitLocation.MeleeDamageType meleeDamageType, int dice, int targetZone)
    {
        throw new System.NotImplementedException();
    }
}
