using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefensiveManuevers;
using static ExcelUtillity.MeleeHitLocation;

public interface IDefensiveManuever {

    public DefensiveManueverType manueverType { get; }
    public void ResolveDefensiveManeuver(Exchange exchange);

    public int GetActivationCost();

    public string GetManeuverName();
    public int GetTargetNumber(Combatant defender);

    public bool RequiresShield();

    public bool SimultaneousAttack();

    public void SetSimultaneousAttack(SelectManuever selectManuever, MeleeDamageType meleeDamageType, int dice, int targetZone);

}
