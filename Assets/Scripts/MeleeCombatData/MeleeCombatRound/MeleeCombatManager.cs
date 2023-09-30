using Character;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeleeCombatManager : MonoBehaviour
{
    public enum MeleeStatus { 
        RED,BLUE,LEAVE_COMBAT,UNDECIDED
    }

    public List<Bout> bouts;
    public bool firstExchange;

    public List<(Bout, Exchange)> redNone;
    public List<(Bout, Exchange)> redBlue;
    public List<(Bout, Exchange, Exchange)> redRed;
    public List<Bout> blueBlue;

    public static MeleeCombatManager meleeCombatManager;

    public void Start()
    {
        meleeCombatManager = this;
        firstExchange = true;
        bouts = new List<Bout>();
    }

    public void ResolveExchanges(bool playAnimations=false) {
        
        List<(Bout, Exchange, Exchange, int, bool)> exchanges = new List<(Bout, Exchange, Exchange, int, bool)>();

        foreach (var rn in redNone) {
            var exchange = rn.Item2;
            int e1tn = exchange.offensiveManuever.GetTargetNumber(exchange.attacker, exchange.meleeDamageType);
            int e1Success = DiceRoller.GetSuccess(exchange.attacker.characterSheet.meleeCombatStats.reflexes, e1tn);
            exchanges.Add((rn.Item1, rn.Item2, null, e1Success, false));
        }

        foreach (var rb in redBlue)
        {
            var exchange = rb.Item2;
            int e1tn = exchange.offensiveManuever.GetTargetNumber(exchange.attacker, exchange.meleeDamageType);
            int e1Success = DiceRoller.GetSuccess(exchange.attacker.characterSheet.meleeCombatStats.reflexes, e1tn);
            exchanges.Add((rb.Item1, rb.Item2, null, e1Success, true));
        }

        foreach (var rr in redRed) {
            var e1 = rr.Item2;
            var e2 = rr.Item3;
            int e1tn = e1.offensiveManuever.GetTargetNumber(e1.attacker, e1.meleeDamageType);
            int e2tn = e2.offensiveManuever.GetTargetNumber(e2.attacker, e2.meleeDamageType);
            int e1Success = DiceRoller.GetSuccess(e1.attacker.characterSheet.meleeCombatStats.reflexes, e1tn);
            int e2Success = DiceRoller.GetSuccess(e2.attacker.characterSheet.meleeCombatStats.reflexes, e2tn);
            exchanges.Add((rr.Item1, rr.Item2, rr.Item3, e1Success > e2Success ? e1Success : e2Success, true));
        }

        var sortedExchanges = exchanges.OrderByDescending(t => t.Item4).ToList();

        ResolveListOfExchanges(sortedExchanges);

        foreach (var bb in blueBlue)
            bb.BlueBlue();

        firstExchange = !firstExchange;

        if (firstExchange)
            RefreshCP();

        foreach (var bout in bouts) {
            bout.combatantA.selectManuever = null;
            bout.combatantB.selectManuever = null;

            if (bout.initativeCombatant != null)
                return;
            bout.combatantA.meleeDecision = MeleeStatus.UNDECIDED;
            bout.combatantB.meleeDecision = MeleeStatus.UNDECIDED;
        }

        StandUpTests();
        ResetCombatants();
    }


    private void ResetCombatants()
    {
        foreach (var bout in bouts)
        {
            bout.combatantA.selectManuever = null;
            bout.combatantA.shieldBeaten = false;
            bout.combatantA.weaponBeaten = false;
            bout.combatantB.selectManuever = null;
            bout.combatantB.shieldBeaten = false;
            bout.combatantB.weaponBeaten = false;
        }
    }

    private void StandUpTests() {
        foreach (var bout in bouts)
        {
            if (bout.combatantA.knockedDown && bout.combatantA.currentDice > 0) {
                bout.combatantA.knockedDown = false;
                bout.combatantA.currentDice /= 2;
                Debug.Log("Combatant("+bout.combatantA.characterSheet.name
                    +") stood up. Remaining Dice: "+bout.combatantA.currentDice);
                CombatLog.Log("Combatant(" + bout.combatantA.characterSheet.name
                    + ") stood up. Remaining Dice: " + bout.combatantA.currentDice);
            }
            if (bout.combatantB.knockedDown && bout.combatantA.currentDice > 0)
            {
                bout.combatantB.knockedDown = false;
                bout.combatantB.currentDice /= 2;
                Debug.Log("Combatant(" + bout.combatantB.characterSheet.name 
                    + ") stood up. Remaining Dice: " + bout.combatantB.currentDice);
                CombatLog.Log("Combatant(" + bout.combatantB.characterSheet.name
                    + ") stood up. Remaining Dice: " + bout.combatantB.currentDice);
            }
        }
    }

    private void RefreshCP() {

        foreach (var bout in bouts) {
            bout.combatantA.RefillDice();
            bout.combatantB.RefillDice();
            bout.onPause = false;
        }

    }

    private void ResolveListOfExchanges(List<(Bout, Exchange, Exchange, int, bool)> exchanges) {

        foreach (var exchange in exchanges) {

            if (exchange.Item3 == null && !exchange.Item5)
            {
                exchange.Item1.RedNone(exchange.Item2);
            } else if (exchange.Item3 == null && exchange.Item5) {
                exchange.Item1.RedBlue(exchange.Item2);
            }
            else {
                exchange.Item1.RedRed(exchange.Item2, exchange.Item3);
            }
        }

    }

    public void CreateExchanges() {

        redNone = new List<(Bout, Exchange)>();
        redBlue = new List<(Bout, Exchange)>();
        redRed = new List<(Bout, Exchange, Exchange)>();
        blueBlue = new List<Bout>();

        foreach (var bout in bouts) {

            bool combatantARed = bout.combatantA.selectManuever != null && bout.combatantA.selectManuever.meleeStatus == MeleeStatus.RED;
            bool combatantBRed = bout.combatantB.selectManuever != null && bout.combatantB.selectManuever.meleeStatus == MeleeStatus.RED;

            if (bout.onPause) {
                blueBlue.Add(bout);
                continue; 
            }

            if (combatantARed && combatantBRed)
            {
                var e1 = CreateExchange(bout, bout.combatantA, bout.combatantB);
                var e2 = CreateExchange(bout, bout.combatantB, bout.combatantA);
                redRed.Add((bout, e1, e2));
                //bout.RedRed(e1, e2);
            }
            else if (combatantARed)
            {
                CreateSingleRed(bout, bout.combatantA, bout.combatantB);
            }
            else if (combatantBRed)
            {
                CreateSingleRed(bout, bout.combatantB, bout.combatantA);
            }
            else {
                blueBlue.Add(bout);
                //bout.BlueBlue();
            }

        }
    }

    private void CreateSingleRed(Bout bout, Combatant attackingCombatant, Combatant defendingCombatant) {
        if (defendingCombatant.selectManuever.meleeStatus == MeleeStatus.LEAVE_COMBAT)
        {
            redNone.Add((bout, CreateExchange(bout, attackingCombatant, defendingCombatant)));
            //bout.RedNone(CreateExchange(bout, attackingCombatant, defendingCombatant));
        }
        else
        {
            redBlue.Add((bout, CreateExchange(bout, attackingCombatant, defendingCombatant)));
            //bout.RedBlue(CreateExchange(bout, attackingCombatant, defendingCombatant));
        }
    }

    private Exchange CreateExchange(Bout bout, Combatant attacker, Combatant defender) {

        Exchange exchange;

        if (defender.selectManuever.defensiveManuever == null)
        {
            exchange = new Exchange(bout, attacker, defender, attacker.selectManuever, null);
        }
        else {
            exchange = new Exchange(bout, attacker, defender, attacker.selectManuever, defender.selectManuever);
        }

        return exchange;
    }

    public Dictionary<Bout, List<Combatant>> GetAttackers() {

        var attackers = new Dictionary<Bout, List<Combatant>>();

        foreach (var bout in bouts) {
            var (attackerA, attackerB) = GetAttackersInBout(bout);
            if (attackerA != null)
                AddCombatant(attackers, bout, attackerA);
            if (attackerB != null)
                AddCombatant(attackers, bout, attackerB);
        }

        return attackers;
    }

    public Dictionary<Bout, List<Combatant>> GetAttackersWithoutManeuver()
    {

        var attackers = new Dictionary<Bout, List<Combatant>>();

        foreach (var bout in bouts)
        {
            var (attackerA, attackerB) = GetAttackersInBout(bout);
            if (attackerA != null && attackerA.selectManuever == null)
                AddCombatant(attackers, bout, attackerA);
            if (attackerB != null && attackerB.selectManuever == null)
                AddCombatant(attackers, bout, attackerB);
        }

        return attackers;
    }

    public Dictionary<Bout, List<Combatant>> GetDefenders() {
        var defenders = new Dictionary<Bout, List<Combatant>>();

        foreach (var bout in bouts) {
            var (defenderA, defenderB) = GetDefendersInBout(bout);
            if (defenderA != null)
                AddCombatant(defenders, bout, defenderA);
            if (defenderB != null)
                AddCombatant(defenders, bout, defenderB);
        }

        return defenders;
    }

    public Dictionary<Bout, List<Combatant>> GetDefendersWithoutManuever()
    {
        var defenders = new Dictionary<Bout, List<Combatant>>();

        foreach (var bout in bouts)
        {
            var (defenderA, defenderB) = GetDefendersInBout(bout);
            if (defenderA != null && defenderA.selectManuever == null)
                AddCombatant(defenders, bout, defenderA);
            if (defenderB != null && defenderB.selectManuever == null)
                AddCombatant(defenders, bout, defenderB);
        }

        return defenders;
    }

    public List<Bout> GetDeclareBouts() {
        List<Bout> declareBouts = new List<Bout>();

        foreach (var bout in bouts) {
            if (bout.onPause)
                continue;

            if ((bout.combatantA.meleeDecision == MeleeStatus.UNDECIDED || bout.combatantB.meleeDecision == MeleeStatus.UNDECIDED)
                && bout.initativeCombatant == null) {
                declareBouts.Add(bout);
            }
        }

        return declareBouts; 
    }

    private void AddCombatant(Dictionary<Bout, List<Combatant>> dictionary, Bout bout, Combatant combatant) {

        if (dictionary.ContainsKey(bout))
        {
            dictionary[bout].Add(combatant);
        }
        else {
            dictionary.Add(bout, new List<Combatant> { combatant });
        }

    }

    private (Combatant, Combatant) GetDefendersInBout(Bout bout) {
        if (bout.initativeCombatant != null && bout.combatantA == bout.initativeCombatant)
        {
            return (null, bout.combatantB);
        }
        else if (bout.initativeCombatant != null && bout.combatantB == bout.initativeCombatant)
        {
            return (bout.combatantA, null);
        }
        else if (bout.combatantA.meleeDecision == MeleeStatus.BLUE &&
              bout.combatantB.meleeDecision == MeleeStatus.BLUE)
        {
            return (null, null);
        }
        else if (bout.combatantA.meleeDecision == MeleeStatus.BLUE)
        {
            return (bout.combatantA, null);
        }
        else if (bout.combatantB.meleeDecision == MeleeStatus.BLUE)
        {
            return (null, bout.combatantB);
        }

        return (null, null);
    }

    private (Combatant, Combatant) GetAttackersInBout(Bout bout) {
        if (bout.initativeCombatant != null && bout.combatantA == bout.initativeCombatant) {
            return (bout.combatantA, null);
        } else if (bout.initativeCombatant != null && bout.combatantB == bout.initativeCombatant) {
            return (null, bout.combatantB);
        } else if (bout.combatantA.meleeDecision == MeleeStatus.RED &&
               bout.combatantB.meleeDecision == MeleeStatus.RED) {
            return (bout.combatantA, bout.combatantB);
        } else if (bout.combatantA.meleeDecision == MeleeStatus.RED) {
            return (bout.combatantA, null);
        } else if (bout.combatantB.meleeDecision == MeleeStatus.RED) {
            return (null, bout.combatantB);
        }

        return (null, null);
    }

    public void EnterMeleeCombat(CharacterSheet initator, CharacterSheet target) {
        foreach (var bout in bouts) {
            if ((bout.combatantA.characterSheet == initator && bout.combatantB.characterSheet == target) ||
                (bout.combatantB.characterSheet == initator && bout.combatantA.characterSheet == target)) {
                throw new System.Exception("Characters already in bout with each other.");
            }
        }

        Combatant combatantA = new Combatant(initator, initator.meleeCombatStats.weapon, 0);
        Combatant combatantB = new Combatant(target, target.meleeCombatStats.weapon, 0);

        Bout newBout = new Bout(combatantA, combatantB);

        bouts.Add(newBout);

    }

    public void Declare(Combatant combatant, MeleeStatus meleeDecision) {
        combatant.meleeDecision = meleeDecision;

        if (meleeDecision == MeleeStatus.LEAVE_COMBAT)
            combatant.SetSelectedManeuver(new SelectManuever());

    }

    public void PickCommittedCP(Combatant combatant, int number) {
        combatant.diceAssignedToBout = number;
    }

    public Bout FindBout(string combatantAName, string combatantBName)
    {
        foreach (var bout in bouts)
        {
            if (bout.combatantA.characterSheet.name == combatantAName && bout.combatantB.characterSheet.name == combatantBName)
            {
                return bout;
            }
            else if (bout.combatantA.characterSheet.name == combatantBName && bout.combatantB.characterSheet.name == combatantAName)
            {
                return bout;
            }
        }
        return null;
        //Debug.Log("bout not found between these two combatants: "+combatantAName+", "+combatantBName);
    }

    public static int GetReachCost(Combatant combatant, Combatant targetCombatant, Bout bout) {
        int reachCost = 0;
        int combatantReach = combatant.meleeWeaponStatBlock.reach;
        int targetComtantantReach = targetCombatant.meleeWeaponStatBlock.reach;

        if (bout.reachCombatant == null && targetComtantantReach > combatantReach)
        {
            reachCost = targetComtantantReach - combatantReach;
        }
        else if (bout.reachCombatant == targetCombatant)
        {
            reachCost = Mathf.Abs(targetComtantantReach - combatantReach);
        }

        return reachCost;
    }

    public static List<Bout> GetBouts(string name)
    {
        List<Bout> bouts = new List<Bout>();

        foreach (var b in meleeCombatManager.bouts)
        {
            if (b.combatantA.characterSheet.name == name ||
                b.combatantB.characterSheet.name == name)
            {
                bouts.Add(b);
            }
        }

        return bouts;
    }

    public static List<Combatant> GetCombatants(string name, bool opposing = false)
    {
        List<Combatant> combatants = new List<Combatant>();

        var bouts = GetBouts(name);

        foreach (var bout in bouts)
        {
            if (bout.combatantA.characterSheet.name == name)
            {
                combatants.Add(opposing ? bout.combatantB : bout.combatantA);
            }
            else
            {
                combatants.Add(opposing ? bout.combatantA : bout.combatantB);
            }
        }


        return combatants;
    }


}