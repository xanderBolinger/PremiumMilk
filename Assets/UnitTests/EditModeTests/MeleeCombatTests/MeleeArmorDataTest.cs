using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MeleeArmorDataTest
{

    [Test]
    public void ReadArmor()
    {
        var armor = ArmorLoader.ReadArmor();
        Assert.AreEqual(15, armor.Count);
        Assert.AreEqual(true, armor[0].Protected("Skull"));
    }

    [Test]
    public void ReadShields()
    {
        var shields = MeleeShieldLoader.ReadShields();
        Assert.AreEqual(3, shields.Count);
        Assert.AreEqual(7, shields[0].DTN);
        Assert.AreEqual(6, shields[1].DTN);
        Assert.AreEqual(5, shields[2].DTN);
        Assert.AreEqual(7, MeleeShieldLoader.GetShieldByName("Buckler").DTN);
        Assert.AreEqual(6, MeleeShieldLoader.GetShieldByName("Round Shield").DTN);
        Assert.AreEqual(5, MeleeShieldLoader.GetShieldByName("Medium Shield").DTN);
    }

}
