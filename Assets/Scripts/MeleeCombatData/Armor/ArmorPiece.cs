using ExcelUtillity;
using System;
using System.Collections.Generic;

public class ArmorPiece
{
    public static HitZoneData locationData = new HitZoneData();

    public enum ArmorPieceSlots
    {
        UnderArmUpper,
        OverArmUpper,
        UnderArmLower,
        OverArmLower,
        UnderTorsoUpper,
        OverTorsoUpper,
        UnderTorsoLower,
        OverTorsoLower,
        OverLegLower,
        UnderLegLower,
        OverLegUpper,
        UnderLegUpper,
        OverHeadUpper,
        OverHeadLower,
        UnderHeadUpper,
        UnderHeaderLower
    }

    public enum ArmorMaterial
    {
        PaddedCloth,
        Leather,
        CuirBouilli,
        ScaleArmor,
        Mail,
        DoubledMail,
        BandedMail,
        LightPlate,
        Plate,
        HeavyPlate
    }

    public string Name { get; set; }
    public int ArmorValue { get; set; }
    public List<string> ProtectedBodyParts { get; set; }
    public int Weight { get; set; }
    public ArmorMaterial Material { get; set; }
    public List<ArmorPieceSlots> Slots { get; set; }

    int bluntMod;
    int stabMod;
    int cutMod;

    public ArmorPiece() { }

    public ArmorPiece(string name, int armorValue, int weight, ArmorMaterial material, List<ArmorPieceSlots> slots)
    {
        Name = name;
        ArmorValue = armorValue;
        Weight = weight;
        Material = material;
        Slots = slots;

        ProtectedBodyParts = new List<string>();

        InstantiateParts();

        SetAttackModifiers();
    }

    public int GetMod(MeleeHitLocation.MeleeDamageType dmgType) {
        switch (dmgType)
        {
            case MeleeHitLocation.MeleeDamageType.CUTTING:
                return cutMod;
            case MeleeHitLocation.MeleeDamageType.PIERICNG:
                return stabMod;
            case MeleeHitLocation.MeleeDamageType.BLUNT:
                return bluntMod;
            default:
                throw new Exception("Dmg type not found for dmgType: " + dmgType);
        }
    }

    private void InstantiateParts()
    {
        bool upperHead = false;
        bool lowerHead = false;
        bool upperLegs = false;
        bool lowerLegs = false;
        bool upperArms = false;
        bool lowerArms = false;
        bool upperTorso = false;
        bool lowerTorso = false;

        foreach (var slot in Slots)
        {
            switch (slot)
            {
                case ArmorPieceSlots.UnderArmUpper:
                    upperArms = true;
                    break;
                case ArmorPieceSlots.OverArmUpper:
                    upperArms = true;
                    break;
                case ArmorPieceSlots.UnderArmLower:
                    lowerArms = true;
                    break;
                case ArmorPieceSlots.OverArmLower:
                    lowerArms = true;
                    break;
                case ArmorPieceSlots.UnderTorsoUpper:
                    upperTorso = true;
                    break;
                case ArmorPieceSlots.OverTorsoUpper:
                    upperTorso = true;
                    break;
                case ArmorPieceSlots.UnderTorsoLower:
                    lowerTorso = true;
                    break;
                case ArmorPieceSlots.OverTorsoLower:
                    lowerTorso = true;
                    break;
                case ArmorPieceSlots.UnderLegLower:
                    lowerLegs = true;
                    break;
                case ArmorPieceSlots.OverLegUpper:
                    upperLegs = true;
                    break;
                case ArmorPieceSlots.OverHeadUpper:
                    upperHead = true;
                    break;
                case ArmorPieceSlots.OverHeadLower:
                    lowerHead = true;
                    break;
                case ArmorPieceSlots.UnderHeadUpper:
                    upperHead = true;
                    break;
                case ArmorPieceSlots.UnderHeaderLower:
                    lowerHead = true;
                    break;
                case ArmorPieceSlots.OverLegLower:
                    lowerLegs = true;
                    break;
                case ArmorPieceSlots.UnderLegUpper:
                    upperLegs = true;
                    break;
            }

        }

        if (upperHead == true) {
            AddList(locationData.headUpper);
        }
        if (lowerHead == true)
        {
            AddList(locationData.headLower);
        }
        if (upperLegs == true)
        {
            AddList(locationData.legsUpper);
        }
        if (lowerLegs == true)
        {
            AddList(locationData.legsLower);
        }
        if (upperArms == true)
        {
            AddList(locationData.armsUpper);
        }
        if (lowerArms == true)
        {
            AddList(locationData.armsLower);
        }
        if (upperTorso == true)
        {
            AddList(locationData.torsoUpper);
        }
        if (lowerTorso == true)
        {
            AddList(locationData.torsoLower);
        }

    }

