using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelUtillity.MeleeHitLocation;

public class Cut : IOffensiveManuever
{
    public OffensiveManuevers.OffensiveManueverType manueverType { get; } = OffensiveManuevers.OffensiveManueverType.CUT;

    public int GetTargetNumber(Combatant attacker, MeleeDamageType meleeDamageType)
    {
        return attacker.meleeWeaponStatBlock.atnCut;
    }


    public string GetManeuverName()
    {
        return "Cut";
    }

    public void ResolveOffensiveManever(Exchange exchange, MeleeDamageType damageType) {
        exchange.ApplyMeleeHit(MeleeDamageType.CUTTING);
        exchange.SetAttackerInitative();
        exchange.reachWinnder = exchange.attacker;
    }

    public int GetActivationCost()
    {
        return 0;
    }

    public bool SimultaneousDefense()
    {
        return false;
    }

    public void SetSimultaneousDefense(int dice, SelectManuever selectManuever)
    {
        throw new System.NotImplementedException();
    }

    public bool RequiresShield() { return false; }

    public (bool, int) AlternateReachCost(int reachCost)
    {
        return (false, 0);
    }

    public bool weaponBeat { get; set; }
    public void SetWeaponBeat(bool weaponBeat) { this.weaponBeat = weaponBeat; }
}
