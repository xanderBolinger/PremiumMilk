using System.Collections;
using System.Collections.Generic;
using Character;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MedicalDataTest
{

    [Test]
    public void MedicalTest()
    {
        Injury i = new Injury(1, 11, 15, 90, "metal-sword", "James Smith", "location",0);
        Injury i2 = new Injury(1, 11, 15, 90, "metal-sword", "James Smith the 2nd", "location", 0);
        Attributes attributes = new Attributes(10,10,10,10,10);
        MedicalData data = new MedicalData(attributes, SpeciesLoader.GetSpeciesByName("Human"));

        Assert.AreEqual(false, data.IsInjured());
        data.AddInjury(i);
        data.AddInjury(i2);
        Assert.AreEqual(!false, data.IsInjured());
        Assert.AreEqual(2, data.GetPD());
        Assert.AreEqual(22, data.GetBloodlossPD());
        Assert.AreEqual(27, data.GetPain());
    }

    [Test]
    public void SpeciesDamageLevel() {
        Assert.AreEqual(1, Species.GetDamageLevel(Species.SpeciesType.MINISCULE, 1));
        Assert.AreEqual(2, Species.GetDamageLevel(Species.SpeciesType.SMALL, 3));
        Assert.AreEqual(3, Species.GetDamageLevel(Species.SpeciesType.MEDIUM_SIZE, 9));
        Assert.AreEqual(4, Species.GetDamageLevel(Species.SpeciesType.LARGE, 15));
        Assert.AreEqual(5, Species.GetDamageLevel(Species.SpeciesType.VERY_LARGE, 21));
        Assert.AreEqual(5, Species.GetDamageLevel(Species.SpeciesType.GIANT, 1000));
    }

}
