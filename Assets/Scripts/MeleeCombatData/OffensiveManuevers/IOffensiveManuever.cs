using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OffensiveManuevers;
using static ExcelUtillity.MeleeHitLocation;

public interface IOffensiveManuever
{

    public bool weaponBeat { get; set; } 

    public OffensiveManueverType manueverType { get; }
    public void ResolveOffensiveManever(Exchange exchange, MeleeDamageType meleeDamageType);

    public int GetActivationCost();

    public string GetManeuverName();

    public int GetTargetNumber(Combatant attacker, MeleeDamageType meleeDamageType);

    public bool SimultaneousDefense();

    public void SetSimultaneousDefense(int dice, SelectManuever selectManuever);

    public bool RequiresShield();

    public (bool, int) AlternateReachCost(int reachCost);

    public void SetWeaponBeat(bool weaponBeat);
}
