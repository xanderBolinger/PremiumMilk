using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeleeCombatController;

[RequireComponent(typeof(CharacterNetwork))]
public class CharacterCombatController : MonoBehaviour
{
    CharacterNetwork characterNetwork;

    private void Start()
    {
        characterNetwork = GetComponent<CharacterNetwork>();
    }

    public void EnterCombat(GameObject target) {
        if (target == null)
            return;

        var distance = Distance(target);

        if (distance > 1)
            return;

        EnterCombat(target.GetComponent<CharacterNetwork>().characterName);
    }

    private int Distance(GameObject target) {
        var targetGridInfo = target.GetComponent<CharacterGridInfo>();
        var characterGridInfo = target.GetComponent<CharacterGridInfo>();

        return Math.Abs(targetGridInfo.standingOnX - characterGridInfo.standingOnX) 
            + Math.Abs(targetGridInfo.standingOnY - characterGridInfo.standingOnY);
    }

    private void AssignDice(int dice = 0, bool manual = false)
    {
        var cs = characterNetwork.GetCharacterSheet();
        var name = characterNetwork.GetCharacterSheet().name;
        List<Bout> bouts = GetCharacterBouts(name);
        List<Combatant> combatants = GetCharacterCombatants(bouts, name);

        bool firstCombat = combatants.Count == 1;
        int currentDiceTotal = 0;

        foreach (var c in combatants)
        {
            currentDiceTotal += c.currentDice;
            c.AssignCP(0, 0);
        }

        int maxCp = cs.meleeCombatStats.GetMaxCp(cs.medicalData.GetPain());

        int diceAssignedToOtherBouts = 0;

        foreach (var c in combatants)
        {
            int bonus = (maxCp % bouts.Count == 0 ? 0 : 1);

            c.AssignCP((maxCp / bouts.Count)
                + (combatants[combatants.Count - 1] == c ? bonus : 0),
                diceAssignedToOtherBouts);
            diceAssignedToOtherBouts += maxCp / bouts.Count;

        }

        foreach (var c in combatants)
        {

            int bonus = (maxCp % bouts.Count == 0 ? 0 : 1);

            if (firstCombat)
            {
                c.currentDice = MeleeCombatManager.meleeCombatManager.firstExchange ? c.diceAssignedToBout : (c.diceAssignedToBout / 2)
                    + (combatants[combatants.Count - 1] == c ? bonus : 0);
            }
            else
            {
                c.currentDice = (currentDiceTotal / bouts.Count)
                    + (combatants[combatants.Count - 1] == c ? bonus : 0);
            }

        }

    }

    private List<Combatant> GetCharacterCombatants(List<Bout> bouts, string name)
    {
        List<Combatant> combatants = new List<Combatant>();

        foreach (var bout in bouts)
        {
            Combatant combatant = null;

            if (bout.combatantA.characterSheet.name == name)
                combatant = bout.combatantA;
            else if (bout.combatantB.characterSheet.name == name)
                combatant = bout.combatantB;
            combatants.Add(combatant);
        }

        return combatants;
    }

    private List<Bout> GetCharacterBouts(string name)
    {

        List<Bout> bouts = new List<Bout>();
        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts)
        {
            if (bout.combatantA.characterSheet.name == name)
            {
                bouts.Add(bout);
            }
            else if (bout.combatantB.characterSheet.name == name)
            {
                bouts.Add(bout);
            }
        }
        return bouts;
    }

    public void EnterCombat(string targetName)
    {
        var characterName = GetComponent<CharacterNetwork>().GetCharacterSheet().name;
        var bout = MeleeCombatManager.meleeCombatManager.FindBout(characterName, targetName);
        if (bout != null || characterName == targetName) return;
        meleeCombatController.selectedCharacterIndex
                = meleeCombatController.selectedCharacterList.IndexOf(characterName);
        meleeCombatController.targetCharacterIndex
            = meleeCombatController.targetCharacterList.IndexOf(targetName);
        meleeCombatController.EnterCombat();
        AssignDice();

        var targetObj = CharacterController.GetCharacterObject(targetName);
        var targetGridController = targetObj.GetComponent<CharacterCombatController>();
        targetGridController.AssignDice();
    }

}
