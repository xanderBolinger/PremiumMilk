using ExcelUtillity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBeat : IOffensiveManuever
{

    
    public OffensiveManuevers.OffensiveManueverType manueverType => OffensiveManuevers.OffensiveManueverType.SHIELD_BEAT;

    public int GetActivationCost()
    {
        return 2;
    }

    public string GetManeuverName()
    {
        return "Shield Beat";
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
        if (weaponBeat)
            SetWeaponBeaten(exchange.defender);
        else
            SetShieldBeaten(exchange.defender);



    }

    private void SetWeaponBeaten(Combatant target)
    {
        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts)
        {
            if (bout.combatantA == target)
                bout.combatantA.weaponBeaten = true;
            if (bout.combatantB == target)
                bout.combatantB.weaponBeaten = true;
        }
    }

    private void SetShieldBeaten(Combatant target) {
        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts) {
            if (bout.combatantA == target)
                bout.combatantA.shieldBeaten = true;
            if (bout.combatantB == target)
                bout.combatantB.shieldBeaten = true;
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
        return (true, reachCost / 2);
    }

    public bool weaponBeat { get; set; }
    public void SetWeaponBeat(bool weaponBeat) { this.weaponBeat = weaponBeat; }
}
