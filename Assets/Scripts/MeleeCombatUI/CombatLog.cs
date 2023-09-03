using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeleeCombatManager;

public static class CombatLog
{
    public static void Log(string message) {
        LogServer(message);
    }

    public static void LogEnterCombat(string initiator, string target) {
        LogServer(initiator+" has entered melee combat with: "+target);
    }

    public static void LogDeclare() {

        foreach (var bout in meleeCombatManager.bouts) {
            if (bout.initativeCombatant != null)
                return;
        }

        foreach (var bout in meleeCombatManager.bouts) {
            WriteDeclare(bout.combatantA.characterSheet.name, bout.combatantA.meleeDecision);
            WriteDeclare(bout.combatantB.characterSheet.name, bout.combatantB.meleeDecision);
        }


    }

    private static void WriteDeclare(string name, MeleeStatus decision) {
        string msg = name + " has decided to ";

        switch (decision)
        {
            case MeleeStatus.RED:
                msg += "ATTACK!";
                break;
            case MeleeStatus.BLUE:
                msg += "DEFEND!";
                break;
            case MeleeStatus.LEAVE_COMBAT:
                msg += "LEAVE COMBAT!";
                break;
            case MeleeStatus.UNDECIDED:
                msg += "do nothing.";
                break;
        }

        LogServer(msg);

    }


    private static void LogServer(string msg) {
        MeleeCombatUI.AddLogServer(msg);
    }

    public static void LogDead(CharacterSheet characterSheet)
    {
        LogServer(characterSheet.name + " has succomb to their injuries and died.");
    }
}
