using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelUtillity.MeleeHitLocation;
using static MeleeCombatManager;
using static OffensiveManuevers;
using Character;
using static DefensiveManuevers;

public static class MeleeCombatAI
{

    public static SelectManuever SetDefense(Combatant attacker, Combatant defender, MeleeCombatAIData ai) {

        double defenseBias = 0;

        switch (ai.behavior)
        {
            case MeleeCombatAIData.BehaviorType.Agressive:
                defenseBias = 0;
                break;
            case MeleeCombatAIData.BehaviorType.Cowardly:
                defenseBias = 2.0;
                break;
            case MeleeCombatAIData.BehaviorType.Measured:
                defenseBias = 1.0;
                break;
            case MeleeCombatAIData.BehaviorType.Feral:
                defenseBias = 0;
                break;
        }

        DefensiveManueverType defensiveManueverType;
        
        int dice;

        var attackerSelectManuever = attacker.selectManuever;

        int attackerDice = attackerSelectManuever.dice;
        int attackerTn = attackerSelectManuever.offensiveManuever.GetTargetNumber(attacker, attacker.selectManuever.meleeDamageType);
        int predicatedAttackerSuccess = (int)((1.0-((double)attackerTn / 10.0)) * (double)attackerDice);

        defensiveManueverType = DefensiveManueverType.DODGE;
        int tn = 7;

        if (!defender.shieldBeaten && defender.characterSheet.meleeCombatStats.shield != null)
        {
            defensiveManueverType = DefensiveManueverType.BLOCK;
            tn = defender.characterSheet.meleeCombatStats.shield.DTN;
            dice = (int)(((double)predicatedAttackerSuccess+ defenseBias) / (1.0 - ((double)tn / 10.0)));
        }
        else if (defender.meleeWeaponStatBlock.dtn <= 7)
        {
            defensiveManueverType = DefensiveManueverType.PARRY;
            tn = defender.meleeWeaponStatBlock.dtn;
            dice = (int)(((double)predicatedAttackerSuccess+ defenseBias) / (1.0 - ((double)tn / 10.0)));
        }
        else {
            dice = (int)(((double)predicatedAttackerSuccess+ defenseBias) / (1.0 - ((double)tn / 10.0)));
        }

        if (dice > defender.currentDice)
            dice = defender.currentDice;

        if (!meleeCombatManager.firstExchange)
            dice = defender.currentDice;

        return new SelectManuever(defensiveManueverType, dice, 0);
    }

    public static SelectManuever SetAttack(int attackerDice, int reachCost, MeleeCombatAIData ai, MeleeWeaponStatBlock attackerWeapon, 
        List<ArmorPiece> defenderArmor, bool redRed) {

        OffensiveManueverType offensiveManueverType = OffensiveManueverType.CUT;
        MeleeDamageType meleeDamageType = MeleeDamageType.CUTTING;
        int dice, additionalCost;

        dice = 0;
        additionalCost = reachCost;

        if (!meleeCombatManager.firstExchange || redRed)
        {
            dice = attackerDice - additionalCost;
        }
        else {
            switch (ai.behavior)
            {
                case MeleeCombatAIData.BehaviorType.Agressive:
                    dice = (int)((double) (attackerDice-additionalCost) / 1.5);
                    break;
                case MeleeCombatAIData.BehaviorType.Cowardly:
                    dice = (int)((double)(attackerDice - additionalCost) / 2.5);
                    break;
                case MeleeCombatAIData.BehaviorType.Measured:
                    dice = (int)((double)(attackerDice - additionalCost) / 2);
                    break;
                case MeleeCombatAIData.BehaviorType.Feral:
                    dice = attackerDice - additionalCost;
                    break;
            }
        }

        if (attackerWeapon.thrustMod > attackerWeapon.cutMod) {
            offensiveManueverType = OffensiveManueverType.THRUST;
            meleeDamageType = MeleeDamageType.PIERICNG;
        }
        if (attackerWeapon.bluntMod > attackerWeapon.cutMod && attackerWeapon.bluntMod > attackerWeapon.thrustMod) {
            offensiveManueverType = OffensiveManueverType.BASH;
            meleeDamageType = MeleeDamageType.BLUNT;
        }


        return new SelectManuever(offensiveManueverType, dice, meleeDamageType == MeleeDamageType.PIERICNG ? GetTargetZone(defenderArmor, true) :
            GetTargetZone(defenderArmor), meleeDamageType, additionalCost);
    }

