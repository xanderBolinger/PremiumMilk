using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : IDefensiveManuever
{

    public DefensiveManuevers.DefensiveManueverType manueverType { get; } = DefensiveManuevers.DefensiveManueverType.PARRY;

    public string GetManeuverName()
    {
        return "Parry";
    }

    public int GetTargetNumber(Combatant defender)
    {
        return defender.meleeWeaponStatBlock.dtn;
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

    public void SetSimultaneousAttack(SelectManuever selectManuever, MeleeHitLocation.MeleeDamageType meleeDamageType, int dice, int targetZone)
    {
        throw new System.NotImplementedException();
    }
}
