using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bout {

    public Combatant combatantA;
    public Combatant combatantB;
    public Combatant initativeCombatant;
    public Combatant reachCombatant;
    public bool onPause;

    public Bout() { }

    public Bout(Combatant combatantA, Combatant combatantB)
    {
        this.combatantA = combatantA;
        this.combatantB = combatantB;
    }

    public void RedRed(Exchange e1, Exchange e2)
    {
        int e1tn = e1.offensiveManuever.GetTargetNumber(e1.attacker, e1.meleeDamageType);
        int e2tn = e2.offensiveManuever.GetTargetNumber(e2.attacker, e2.meleeDamageType);
        Debug.Log("Reflexes("+e1.attacker.characterSheet.name+") TN: "+e1tn);
        int e1Success = DiceRoller.GetSuccess(e1.attacker.characterSheet.meleeCombatStats.reflexes, e1tn);
        Debug.Log("Reflexes(" + e2.attacker.characterSheet.name + ") TN: " + e2tn);
        int e2Success = DiceRoller.GetSuccess(e2.attacker.characterSheet.meleeCombatStats.reflexes, e2tn);

        Debug.Log(e1.attacker.characterSheet.name+" Simultaneous Defense: ");
        int e1DefensiveSuccess = e1.attackerSelectManuever.SimultaneousManuever() ? 
            DiceRoller.GetSuccess(e1.attackerSelectManuever.secondaryDicePool, 
            e1.attackerSelectManuever.defensiveManuever.GetTargetNumber(e1.attacker)) : 0;
        Debug.Log(e2.attacker.characterSheet.name + " Simultaneous Defense: ");
        int e2DefensiveSuccess = e2.attackerSelectManuever.SimultaneousManuever() ?
            DiceRoller.GetSuccess(e2.attackerSelectManuever.defensiveManuever.GetTargetNumber(e2.attacker), 
            e2.attackerSelectManuever.secondaryDicePool) : 0;

        if (e1.attackerSelectManuever.SimultaneousManuever())
            e1.attacker.currentDice -= e1.attackerSelectManuever.secondaryDicePool;

        if (e2.attackerSelectManuever.SimultaneousManuever())
            e2.attacker.currentDice -= e2.attackerSelectManuever.secondaryDicePool;


        if (e1Success > e2Success)
        {
            ResolveFirstRed(e1, e2DefensiveSuccess);   
            ResolveSecondRed(e2, e1DefensiveSuccess);
        }
        else if (e1Success == e2Success)
        {
            e1.ResolveAttackerSuccess();
            if (e1.attackerSuccess > 0 && e1.attackerSuccess > e2DefensiveSuccess) {
                e1.defenderSuccess = e2DefensiveSuccess;
                e1.ResolveHit();
            }

            e2.ResolveAttackerSuccess();
            if (e2.attackerSuccess > 0 && e2.attackerSuccess > e1DefensiveSuccess) { 
                e2.defenderSuccess = e1DefensiveSuccess;
                e2.ResolveHit();
            }
        }
        else if (e2Success > e1Success)
        {
            ResolveFirstRed(e2, e1DefensiveSuccess);
            ResolveSecondRed(e1, e2DefensiveSuccess);
        }

        reachCombatant = null;
        initativeCombatant = null;
        onPause = true;

    }

    private void ResolveFirstRed(Exchange e1, int e2DefenseSuccess) {
        e1.ResolveAttackerSuccess();

        if (e1.attackerSuccess > 0 && e1.attackerSuccess > e2DefenseSuccess)
        {
            e1.defenderSuccess = e2DefenseSuccess;
            e1.ResolveHit();
        }
    }

    private void ResolveSecondRed(Exchange e2, int e1DefenseSuccess) {
            
        if (e2.attacker.characterSheet.medicalData.conscious && e2.attacker.currentDice > 0)
        {
            e2.attackerDice = e2.attacker.currentDice < e2.attackerDice ? e2.attacker.currentDice : e2.attackerDice;
            e2.ResolveAttackerSuccess();
            if (e2.attackerSuccess > 0 && e2.attackerSuccess > e1DefenseSuccess) { 
                e2.defenderSuccess = e1DefenseSuccess;
                e2.ResolveHit();
            }
        }
    }

    public void BlueBlue() {
        initativeCombatant = null;
        reachCombatant = null;
        onPause = true;
    }

    public void RedBlue(Exchange exchange) {
        exchange.ResolveAll();

        if (exchange.defenderSelectManuever.SimultaneousManuever() && exchange.attackerSuccess <= exchange.defenderSuccess)
        {
            Debug.Log("Simultaneous Attack(" + exchange.defender.characterSheet.name + "):");
            var newSelectManuever = new SelectManuever(exchange.defenderSelectManuever.offensiveManuever.manueverType,
                exchange.defenderSelectManuever.secondaryDicePool, exchange.defenderSelectManuever.targetZone,
                exchange.defenderSelectManuever.meleeDamageType, 0);
            var newExchange = new Exchange(this, exchange.defender, exchange.attacker, newSelectManuever, null);
            newExchange.ResolveAttackerSuccess();
            if (newExchange.attackerSuccess > 0)
                newExchange.ResolveHit();
            initativeCombatant = newExchange.initiativeWinner;
            reachCombatant = newExchange.reachWinnder;
            newExchange.attacker.currentDice -= newExchange.attackerDice;
        }
        else if (exchange.attacker.currentDice > 0) { 
            initativeCombatant = exchange.initiativeWinner;
            reachCombatant = exchange.reachWinnder; 
        }
        else {
            reachCombatant = null;
            initativeCombatant = null;
            onPause = true;
        }
    }

    public void RedNone(Exchange exchange) {
        exchange.ResolveAttackerSuccess();
        if(exchange.attackerSuccess > 0)
            exchange.ResolveHit();
        if (exchange.attacker.currentDice > 0)
        {
            initativeCombatant = exchange.initiativeWinner;
            reachCombatant = exchange.reachWinnder;
        }
        else {
            initativeCombatant = null;
            reachCombatant = null;
            onPause = true;
        }
    }


    public override string ToString()
    {
        return "Bout Combatant A: " + combatantA.characterSheet.name + " to Combatant B: " + combatantB.characterSheet.name;
    }

}
