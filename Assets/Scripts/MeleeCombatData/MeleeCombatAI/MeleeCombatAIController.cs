using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeleeCombatManager;
using static MeleeCombatController;

public static class MeleeCombatAIController
{

    public static void DeclareDefense() {
        var defenders = meleeCombatManager.GetDefendersWithoutManuever();
        foreach (var bout in meleeCombatManager.bouts)
        {
            var nameA = bout.combatantA.characterSheet.name;
            var nameB = bout.combatantB.characterSheet.name;
            if (!IsPlayer(nameA) && defenders.ContainsKey(bout) && defenders[bout].Contains(bout.combatantA))
            {
                Debug.Log("AI set Defense for: " + nameA);
                SetDefense(bout.combatantB, bout.combatantA, bout);
            }
            if (!IsPlayer(nameB) && defenders.ContainsKey(bout) && defenders[bout].Contains(bout.combatantB))
            {
                Debug.Log("AI set attack for: " + nameB);
                SetDefense(bout.combatantA, bout.combatantB, bout);
            }
        }
    }

    private static void SetDefense(Combatant attacker, Combatant defender, Bout bout) {
        var ai = CharacterController.GetCharacterObject(defender.characterSheet.name).GetComponent<MeleeCombatAIData>();
        var manuever = MeleeCombatAI.SetDefense(attacker, defender, ai);


        meleeCombatController.selectedCharacterIndex = meleeCombatController.selectedCharacterList.IndexOf(
            defender.characterSheet.name);
        meleeCombatController.selectedBoutIndex = meleeCombatManager.bouts.IndexOf(bout);
        meleeCombatController.defensiveManueverType = manuever.defensiveManuever.manueverType;
        meleeCombatController.dice = manuever.dice;
        meleeCombatController.secondaryDice = manuever.secondaryDicePool;
        meleeCombatController.SetDefense();
    }

    public static void DeclareAttack() {
        var attackers = meleeCombatManager.GetAttackersWithoutManeuver();
        foreach (var bout in meleeCombatManager.bouts)
        {
            var nameA = bout.combatantA.characterSheet.name;
            var nameB = bout.combatantB.characterSheet.name;
            if (!IsPlayer(nameA) && attackers.ContainsKey(bout) && attackers[bout].Contains(bout.combatantA))
            {
                Debug.Log("AI set attack for: "+nameA);
                SetAttack(nameA, bout);
            }
            if (!IsPlayer(nameB) && attackers.ContainsKey(bout) && attackers[bout].Contains(bout.combatantB)) { 
                Debug.Log("AI set attack for: " + nameB);
                SetAttack(nameB, bout);
            }
        }
    }

    private static void SetAttack(string nameA, Bout bout) {

        var ai = CharacterController.GetCharacterObject(nameA).GetComponent<MeleeCombatAIData>();
        
        meleeCombatController.selectedCharacterIndex = meleeCombatController.selectedCharacterList.IndexOf(nameA);
        meleeCombatController.selectedBoutIndex = meleeCombatManager.bouts.IndexOf(bout);

        var attacker = meleeCombatController.GetCombatant();
        var defender = meleeCombatController.GetTargetCombatant();

        var selectManuever = MeleeCombatAI.SetAttack(attacker.currentDice, meleeCombatController.GetReachCost(), ai,
            attacker.meleeWeaponStatBlock, defender.characterSheet.meleeCombatStats.armorPieces, defender.meleeDecision == MeleeStatus.RED && attacker.meleeDecision == MeleeStatus.RED);

        meleeCombatController.offensiveManueverType = selectManuever.offensiveManuever.manueverType;
        meleeCombatController.meleeDamageType = selectManuever.meleeDamageType;
        meleeCombatController.dice = selectManuever.dice;
        meleeCombatController.secondaryDice = selectManuever.secondaryDicePool;
        meleeCombatController.targetZoneCutting = (TargetZone.TargetZoneCutting)Enum.ToObject(typeof(TargetZone.TargetZoneCutting), selectManuever.targetZone);
        meleeCombatController.targetZonePuncture = (TargetZone.TargetZonePuncture)Enum.ToObject(typeof(TargetZone.TargetZonePuncture), selectManuever.targetZone);

        meleeCombatController.SetAttack();

    }

    public static void DeclareAI() {
        foreach (var bout in meleeCombatManager.GetDeclareBouts()) {
            var nameA = bout.combatantA.characterSheet.name;
            var nameB = bout.combatantB.characterSheet.name;
            if (!IsPlayer(nameA)) {
                SetDeclare(nameA, nameB, bout);
            } 
            if (!IsPlayer(nameB)) {
                SetDeclare(nameB, nameA, bout);
            }
        }
    }

    private static void SetDeclare(string nameA, string nameB, Bout bout) {
        var ai = CharacterController.GetCharacterObject(nameA).GetComponent<MeleeCombatAIData>();
        meleeCombatController.selectedCharacterIndex = meleeCombatController.selectedCharacterList.IndexOf(nameA);
        meleeCombatController.selectedBoutIndex = meleeCombatManager.bouts.IndexOf(bout);
        meleeCombatController.meleeDecision = MeleeCombatAI.DeclareAi(nameA, nameB, ai);
        meleeCombatController.Declare();
    }


    private static bool IsPlayer(string name) {
        var c = CharacterController.GetCharacterObject(name);
        if (c != null)
            return c.GetComponent<CharacterController>().player;

        return true; 
    }

}
