using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static MeleeWeaponAbility;

public class MeleeWeaponDataTestCases
{

    [Test]
    public void KernAxe()
    {
        // Arrange
        string weaponName = "Kern Axe";
        //string weaponType = "HEAVY";
        int atnCut = 7;
        int atnThrust = 8;
        int dtn = 7;
        int cutMod = 3;
        int thrustMod = 1;
        int bluntMod = -2;
        int reach = 4;
        bool oneHandedOnly = false;
        bool twoHandedOnly = true;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = true;
        //List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(2, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void Halberd()
    {
        // Arrange
        string weaponName = "Halberd";
        //string weaponType = "HEAVY";
        int atnCut = 7;
        int atnThrust = 8;
        int dtn = 8;
        int cutMod = 3;
        int thrustMod = 1;
        int bluntMod = -2;
        int reach = 5;
        bool oneHandedOnly = false;
        bool twoHandedOnly = true;
        bool bluntAP = false;
        bool thrustAP = true;
        bool cutAP = true;

        var ability = new MeleeWeaponAbility();
        ability.abilityType = MeleeWeaponAbilityType.TwoHandedShock;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>
        {
            ability
        
        };

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities[0].abilityType, meleeWeapon.specialAbilities[0].abilityType);
    }

    [Test]
    public void Falchion()
    {
        // Arrange
        string weaponName = "Falchion";
        //string weaponType = "HEAVY";
        int atnCut = 6;
        int atnThrust = 7;
        int dtn = 7;
        int cutMod = 2;
        int thrustMod = 0;
        int bluntMod = -2;
        int reach = 2;
        bool oneHandedOnly = false;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = true;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void FalchionOneHanded()
    {
        // Arrange
        string weaponName = "Falchion One Handed";
        //string weaponType = "HEAVY";
        int atnCut = 7;
        int atnThrust = 9;
        int dtn = 8;
        int cutMod = 1;
        int thrustMod = -1;
        int bluntMod = -2;
        int reach = 3;
        bool oneHandedOnly = false;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = true;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void Stiletto()
    {
        // Arrange
        string weaponName = "Stiletto";
        //string weaponType = "LIGHT";
        int atnCut = 5;
        int atnThrust = 5;
        int dtn = 9;
        int cutMod = -2;
        int thrustMod = -2;
        int bluntMod = -4;
        int reach = 1;
        bool oneHandedOnly = true;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = true;
        bool cutAP = false;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.LIGHT, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void Spear()
    {
        // Arrange
        string weaponName = "Spear";
        //string weaponType = "LIGHT";
        int atnCut = 9;
        int atnThrust = 6;
        int dtn = 7;
        int cutMod = -2;
        int thrustMod = 2;
        int bluntMod = -2;
        int reach = 4;
        bool oneHandedOnly = false;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = false;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.MEDIUM, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void SpearOneHanded()
    {
        // Arrange
        string weaponName = "Spear One Handed";
        //string weaponType = "LIGHT";
        int atnCut = 9;
        int atnThrust = 6;
        int dtn = 10;
        int cutMod = -2;
        int thrustMod = 2;
        int bluntMod = -2;
        int reach = 4;
        bool oneHandedOnly = false;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = false;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.MEDIUM, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void BattleAxe()
    {
        // Arrange
        string weaponName = "Battle Axe";
        //string weaponType = "LIGHT";
        int atnCut = 8;
        int atnThrust = 10;
        int dtn = 8;
        int cutMod = 3;
        int thrustMod = -2;
        int bluntMod = 0;
        int reach = 2;
        bool oneHandedOnly = false;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = true;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void BattleAxeOneHanded()
    {
        // Arrange
        string weaponName = "Battle Axe One Handed";
        //string weaponType = "LIGHT";
        int atnCut = 7;
        int atnThrust = 10;
        int dtn = 9;
        int cutMod = 2;
        int thrustMod = -2;
        int bluntMod = 0;
        int reach = 3;
        bool oneHandedOnly = false;
        bool twoHandedOnly = false;
        bool bluntAP = false;
        bool thrustAP = false;
        bool cutAP = true;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

    [Test]
    public void LightMace()
    {
        // Arrange
        string weaponName = "Light Mace";
        //string weaponType = "LIGHT";
        int atnCut = 6;
        int atnThrust = 10;
        int dtn = 6;
        int cutMod = -10;
        int thrustMod = -10;
        int bluntMod = 1;
        int reach = 2;
        bool oneHandedOnly = true;
        bool twoHandedOnly = false;
        bool bluntAP = true;
        bool thrustAP = false;
        bool cutAP = false;
        List<MeleeWeaponAbility> specialAbilities = new List<MeleeWeaponAbility>();

        // Act
        var meleeWeapon = MeleeWeaponLoader.GetWeaponByName(weaponName);

        // Assert
        Assert.AreEqual(weaponName, meleeWeapon.weaponName);
        Assert.AreEqual(MeleeWeaponStatBlock.MeleeWeaponType.HEAVY, meleeWeapon.weaponType);
        Assert.AreEqual(atnCut, meleeWeapon.atnCut);
        Assert.AreEqual(atnThrust, meleeWeapon.atnThrust);
        Assert.AreEqual(dtn, meleeWeapon.dtn);
        Assert.AreEqual(cutMod, meleeWeapon.cutMod);
        Assert.AreEqual(thrustMod, meleeWeapon.thrustMod);
        Assert.AreEqual(bluntMod, meleeWeapon.bluntMod);
        Assert.AreEqual(reach, meleeWeapon.reach);
        Assert.AreEqual(oneHandedOnly, meleeWeapon.oneHandedOnly);
        Assert.AreEqual(twoHandedOnly, meleeWeapon.twoHandedOnly);
        Assert.AreEqual(bluntAP, meleeWeapon.bluntAP);
        Assert.AreEqual(thrustAP, meleeWeapon.thrustAP);
        Assert.AreEqual(cutAP, meleeWeapon.cutAP);
        Assert.AreEqual(specialAbilities.Count, meleeWeapon.specialAbilities.Count);
    }

}
