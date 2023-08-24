using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using static OffensiveManuevers;
using System;
using static MeleeProficiencies;

public class MeleeCombatStatsTest 
{
    // A Test behaves as an ordinary method
    [Test]
    public void CalcReflexesTest() {
        MeleeCombatStats m = new MeleeCombatStats();

        Attributes a = new Attributes(10, 10, 10, 10, 10);

        m.CalcReflexes(a);

        Assert.AreEqual(3, m.reflexes);

        Attributes b = new Attributes(6, 6, 6, 6, 6);
        m.CalcReflexes(b);

        Assert.AreEqual(2, m.reflexes);
    }

    [Test]
    public void GetKnockdownTest() {
        MeleeCombatStats m = new MeleeCombatStats();

        Attributes a = new Attributes(10, 10, 10, 10, 10);

        Assert.AreEqual(3, m.GetKnockDown(a));
    }

    [Test]
    public void GetOffensiveManeuver() {

        Assert.AreEqual(OffensiveManueverType.CUT, OffensiveManuevers.GetManuever(OffensiveManueverType.CUT).manueverType);

        foreach (OffensiveManueverType item in Enum.GetValues(typeof(OffensiveManueverType))) {
            Assert.AreEqual(item, OffensiveManuevers.GetManuever(item).manueverType);
        }

    }

    [Test]
    public void LearnProficiencyTest() {
        MeleeCombatStats m = new MeleeCombatStats();

        Assert.AreEqual(1, m.GetProficiencies().Count);
        Assert.AreEqual(true, m.GetProficiencies().ContainsKey(MeleeProficiencies.CutNThrust.meleeProfType));

        m.LearnProficiency(MeleeProficiencies.CutNThrust, 1);

        Assert.AreEqual(m.GetProficiencies()[MeleeProficiencies.CutNThrust.meleeProfType], 1);

    }

    [Test]
    public void GetMaxCpTest() {
        Species human = new Species();
        human.speciesType = Species.SpeciesType.MEDIUM_SIZE;
        human.arms = 2;
        human.legs = 2;
        human.onlyLegs = false;
        Attributes attributes = new Attributes(10,10,10,10,10);
        MedicalData medicalData = new MedicalData(attributes, human);
        MeleeCombatStats meleeCombatStats = new MeleeCombatStats();
        meleeCombatStats.LearnProficiency(MeleeProficiencies.CutNThrust, 5);
        meleeCombatStats.SetCurrProf(MeleeProficiencies.CutNThrust);
        meleeCombatStats.CalcReflexes(attributes);
        CharacterSheet characterSheet = new CharacterSheet("test", null, attributes, medicalData, meleeCombatStats);

        Assert.AreEqual(3, characterSheet.meleeCombatStats.reflexes);
        Assert.AreEqual(8, characterSheet.meleeCombatStats.GetMaxCp(0));

    }

    

}
