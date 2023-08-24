using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Character;
using ExcelUtillity;
using UnityEditor;

public class ApplyDamageTest
{

    CharacterSheet characterSheet;

    [SetUp]
    public void StartUp() {
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
        characterSheet = new CharacterSheet("test", human, attributes, medicalData, meleeCombatStats);

        var armor = ArmorLoader.ReadArmor();
        characterSheet.meleeCombatStats.armorPieces.Add(armor[0]);
    }

    [Test]
    public void KOTest()
    {
        Assert.AreEqual(true, characterSheet.medicalData.conscious);
        DiceRoller.SetNextTestValue(97);
        Assert.AreEqual(100, characterSheet.medicalData.knockoutValue);
        
        characterSheet.medicalData.AddInjury(new Injury(501, 0,0,0, "","","",0));
        characterSheet.medicalData.Knockout(0);

        Assert.AreEqual(false, characterSheet.medicalData.conscious);

    }

    [Test]
    public void MeleeDamageTest() {
        DiceRoller.AddNextTestValue(1);
        DiceRoller.AddNextTestValue(50);
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName("Kern Axe");

        // TODO: balance lower damage point hits with armor and lower level hits, 
        // TODO: add anatomical location to injury 
        // success 5
        // mod 3
        // mult 4
        // str 3
        // toughness 3
        // armor value 12, armor mod -3 and plus 3 so 0
        // dmg points: 32-12
        // dmg type cutting 
        // zone 5 
        // zone roll 1
        // sub zone roll N/A
        // ko roll 50
        // pd 9000, anatomical zone Skull, 
        // Format: BL,S,P,KD(Y/N),KDM
        // 2000,1000,100,Y,-100
        ApplyMeleeDamage apm = new ApplyMeleeDamage();
        apm.Hit(5, characterSheet, characterSheet, MeleeHitLocation.MeleeDamageType.CUTTING, meleeWeapon, 5);

        Assert.AreEqual(15, apm.av);
        Assert.AreEqual(20000, apm.pd);
        Assert.AreEqual("Skull", apm.anatomicalHitLocation);
        //Assert.AreEqual(2000, apm.hitLocation.bloodLossPD);
        Assert.AreEqual(980, apm.hitLocation.shockPD);
        Assert.AreEqual(97, apm.hitLocation.painPoints);
        //Assert.AreEqual(true, apm.hitLocation.knockDown);
        //Assert.AreEqual(-100, apm.hitLocation.knockDownMod);
    }

    [Test]
    public void EffectiveDamagePointTest() {
        ApplyMeleeDamage apm = new ApplyMeleeDamage();
        apm.speciesType = Species.SpeciesType.GIANT;
        Assert.AreEqual(10, apm.GetEffectiveDamagePoints(21));
        apm.speciesType = Species.SpeciesType.MINISCULE;
        Assert.AreEqual(7, apm.GetEffectiveDamagePoints(3));
    }

}
