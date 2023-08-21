using System.Collections;
using System.Collections.Generic;
using Character;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static ExcelUtillity.MeleeHitLocation;

public class MeleeCombatTests
{
    MeleeCombatManager manager;
    Bout bout;

    [SetUp]
    public void StartUp() {
        
        var obj = new GameObject();
        manager = obj.AddComponent<MeleeCombatManager>();
        manager.firstExchange = true;
        manager.bouts = new List<Bout>();

        MeleeCombatManager.meleeCombatManager = manager;

        var wep = MeleeWeaponLoader.GetWeaponByName("Kern Axe");
        var characterOne = CreateCharater1();
        var characterTwo = CreateCharacter2();
        Combatant combatantOne = new Combatant(characterOne, wep, 10);
        Combatant combatantTwo = new Combatant(characterTwo, wep, 10);
        bout = new Bout(combatantOne, combatantTwo);
        manager.bouts.Add(bout);
        DiceRoller.ClearTestValues();
    }

    public static CharacterSheet CreateCharater1() {
        Species human = new Species();
        human.speciesType = Species.SpeciesType.MEDIUM_SIZE;
        human.arms = 2;
        human.legs = 2;
        human.onlyLegs = false;
        Attributes attributes = new Attributes(10, 10, 10, 10, 10);
        MedicalData medicalData = new MedicalData(attributes, human);
        MeleeCombatStats meleeCombatStats = new MeleeCombatStats();
        meleeCombatStats.LearnProficiency(MeleeProficiencies.CutNThrust, 5);
        meleeCombatStats.SetCurrProf(MeleeProficiencies.CutNThrust);
        meleeCombatStats.CalcReflexes(attributes);
        var characterSheet = new CharacterSheet("test", human, attributes, medicalData, meleeCombatStats);

        var armor = ArmorLoader.ReadArmor();
        characterSheet.meleeCombatStats.armorPieces.Add(armor[0]);
        return characterSheet;
    }

    public static CharacterSheet CreateCharacter2() {
        Species human = new Species();
        human.speciesType = Species.SpeciesType.MEDIUM_SIZE;
        human.arms = 2;
        human.legs = 2;
        human.onlyLegs = false;
        Attributes attributes = new Attributes(10, 10, 10, 10, 10);
        MedicalData medicalData = new MedicalData(attributes, human);
        MeleeCombatStats meleeCombatStats = new MeleeCombatStats();
        meleeCombatStats.LearnProficiency(MeleeProficiencies.CutNThrust, 5);
        meleeCombatStats.SetCurrProf(MeleeProficiencies.CutNThrust);
        meleeCombatStats.CalcReflexes(attributes);
        var characterSheet = new CharacterSheet("test2", human, attributes, medicalData, meleeCombatStats);

        var armor = ArmorLoader.ReadArmor();
        characterSheet.meleeCombatStats.armorPieces.Add(armor[0]);
        return characterSheet;
    }


