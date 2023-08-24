using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefensiveManuevers;
using static MeleeCombatManager;
using static OffensiveManuevers;
using static ExcelUtillity.MeleeHitLocation;

public class SelectManuever
{
    public IOffensiveManuever offensiveManuever { private set; get; }
    public IDefensiveManuever defensiveManuever { private set; get; }
    public MeleeStatus meleeStatus { private set; get; }
    public int dice { private set; get; }

    public int secondaryDicePool { private set; get; }
    public int targetZone { private set; get; }
    public MeleeDamageType meleeDamageType;

    public int additionalCost;

    public SelectManuever(OffensiveManueverType offensiveManueverType, int dice, int targetZone, MeleeDamageType meleeDamageType,
        int additionalCost) {
        this.offensiveManuever = OffensiveManuevers.GetManuever(offensiveManueverType);
        meleeStatus = MeleeStatus.RED;
        this.meleeDamageType = meleeDamageType;
        this.dice = dice;
        this.targetZone = targetZone;
        this.additionalCost = additionalCost;
    }

    public SelectManuever(DefensiveManueverType defensiveManueverType, int dice, int additionalCost)
    {
        this.defensiveManuever = GetManuever(defensiveManueverType);
        meleeStatus = MeleeStatus.BLUE;
        this.dice = dice;
        this.additionalCost = additionalCost;
    }

    public SelectManuever() {
        meleeStatus = MeleeStatus.LEAVE_COMBAT;
        this.additionalCost = 0;
    }

    public void SetSimultaneousDefense(IDefensiveManuever defensiveManuever, int secondaryDice) { 
        this.defensiveManuever = defensiveManuever;
        this.secondaryDicePool = secondaryDice;
    }

    public void SetSimultaneousAttack(IOffensiveManuever offensiveManuever, int secondaryDice, int targetZone,
        MeleeDamageType meleeDamageType) {
        this.offensiveManuever = offensiveManuever;
        this.secondaryDicePool = secondaryDice;
        this.targetZone = targetZone;
        this.meleeDamageType = meleeDamageType;
    }

    public bool SimultaneousManuever() {
        return offensiveManuever != null && defensiveManuever != null;
    }

    public override string ToString()
    {
        switch (meleeStatus) {

            case MeleeStatus.RED:
                return "RED: " + offensiveManuever.manueverType + ", Dice: " + dice+", Target Zone: "+targetZone; 
            case MeleeStatus.BLUE:
                return "BLUE: " + defensiveManuever.manueverType + ", Dice: " + dice;
            case MeleeStatus.LEAVE_COMBAT:
                return "NONE";
        }
        
        

        return "N/A";
    }

}
