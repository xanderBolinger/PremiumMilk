using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelUtillity.MeleeHitLocation;

public class SimultaneousBlockAndStrike : IOffensiveManuever
{
    public OffensiveManuevers.OffensiveManueverType manueverType => OffensiveManuevers.OffensiveManueverType.SIMULTANEOUS_BLOCK_AND_STRIKE;

    public int GetActivationCost()
    {
        return 2;
    }

    public string GetManeuverName()
    {
        return "Simultaneous Block and Strike";
    }

    public int GetTargetNumber(Combatant attacker, MeleeDamageType meleeDamageType)
    {
        if (meleeDamageType == MeleeDamageType.PIERICNG)
            return attacker.meleeWeaponStatBlock.atnThrust;
        else
            return attacker.meleeWeaponStatBlock.atnCut;
    }

    public void ResolveOffensiveManever(Exchange exchange, MeleeDamageType meleeDamageType)
    {
        exchange.ApplyMeleeHit(meleeDamageType);
        exchange.SetAttackerInitative();
        exchange.reachWinnder = exchange.attacker;
    }

    public void SetSimultaneousDefense(int dice, SelectManuever selectManuever)
    {
        selectManuever.SetSimultaneousDefense(DefensiveManuevers.GetManuever(DefensiveManuevers.DefensiveManueverType.BLOCK), dice);
    }

    public bool SimultaneousDefense()
    {
        return true;
    }

    public bool RequiresShield() { return true; }

    public (bool, int) AlternateReachCost(int reachCost)
    {
        return (false, 0);
    }

    public bool weaponBeat { get; set; }
    public void SetWeaponBeat(bool weaponBeat) { this.weaponBeat = weaponBeat; }
}
