using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : IDefensiveManuever
{

    public DefensiveManuevers.DefensiveManueverType manueverType { get; } = DefensiveManuevers.DefensiveManueverType.BLOCK;

    public string GetManeuverName()
    {
        return "Block";
    }

    public int GetTargetNumber(Combatant defender)
    {
        return defender.characterSheet.meleeCombatStats.shield.DTN;
    }

    public void ResolveDefensiveManeuver(Exchange exchange)
    {
        exchange.SetDefenderInitative();
        exchange.reachWinnder = exchange.defender;
    }

    public int GetActivationCost() { return 0; }
    public bool RequiresShield() { return true; }

    public bool SimultaneousAttack()
    {
        return false;
    }

    public void SetSimultaneousAttack(SelectManuever selectManuever, MeleeHitLocation.MeleeDamageType meleeDamageType, int dice, int targetZone)
    {
        throw new System.NotImplementedException();
    }
}
