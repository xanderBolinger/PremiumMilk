using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BindAndStrike : IOffensiveManuever
{
    public OffensiveManuevers.OffensiveManueverType manueverType => OffensiveManuevers.OffensiveManueverType.BIND_AND_STRIKE;

    public int GetActivationCost()
    {
        return 2;
    }

    public string GetManeuverName()
    {
        return "Bind and Strike";
    }

    public int GetTargetNumber(Combatant attacker, MeleeHitLocation.MeleeDamageType meleeDamageType)
    {
        return attacker.characterSheet.meleeCombatStats.shield.DTN;
    }

    public bool RequiresShield()
    {
        return true;
    }

    public void ResolveOffensiveManever(Exchange exchange, MeleeHitLocation.MeleeDamageType meleeDamageType)
    {
        int margin = exchange.attackerSuccess - exchange.defenderSuccess;
        if (MeleeCombatManager.meleeCombatManager.firstExchange)
        {
            exchange.defender.currentDice -= margin;
        }
        else {
            exchange.defender.penalty += margin;
        }

    }

    public void SetSimultaneousDefense(int dice, SelectManuever selectManuever)
    {
        throw new System.NotImplementedException();
    }

    public bool SimultaneousDefense()
    {
        return false;
    }

    public (bool, int) AlternateReachCost(int reachCost)
    {
        return (false, 0);
    }

    public bool weaponBeat { get; set; }
    public void SetWeaponBeat(bool weaponBeat) { this.weaponBeat = weaponBeat; }
}
