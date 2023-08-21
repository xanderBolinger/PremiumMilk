using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using static OffensiveManuevers;
using static DefensiveManuevers;

[TestFixture]
public class MeleeProficienciesTests
{

    [Test]
    public void CutNThrustProficiency_ShouldHaveCorrectValues()
    {
        Proficiency cutNThrust = MeleeProficiencies.CutNThrust;

        Assert.AreEqual("Cut and Thrust", cutNThrust.name);
        Assert.AreEqual(true, cutNThrust.ContainsManeuver(OffensiveManueverType.CUT));
        Assert.AreEqual(true, cutNThrust.ContainsManeuver(OffensiveManueverType.THRUST));
        Assert.AreEqual(true, cutNThrust.ContainsManeuver(DefensiveManueverType.PARRY));

        Assert.IsTrue(cutNThrust.defaults.ContainsKey(MeleeProficiencies.MeleeProfType.MassWeaponAndShield));
        Assert.AreEqual(-4, cutNThrust.defaults[MeleeProficiencies.MeleeProfType.MassWeaponAndShield]);
    }

    [Test]
    public void MassWeaponAndShieldProficiency_ShouldHaveCorrectValues()
    {
        Proficiency massWeaponAndShield = MeleeProficiencies.MassWeaponAndShield;

        Assert.AreEqual("Mass Weapon and Shield", massWeaponAndShield.name);
        Assert.AreEqual(true, massWeaponAndShield.ContainsManeuver(OffensiveManueverType.CUT));
        Assert.AreEqual(true, massWeaponAndShield.ContainsManeuver(OffensiveManueverType.THRUST));
        Assert.AreEqual(true, massWeaponAndShield.ContainsManeuver(DefensiveManueverType.PARRY));
        Assert.AreEqual(true, massWeaponAndShield.ContainsManeuver(DefensiveManueverType.BLOCK));

        Assert.IsTrue(massWeaponAndShield.defaults.ContainsKey(MeleeProficiencies.MeleeProfType.CutAndThrust));
        Assert.AreEqual(-4, massWeaponAndShield.defaults[MeleeProficiencies.MeleeProfType.CutAndThrust]);
    }

    [Test]
    public void GetProfByType_ShouldReturnCorrectProficiency()
    {
        Proficiency cutNThrust = MeleeProficiencies.GetProfByType(MeleeProficiencies.MeleeProfType.CutAndThrust);
        Proficiency massWeaponAndShield = MeleeProficiencies.GetProfByType(MeleeProficiencies.MeleeProfType.MassWeaponAndShield);

        Assert.AreEqual(MeleeProficiencies.CutNThrust, cutNThrust);
        Assert.AreEqual(MeleeProficiencies.MassWeaponAndShield, massWeaponAndShield);
    }

    [Test]
    public void GetProfByType_ShouldThrowExceptionForInvalidType()
    {
        Assert.Throws<System.Exception>(() => MeleeProficiencies.GetProfByType((MeleeProficiencies.MeleeProfType)99));
    }
}