    public static int GetTargetZone(List<ArmorPiece> defenderArmor, bool stabbing=false) {
        bool headLowerProtected = GetProtectedArea(ArmorPiece.locationData.headLower, defenderArmor);
        bool headUpperProtected = GetProtectedArea(ArmorPiece.locationData.headUpper, defenderArmor);

        bool torsoLowerProtected = GetProtectedArea(ArmorPiece.locationData.torsoLower, defenderArmor);
        bool torsoUpperProtected = GetProtectedArea(ArmorPiece.locationData.torsoUpper, defenderArmor);

        bool armsLowerProtected = GetProtectedArea(ArmorPiece.locationData.armsLower, defenderArmor);
        bool armsUpperProtected = GetProtectedArea(ArmorPiece.locationData.armsUpper, defenderArmor);

        bool legsLowerProtected = GetProtectedArea(ArmorPiece.locationData.legsLower, defenderArmor);
        bool legsUpperProtected = GetProtectedArea(ArmorPiece.locationData.legsUpper, defenderArmor);

        if (!headLowerProtected && !headUpperProtected)
        {
            return stabbing ? ArmorPiece.StabbingPartToZone("Face") : ArmorPiece.SlashingPartToZone("Face");
        }
        else if (!torsoLowerProtected && !torsoUpperProtected)
        {
            return stabbing ? ArmorPiece.StabbingPartToZone("Ribcage") : ArmorPiece.SlashingPartToZone("Ribcage");
        }
        else if (!armsLowerProtected && !armsUpperProtected)
        {
            return stabbing ? ArmorPiece.StabbingPartToZone("Forearm") : ArmorPiece.SlashingPartToZone("Forearm");
        }
        else if (!legsLowerProtected && !legsUpperProtected)
        {
            return stabbing ? ArmorPiece.StabbingPartToZone("Shin") : ArmorPiece.SlashingPartToZone("Shin");
        }
        else {

            int av = defenderArmor[0].ArmorValue;
            string zone = defenderArmor[0].ProtectedBodyParts[0];

            foreach(var armor in defenderArmor) {
                if (armor.ArmorValue < av) {
                    av = armor.ArmorValue;
                    zone = armor.ProtectedBodyParts[0];
                }
            }

            return stabbing? ArmorPiece.StabbingPartToZone(zone) : ArmorPiece.SlashingPartToZone(zone);

        }

    }

    private static bool GetProtectedArea(List<string> parts, List<ArmorPiece> defenderArmor)
    {
        foreach (var part in ArmorPiece.locationData.legsUpper)
        {
            foreach (var armor in defenderArmor)
                if (armor.Protected(part))
                    return true;
        }

        return false;
    }

    public static MeleeStatus DeclareAi(string characterName, string targetName, MeleeCombatAIData aiData) {

        var bout = meleeCombatManager.FindBout(characterName, targetName);

        var (ownedCp, opposingCp) = GetCp(characterName);

        if (opposingCp < 1)
            opposingCp = 1;

        double ratio = ((double)ownedCp) / ((double)opposingCp);

        switch (aiData.behavior)
        {
            case MeleeCombatAIData.BehaviorType.Agressive:
                int tn = 1;
                if (ratio >= 1.0)
                    return MeleeStatus.RED;
                else if (ratio <= 0.5)
                    tn = 3;
                else if (ratio <= 0.66)
                    tn = 2;
                

                int roll = DiceRoller.Roll(1,4);
                if (roll > tn)
                    return MeleeStatus.RED;
                else
                    return MeleeStatus.BLUE;
            case MeleeCombatAIData.BehaviorType.Cowardly:
                int tn2 = 1;
                if (ratio >= 1.0 && ratio <= 1.5)
                    tn2 = 2;
                else if (ratio <= 0.5)
                    return MeleeStatus.BLUE;
                else if (ratio <= 0.66)
                    tn2 = 3;

                int roll2 = DiceRoller.Roll(1, 4);
                if (roll2 > tn2)
                    return MeleeStatus.RED;
                else
                    return MeleeStatus.BLUE;
            case MeleeCombatAIData.BehaviorType.Measured:
                int tn3 = 1;
                if (ratio >= 1.0 && ratio <= 1.5)
                    tn3 = 1;
                else if (ratio <= 0.5)
                    tn3 = 3;
                else if (ratio <= 0.66)
                    tn3 = 2;

                int roll3 = DiceRoller.Roll(1, 4);
                if (roll3 > tn3)
                    return MeleeStatus.RED;
                else
                    return MeleeStatus.BLUE;

            case MeleeCombatAIData.BehaviorType.Feral:
                return MeleeStatus.RED;
        }

        return MeleeStatus.BLUE;
    }
    


    

    public static (int, int) GetCp(string name) {

        var ownedCombatants = GetCombatants(name);
        var opposingCombatants = GetCombatants(name, true);

        int ownedCp = 0;
        int opposingCp = 0;

        foreach (var c in ownedCombatants) {
            ownedCp += c.currentDice;
        }

        foreach (var c in opposingCombatants)
        {
            opposingCp += c.currentDice;
        }

        return (ownedCp, opposingCp);
    }

}
