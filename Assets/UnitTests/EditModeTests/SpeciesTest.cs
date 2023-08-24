using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Character;
using UnityEditor;

public class SpeciesTest
{

    [Test]
    public void SpeciesTestSimplePasses()
    {

        Species human = new Species();
        human.arms = 2;
        human.legs = 2;
        human.onlyLegs = false; 

        Assert.AreEqual(2, human.arms);
        Assert.AreEqual(2, human.legs);
        Assert.AreEqual(true, human.canRun());
        Assert.AreEqual(true, human.canWalk());
        Assert.AreEqual(true, human.canUseTwoHanded());

        human.disabledLegs++;

        Assert.AreEqual(false, human.canRun());
        Assert.AreEqual(true, human.canWalk());
        Assert.AreEqual(true, human.canUseTwoHanded());

        human.disabledLegs++;

        Assert.AreEqual(false, human.canWalk());
        Assert.AreEqual(false, human.canRun());
        Assert.AreEqual(true, human.canUseTwoHanded());
        Assert.AreEqual(false, human.canFight());
        human.disabledArms++;

        Assert.AreEqual(false, human.canUseTwoHanded());


        Species giantSider = new Species();
        giantSider.arms = 0;
        giantSider.legs = 6;
        giantSider.onlyLegs = true;

        Assert.AreEqual(true, giantSider.canRun());
        Assert.AreEqual(true, giantSider.canWalk());
        Assert.AreEqual(true, giantSider.canUseTwoHanded());

        giantSider.disabledLegs++;
        giantSider.disabledLegs++;

        Assert.AreEqual(true, giantSider.canRun());
        Assert.AreEqual(true, giantSider.canWalk());
        Assert.AreEqual(true, giantSider.canUseTwoHanded());

        giantSider.disabledLegs++;

        Assert.AreEqual(false, giantSider.canRun());
        Assert.AreEqual(true, giantSider.canWalk());
        Assert.AreEqual(true, giantSider.canUseTwoHanded());

        giantSider.disabledLegs++;

        Assert.AreEqual(false, giantSider.canRun());
        Assert.AreEqual(false, giantSider.canWalk());
        Assert.AreEqual(true, giantSider.canUseTwoHanded());
    }

    [Test]
    public void SpeciesData_LoadsCorrectly()
    {
        var speciesData = SpeciesLoader.LoadSpeciesData();

        Assert.AreEqual(true, speciesData.Count >= 3);

        // Assert the properties of the loaded species data
        Assert.AreEqual("Human", speciesData[0].speciesName);
        Assert.AreEqual(Species.SpeciesType.MEDIUM_SIZE, speciesData[0].speciesType);
        Assert.AreEqual(2, speciesData[0].arms);
        Assert.AreEqual(2, speciesData[0].legs);
        Assert.IsFalse(speciesData[0].onlyLegs);

        Assert.AreEqual("Goblin", speciesData[1].speciesName);
        Assert.AreEqual(Species.SpeciesType.SMALL, speciesData[1].speciesType);
        Assert.AreEqual(2, speciesData[1].arms);
        Assert.AreEqual(2, speciesData[1].legs);
        Assert.IsFalse(speciesData[1].onlyLegs);

        Assert.AreEqual("Giant Spider", speciesData[2].speciesName);
        Assert.AreEqual(Species.SpeciesType.LARGE, speciesData[2].speciesType);
        Assert.AreEqual(0, speciesData[2].arms);
        Assert.AreEqual(6, speciesData[2].legs);
        Assert.IsTrue(speciesData[2].onlyLegs);
    }

    [Test]
    public void GetSpeciesByName_SpeciesExists_ReturnsCorrectSpecies()
    {
        string speciesName = "Goblin";

        Species species = SpeciesLoader.GetSpeciesByName(speciesName);

        Assert.IsNotNull(species);
        Assert.AreEqual(speciesName, species.speciesName);
        Assert.AreEqual(Species.SpeciesType.SMALL, species.speciesType);
        Assert.AreEqual(2, species.arms);
        Assert.AreEqual(2, species.legs);
        Assert.IsFalse(species.onlyLegs);
    }

    [Test]
    public void GetSpeciesByName_SpeciesDoesNotExist_ThrowsException()
    {
        string speciesName = "Invalid Species Test Name";

        Assert.Throws<System.Exception>(() =>
        {
            SpeciesLoader.GetSpeciesByName(speciesName);
        });
    }

    [Test]
    public void SpeciesDamageValueTest() {

        Assert.AreEqual(13, Species.GetDamageValue(Species.SpeciesType.MEDIUM_SIZE, 5));
        Assert.AreEqual(7, Species.GetDamageValue(Species.SpeciesType.MEDIUM_SIZE, 3));
    }

}
