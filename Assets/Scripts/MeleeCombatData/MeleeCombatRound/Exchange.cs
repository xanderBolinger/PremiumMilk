using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelUtillity.MeleeHitLocation;

public class Exchange {

    public Bout bout;
    public Combatant attacker;
    public Combatant defender;
    public IOffensiveManuever offensiveManuever;
    public IDefensiveManuever defensiveManuever;
    public int attackerDice;
    public int attackerTargetZone;
    public int attackerCost;
    public int defenderDice;
    public int defendeCost;
    public MeleeDamageType meleeDamageType;
    public Combatant initiativeWinner;
    public Combatant reachWinnder;
    public int attackerSuccess; 
    public int defenderSuccess;
    public ApplyMeleeDamage amd;

    public SelectManuever attackerSelectManuever;
    public SelectManuever defenderSelectManuever;

    public Exchange(Bout bout, Combatant attacker, Combatant defender, SelectManuever attackerSelectManuever, 
        SelectManuever defenderSelectManuever)
    {
        this.bout = bout;
        this.attacker = attacker;
        this.defender = defender;
        meleeDamageType = attackerSelectManuever.meleeDamageType;
        this.offensiveManuever = attackerSelectManuever.offensiveManuever;
        this.attackerCost = attackerSelectManuever.additionalCost;
        this.defensiveManuever = defenderSelectManuever == null ? null : defenderSelectManuever.defensiveManuever;
        this.attackerDice = attackerSelectManuever.dice;
        this.attackerTargetZone = attackerSelectManuever.targetZone;
        this.defenderDice = defenderSelectManuever == null ? 0 : defenderSelectManuever.dice;
        this.defendeCost = defenderSelectManuever == null ? 0 : defenderSelectManuever.additionalCost;
        this.attackerSelectManuever = attackerSelectManuever;
        this.defenderSelectManuever = defenderSelectManuever; 
    }

    public void ResolveAll() {
        ResolveAttackerSuccess();
        ResolveDefenderSuccess();
        ResolveHit();
    }

    public void ResolveAttackerSuccess() {
        Debug.Log("Attack("+attacker.characterSheet.name+" to "+defender.characterSheet.name+"): ");
        attackerSuccess = DiceRoller.GetSuccess(attackerDice, offensiveManuever.GetTargetNumber(attacker, meleeDamageType));
        attacker.currentDice -= attackerDice + attackerCost;
        Debug.Log(attacker.characterSheet.name+" to "+defender.characterSheet.name+", Attacker Success(TN: "
            + offensiveManuever.GetTargetNumber(attacker, meleeDamageType) + "): "+attackerSuccess);
        
    }

    public void ResolveDefenderSuccess() {
        Debug.Log("Defense(" + defender.characterSheet.name + " to " + attacker.characterSheet.name + "): ");
        defenderSuccess = DiceRoller.GetSuccess(defenderDice, defensiveManuever.GetTargetNumber(defender));
        defender.currentDice -= defenderDice + defendeCost;
        Debug.Log(attacker.characterSheet.name + " to " + defender.characterSheet.name + ", Defender Success(TN: "
            + defensiveManuever.GetTargetNumber(defender) + "): " + defenderSuccess);
    }

    public void ResolveHit() {

        if (defensiveManuever == null && attackerSuccess <= 0) {
            initiativeWinner = attacker;
            return;
        } else if (defensiveManuever == null && attackerSuccess > 0) {
            offensiveManuever.ResolveOffensiveManever(this, meleeDamageType);
            return;
        }

        if (attackerSuccess > defenderSuccess)
        {
            Debug.Log("Resolve Offensive Manuever(As/Ds)("+attackerSuccess+"/"+defenderSuccess+"): "+offensiveManuever.GetManeuverName());
            CombatLog.Log(attacker.characterSheet.name + " resolve offensive manuever(As/Ds)(" + attackerSuccess + "/" + defenderSuccess + "): " + offensiveManuever.GetManeuverName() 
                +" against "+defender.characterSheet.name);
            offensiveManuever.ResolveOffensiveManever(this, meleeDamageType);
        }
        else {
            CombatLog.Log(defender.characterSheet.name + " resolve defensive manuever(Ds/As)(" + defenderSuccess + "/" + attackerSuccess + "): " + defensiveManuever.GetManeuverName()
                + " against " + attacker.characterSheet.name);
            Debug.Log("Resolve Defensive Manuever(Ds/As)("+defenderSuccess+"/"+attackerSuccess+"): " + defensiveManuever.GetManeuverName());
            defensiveManuever.ResolveDefensiveManeuver(this);
        }

    }

    public void ApplyMeleeHit(MeleeDamageType damageType) {

        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();

        ApplyMeleeDamage amd = new ApplyMeleeDamage();
        amd.Hit(attackerSuccess-defenderSuccess, attacker.characterSheet,
            defender.characterSheet,
            damageType,
            attacker.meleeWeaponStatBlock,
            attackerTargetZone);
        this.amd = amd;

        watch.Stop();

        Debug.Log($"Hit Execution Time: {watch.ElapsedMilliseconds} ms");

        if (amd.av >= amd.damagePoints) {
            CombatLog.Log("Hit to location: " + amd.anatomicalHitLocation + " stopped by armor.");
            Debug.Log("Hit to location: "+amd.anatomicalHitLocation+" stopped by armor.");
            return;
        }
        CharacterController.GetCharacterObject(defender.characterSheet.name)?
               .GetComponent<CharacterSoundEffects>().RpcHit();
        if (amd.knockedDown) { KnockDown(); }
       
        defender.ApplyShock(amd.hitLocation.shockPD);
        defender.ApplyPain(amd.hitLocation.painPoints);
    }

    private void KnockDown() {

        foreach (var b in MeleeCombatManager.meleeCombatManager.bouts) {
            if (b.combatantA.characterSheet == defender.characterSheet)
            {
                CombatLog.Log(b.combatantA.characterSheet.name + " was knocked down.");
                b.combatantA.knockedDown = true;
            }
            else if (b.combatantB.characterSheet == defender.characterSheet) {
                CombatLog.Log(b.combatantB.characterSheet.name + " was knocked down.");
                b.combatantB.knockedDown = true;
            }
        }

    }

    public void SetAttackerInitative() {
        initiativeWinner = attacker;
    }

    public void SetDefenderInitative() {
        initiativeWinner = defender;
    }

    public void SetNoInitative() {
        initiativeWinner = null;
    }
}