    // A Test behaves as an ordinary method
    [Test]
    public void RedRedOneKillsOther()
    {
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 10, 5, MeleeDamageType.CUTTING, 0);
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, null);
        Exchange e2 = new Exchange(bout, bout.combatantB, bout.combatantA, attackSelectManuever, null);

        for (int i = 0; i < 3; i++) 
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(4);
        DiceRoller.AddNextTestValue(0);

        bout.RedRed(e1, e2);

        Assert.AreEqual(true, e1.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(false, e2.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(null, bout.reachCombatant);
    }


    [Test]
    public void SimultaneousDefenseSuccess() {
        bout.combatantA.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        bout.combatantB.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        SelectManuever simultaneousManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 5, 5, MeleeDamageType.CUTTING, 0);
        simultaneousManuever.SetSimultaneousDefense(DefensiveManuevers.GetManuever(DefensiveManuevers.DefensiveManueverType.BLOCK), 7);
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 8, 5, MeleeDamageType.CUTTING, 0);
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, simultaneousManuever, null);
        Exchange e2 = new Exchange(bout, bout.combatantB, bout.combatantA, attackSelectManuever, null);

        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 7; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 5; i++)
            DiceRoller.AddNextTestValue(10);
        /*for (int i = 0; i < 6; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 1; i++)
            DiceRoller.AddNextTestValue(5);
        for (int i = 0; i < 1; i++)
            DiceRoller.AddNextTestValue(0);

        bout.RedRed(e1, e2);

        Assert.AreEqual(true, e1.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(false, e2.attacker.characterSheet.medicalData.conscious);
    }

    [Test]
    public void RedRedOneKnocksDownOther()
    {
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 10, 1, MeleeDamageType.CUTTING, 0);
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, null);
        Exchange e2 = new Exchange(bout, bout.combatantB, bout.combatantA, attackSelectManuever, null);

        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(4);
        DiceRoller.AddNextTestValue(0);

        bout.RedRed(e1, e2);

        Assert.AreEqual(true, e1.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(false, e2.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(null, bout.reachCombatant);
        Assert.AreEqual(true, e2.attacker.knockedDown);
    }

    [Test]
    public void RedRedBothHit()
    {
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 10, 5, MeleeDamageType.CUTTING, 0);
        SelectManuever attackSelectManuever2 = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 10, 5, MeleeDamageType.CUTTING, 0);
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, null);
        Exchange e2 = new Exchange(bout, bout.combatantB, bout.combatantA, attackSelectManuever2, null);

        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);

        bout.RedRed(e1, e2);

        Assert.AreEqual(false, e1.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(false, e2.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(null, bout.reachCombatant);
    }

    

    [Test]
    public void RedBlueTakesInitative()
    {
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 7, 5, MeleeDamageType.CUTTING, 0);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 0, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        
        for (int i = 0; i < 7; i++)
            DiceRoller.AddNextTestValue(7);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedBlue(e1);

        Assert.AreEqual(false, e1.defender.characterSheet.medicalData.conscious);
        Assert.AreEqual(bout.initativeCombatant, e1.attacker);
        Assert.AreEqual(e1.attacker, bout.reachCombatant);
    }

    [Test]
    public void RedBlueBlockAndOpenDefense()
    {
        bout.combatantA.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        bout.combatantB.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 
            5, 5, MeleeDamageType.CUTTING, 0);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.
            DefensiveManueverType.BLOCK_AND_OPEN_STRIKE, 6, 0);
        defendSelectManuever.SetSimultaneousAttack(OffensiveManuevers.GetManuever(OffensiveManuevers.OffensiveManueverType.CUT),
            3, 5, MeleeDamageType.CUTTING);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        //bout.RedRed(e1, e2);
        for (int i = 0; i < 5; i++)
            DiceRoller.AddNextTestValue(0);
        for (int i = 0; i < 6; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);

        bout.RedBlue(e1);

        Assert.AreEqual(false, e1.attacker.characterSheet.medicalData.conscious);
        Assert.AreEqual(bout.initativeCombatant, e1.defender);
        Assert.AreEqual(e1.defender, bout.reachCombatant);
    }


    [Test]
    public void RedBlueNoDiceAfterAttack() {
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 10, 5, MeleeDamageType.CUTTING , 0);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 0, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        /*for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(10);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedBlue(e1);
        Assert.AreEqual(10-e1.defender.characterSheet.medicalData.GetPain(), e1.defender.diceAssignedToBout);
        Assert.AreEqual(false, e1.defender.characterSheet.medicalData.conscious);
        Assert.AreEqual(null, bout.initativeCombatant);
        Assert.AreEqual(null, bout.reachCombatant);
    }

    [Test]
    public void RedNone() {

        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.CUT, 9, 5, MeleeDamageType.CUTTING, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, null);

        /*for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 9; i++)
            DiceRoller.AddNextTestValue(9);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedNone(e1);

        Assert.AreEqual(false, e1.defender.characterSheet.medicalData.conscious);
        Assert.AreEqual(e1.attacker, bout.initativeCombatant);
        Assert.AreEqual(e1.attacker, bout.reachCombatant);
    }

    [Test]
    public void BlueBlue() {
        bout.BlueBlue();
        Assert.AreEqual(null, bout.initativeCombatant);
        Assert.AreEqual(null, bout.reachCombatant);
    }

    [Test]
    public void RedBlueBindAndStrikeFirstExchange()
    {
        bout.combatantA.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        bout.combatantB.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.BIND_AND_STRIKE, 10, 5, MeleeDamageType.CUTTING, 0);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 0, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        /*for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(6);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedBlue(e1);
        Assert.AreEqual(0, e1.defender.currentDice);
    }

    [Test]
    public void RedBlueBindAndStrikeSecondExchange()
    {
        manager.firstExchange = false;
        bout.combatantA.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        bout.combatantB.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.BIND_AND_STRIKE, 10, 5, MeleeDamageType.CUTTING, 0);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 0, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        /*for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(6);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedBlue(e1);
        Assert.AreEqual(10, e1.defender.penalty);
    }

    [Test]
    public void RedBlueShieldBeatShieldTarget()
    {
        manager.firstExchange = false;
        bout.combatantA.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        bout.combatantB.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.SHIELD_BEAT, 10, 5, MeleeDamageType.CUTTING, 0);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 0, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        /*for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(6);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedBlue(e1);
        Assert.AreEqual(true, e1.defender.shieldBeaten);
    }

    [Test]
    public void RedBlueShieldBeatWeaponTarget()
    {
        manager.firstExchange = false;
        bout.combatantA.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        bout.combatantB.characterSheet.meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName("Round Shield");
        SelectManuever attackSelectManuever = new SelectManuever(OffensiveManuevers.OffensiveManueverType.SHIELD_BEAT, 10, 5, MeleeDamageType.CUTTING, 0);
        attackSelectManuever.offensiveManuever.SetWeaponBeat(true);
        SelectManuever defendSelectManuever = new SelectManuever(DefensiveManuevers.DefensiveManueverType.PARRY, 0, 0);
        // Use the Assert class to test conditions
        Exchange e1 = new Exchange(bout, bout.combatantA, bout.combatantB, attackSelectManuever, defendSelectManuever);

        /*for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(10);
        for (int i = 0; i < 3; i++)
            DiceRoller.AddNextTestValue(0);*/

        for (int i = 0; i < 10; i++)
            DiceRoller.AddNextTestValue(6);
        DiceRoller.AddNextTestValue(5);
        DiceRoller.AddNextTestValue(0);
        //bout.RedRed(e1, e2);

        bout.RedBlue(e1);
        Assert.AreEqual(true, e1.defender.weaponBeaten);
    }

}
