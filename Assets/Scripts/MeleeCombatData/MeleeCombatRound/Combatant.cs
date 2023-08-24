using Character;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MeleeCombatManager;

public class Combatant
{
    public CharacterSheet characterSheet;
    public MeleeWeaponStatBlock meleeWeaponStatBlock;
    public int diceAssignedToBout;
    public int currentDice;
    public int penalty;
    public SelectManuever selectManuever { set; get; }
    public MeleeStatus meleeDecision;
    public bool knockedDown;
    public bool weaponBeaten;
    public bool shieldBeaten;

    public Combatant() { }

    public Combatant(CharacterSheet characterSheet, MeleeWeaponStatBlock meleeWeaponStatBlock, int diceAssignedToBout)
    {
        this.characterSheet = characterSheet;
        this.meleeWeaponStatBlock = meleeWeaponStatBlock;
        this.diceAssignedToBout = diceAssignedToBout;
        currentDice = diceAssignedToBout;
        meleeDecision = MeleeStatus.UNDECIDED;
        this.knockedDown = false;
    }

    public void ApplyShock(int shockPD)
    {
        int shockPenalty = CalculateShockPenalty(shockPD);

        while (shockPenalty > 0)
        {
            foreach (var bout in meleeCombatManager.bouts)
            {
                if (IsCharacterCombatant(bout.combatantA))
                {
                    ApplyShockToCombatant(bout.combatantA, ref shockPenalty);
                }
                else if (IsCharacterCombatant(bout.combatantB))
                {
                    ApplyShockToCombatant(bout.combatantB, ref shockPenalty);
                }

                if (shockPenalty <= 0)
                    break;
            }
        }
    }

    private int CalculateShockPenalty(int shockPD)
    {
        int shockPenalty = shockPD / 20;
        shockPenalty -= characterSheet.attributes.wil / 6;
        return shockPenalty;
    }

    private bool IsCharacterCombatant(Combatant combatant)
    {
        return combatant.characterSheet == characterSheet;
    }

    private void ApplyShockToCombatant(Combatant combatant, ref int shockPenalty)
    {
        if (MeleeCombatManager.meleeCombatManager.firstExchange || currentDice > 0)
        {
            ReduceCurrentDice(combatant);
            shockPenalty--;
        }
        else if (currentDice <= 0)
        {
            shockPenalty--;
            IncreasePenalty(combatant);
        }
    }

    private void ReduceCurrentDice(Combatant combatant)
    {
        combatant.currentDice--;
    }

    private void IncreasePenalty(Combatant combatant)
    {
        combatant.penalty++;
    }

    public void ApplyPain(int pain)
    {
        pain -= characterSheet.attributes.wil / 3;

        while (pain > 0)
        {
            foreach (var bout in meleeCombatManager.bouts)
            {
                if (IsCharacterCombatant(bout.combatantA))
                {
                    ApplyPainToCombatant(bout.combatantA, ref pain);
                }
                else if (IsCharacterCombatant(bout.combatantB))
                {
                    ApplyPainToCombatant(bout.combatantB, ref pain);
                }

                if (pain <= 0)
                    break;
            }
        }
    }

    private void ApplyPainToCombatant(Combatant combatant, ref int pain)
    {
        combatant.diceAssignedToBout--;
        pain--;

        if (combatant.diceAssignedToBout < combatant.currentDice)
        {
            combatant.currentDice = combatant.diceAssignedToBout;
        }
    }


    public void RefillDice() {
        currentDice = diceAssignedToBout;
        currentDice -= penalty;
        penalty = 0;
    }

    public void SetSelectedManeuver(SelectManuever man) {
        this.selectManuever = man;
    }

    public bool AssignCP(int cp, int cpAssignedToOtherBouts)
    {
        int pain = characterSheet.medicalData.GetPain();
        int maxCP = characterSheet.meleeCombatStats.GetMaxCp(pain);
        int availableCP = maxCP - cpAssignedToOtherBouts;

        if (cp > availableCP)
            return false;

        diceAssignedToBout = cp;

        return true;

    }

}
