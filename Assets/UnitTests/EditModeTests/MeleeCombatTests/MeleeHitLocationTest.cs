using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ExcelUtillity;

public class MeleeHitLocationTest
{
    
    [Test]
    public void StabbingTest()
    {
        // Format: BL,S,P,KD(Y/N),KDM
        // 100,350,10,Y,-5
        var location = MeleeHitLocation.
            GetMeleeHitLocation(MeleeHitLocation.MeleeDamageType.PIERICNG, 5, 1, 6);
        Assert.AreEqual(100, location.bloodLossPD);
        Assert.AreEqual(350, location.shockPD);
        Assert.AreEqual(10, location.painPoints);
        Assert.AreEqual(true, location.knockDown);
        Assert.AreEqual(-5, location.knockDownMod);

        var (pd, disabled, pcHitLocation) = MeleeHitLocation.GetMeleeHitPD(MeleeHitLocation.MeleeDamageType.PIERICNG, 36, 0, "Knee");
        Assert.AreEqual(45, pd);
        Assert.AreEqual(true, disabled);

        List<string> bodyParts = new List<string>()
        {
            "Forehead",
            "Eye",
            "Mouth",
            "Neck",
            "Base of Neck",
            "Shoulder Socket",
            "Shoulder Scapula",
            "Lung",
            "Heart",
            "Liver",
            "Stomach",
            "Stomach-Kidney",
            "Liver-Spine",
            "Liver-Kidney",
            "Intestines",
            "Spine",
            "Instetines-Pelvis",
            "Hip Socket",
            "Upper Arm",
            "Forearm",
            "Hand",
            "Thigh",
            "Shin",
            "Foot",
            "Knee"
        };

        foreach (var part in bodyParts) {
            var (pd2, disabled2, pcHitLocation2) = MeleeHitLocation.GetMeleeHitPD(MeleeHitLocation.MeleeDamageType.PIERICNG, 36, 0, part);
            Assert.AreEqual(true, pd2 > 0);
        }

    }

    [Test]
    public void CuttingTest()
    {
        // Format: BL,S,P,KD(Y/N),KDM
        //0,60,3,N,0
        var location = MeleeHitLocation.
            GetMeleeHitLocation(MeleeHitLocation.MeleeDamageType.CUTTING, 1, 7, 1);
        Assert.AreEqual(0, location.bloodLossPD);
        Assert.AreEqual(60, location.shockPD);
        Assert.AreEqual(3, location.painPoints);
        Assert.AreEqual(false, location.knockDown);
        Assert.AreEqual(0, location.knockDownMod);

        List<string> bodyParts = new List<string>()
        {
            "Skull",
            "Face",
            "Throat",
            "Shoulder",
            "Upper Chest",
            "Lower Chest",
            "Stomach",
            "Pelvis",
            "Upper Arm",
            "Forearm",
            "Hand",
            "Thigh",
            "Shin",
            "Knee",
            "Foot"
        };

        foreach (var part in bodyParts)
        {
            var (pd2, disabled2, pcHitLocation3) = MeleeHitLocation.GetMeleeHitPD(MeleeHitLocation.MeleeDamageType.CUTTING, 36, 0, part);
            Assert.AreEqual(true, pd2 > 0);
            Assert.AreEqual(true, pcHitLocation3.Length > 0);
        }

        //var (pd, disabled, pcHitLocation4) = MeleeHitLocation.GetMeleeHitPD(MeleeHitLocation.MeleeDamageType.CUTTING, 16, 13, "Knee");
        //Assert.AreEqual(6, pd);
        //Assert.AreEqual(true, disabled);
        //Assert.AreEqual("Knee", pcHitLocation4);
    }

    [Test]
    public void BluntTest()
    {
        // Format: BL,S,P,KD(Y/N),KDM
        //20,160,7,N,0
        var location = MeleeHitLocation.
            GetMeleeHitLocation(MeleeHitLocation.MeleeDamageType.BLUNT, 3, 3, 3);
        Assert.AreEqual(20, location.bloodLossPD);
        Assert.AreEqual(160, location.shockPD);
        Assert.AreEqual(7, location.painPoints);
        Assert.AreEqual(false, location.knockDown);
        Assert.AreEqual(0, location.knockDownMod);


        List<string> bodyParts = new List<string>()
        {
            "Skull",
            "Face",
            "Throat",
            "Shoulder",
            "Upper Chest",
            "Lower Chest",
            "Stomach",
            "Pelvis",
            "Upper Arm",
            "Forearm",
            "Hand",
            "Thigh",
            "Shin",
            "Knee",
            "Foot"
        };

        foreach (var part in bodyParts)
        {
            var (pd2, disabled2, pcHitLocation3) = MeleeHitLocation.GetMeleeHitPD(MeleeHitLocation.MeleeDamageType.BLUNT, 36, 0, part);
            Assert.AreEqual(true, pd2 > 0);
        }

        DiceRoller.SetNextTestValue(10);
        var (pd, disabled, pcHitLocation4) = MeleeHitLocation.GetMeleeHitPD(MeleeHitLocation.MeleeDamageType.BLUNT, 8, 4, "Forearm");
        Assert.AreEqual(18, pd);
        Assert.AreEqual(false, disabled);
        Assert.AreEqual("Forearm", pcHitLocation4);
    }

}
