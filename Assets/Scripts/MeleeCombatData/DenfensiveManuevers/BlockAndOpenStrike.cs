using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OffensiveManuevers;

public class BlockAndOpenStrike : IDefensiveManuever
{
    public DefensiveManuevers.DefensiveManueverType manueverType => DefensiveManuevers.DefensiveManueverType.BLOCK_AND_OPEN_STRIKE;

    public int GetActivationCost()
    {
        return 2;
    }

    public string GetManeuverName()
    {
        return "Block and Open Strike";
    }

    public int GetTargetNumber(Combatant defender)
    {
        return defender.characterSheet.meleeCombatStats.shield.DTN;
    }

    public bool RequiresShield()
    {
        return true;
    }

    public void ResolveDefensiveManeuver(Exchange exchange)
    {
        exchange.SetDefenderInitative();
        exchange.reachWinnder = exchange.defender;
    }

    public void SetSimultaneousAttack(SelectManuever selectManuever, MeleeHitLocation.MeleeDamageType meleeDamageType, int dice, int targetZone)
    {
        OffensiveManueverType offensiveManeuverType = OffensiveManueverType.CUT;

        if (meleeDamageType == MeleeHitLocation.MeleeDamageType.BLUNT)
            offensiveManeuverType = OffensiveManueverType.BASH;
        else if (meleeDamageType == MeleeHitLocation.MeleeDamageType.PIERICNG)
            offensiveManeuverType = OffensiveManueverType.THRUST;

        selectManuever.SetSimultaneousAttack(GetManuever(offensiveManeuverType), 
            dice, targetZone, meleeDamageType);

    }

    public bool SimultaneousAttack()
    {
        return true;
    }
}