    private void AddList(List<string> protectedParts) { 
    
        foreach(var item in protectedParts)
            ProtectedBodyParts.Add(item);

    }

    public bool Protected(string locationName) {

        foreach (var item in ProtectedBodyParts) { 
            if(item == locationName)
                return true;
        }

        return false;
    }

    public static int StabbingPartToZone(string part) {
        if (locationData.legsLower.Contains(part))
            return 1;
        if (locationData.legsUpper.Contains(part))
            return 2;
        if (locationData.torsoLower.Contains(part))
            return DiceRoller.Roll(1,2) == 1 ? 3 : 4;
        if (locationData.torsoUpper.Contains(part))
            return 5;
        if (locationData.headUpper.Contains(part) || locationData.headLower.Contains(part))
            return 6;
        if (locationData.armsLower.Contains(part) || locationData.armsUpper.Contains(part))
            return 7;

        throw new Exception("Stabbing Part not contained in location data: "+part);
    }

    public static int SlashingPartToZone(string part) {

        if (locationData.legsLower.Contains(part))
            return 1;
        if (locationData.legsUpper.Contains(part))
            return 2;
        if (locationData.torsoLower.Contains(part) || locationData.torsoUpper.Contains(part))
            return DiceRoller.Roll(1, 2) == 1 ? 3 : 6;
        if (locationData.headUpper.Contains(part) || locationData.headLower.Contains(part))
            return DiceRoller.Roll(1, 2) == 1 ? 4 : 5;
        if (locationData.armsLower.Contains(part) || locationData.armsUpper.Contains(part))
            return 7;
        throw new Exception("Slashing Part not contained in location data: " + part);
    }


    public void SetAttackModifiers()
    {
        

        switch (Material)
        {
            case ArmorMaterial.PaddedCloth:
                bluntMod = 0;
                stabMod = 0;
                cutMod = 0;
                break;
            case ArmorMaterial.Leather:
                bluntMod = 0;
                stabMod = 0;
                cutMod = 0;
                break;
            case ArmorMaterial.CuirBouilli:
                bluntMod = 0;
                stabMod = 0;
                cutMod = 0;
                break;
            case ArmorMaterial.ScaleArmor:
                bluntMod = 0;
                stabMod = 5;
                cutMod = 8;
                break;
            case ArmorMaterial.Mail:
                bluntMod = 0;
                stabMod = 3;
                cutMod = 6;
                break;
            case ArmorMaterial.DoubledMail:
                bluntMod = 0;
                stabMod = 3;
                cutMod = 8;
                break;
            case ArmorMaterial.BandedMail:
                bluntMod = 0;
                stabMod = 10;
                cutMod = 15;
                break;
            case ArmorMaterial.LightPlate:
                bluntMod = 0;
                stabMod = 10;
                cutMod = 15;
                break;
            case ArmorMaterial.Plate:
                bluntMod = 0;
                stabMod = 10;
                cutMod = 16;
                break;
            case ArmorMaterial.HeavyPlate:
                bluntMod = 0;
                stabMod = 12;
                cutMod = 18;
                break;
        }


    }
}
