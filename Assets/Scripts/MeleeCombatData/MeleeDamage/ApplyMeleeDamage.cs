using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using System;
using ExcelUtillity;
using JetBrains.Annotations;
using static Character.Species;

public class ApplyMeleeDamage
{
    public ExcelUtillity.MeleeHitLocationData hitLocation;
    public string anatomicalHitLocation;
    public int pd;
    public bool disabled;
    public int damagePoints;
    public int av;
    public bool knockedDown;
    public int level;
    public SpeciesType speciesType;

    public void Hit(int success, CharacterSheet attacker, CharacterSheet defender,
        MeleeHitLocation.MeleeDamageType dmgType, MeleeWeaponStatBlock wep, int zone) {

        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();

        speciesType = defender.species.speciesType;

        damagePoints = CalculateDamagePoints(success, attacker, defender, dmgType, wep);

        watch.Stop();

        Debug.Log($"Calc DP Execution Time: {watch.ElapsedMilliseconds} ms");

        int d6Roll = DiceRoller.Roll(1, 6);

        watch.Start();

        var (unsuedVar, unusedVar2, anatomicalHitLocation) = MeleeHitLocation.GetMeleeHitPD(dmgType, 10, 0,
            MeleeHitLocation.GetHitLocationZoneName(dmgType, zone, d6Roll));

        watch.Stop();

        Debug.Log($"Get Melee Hit PD Execution Time: {watch.ElapsedMilliseconds} ms");

        this.anatomicalHitLocation = anatomicalHitLocation;

        watch.Start();

        av = CalculateArmor(defender, dmgType, wep, anatomicalHitLocation);

        watch.Stop();

        Debug.Log($"Calc AV Execution Time: {watch.ElapsedMilliseconds} ms");

        if (damagePoints - av <= 0)
            return;
        else
            damagePoints -= av;

        watch.Start();

        ApplyMeleeHit(defender, zone, damagePoints, av, dmgType, d6Roll);

        watch.Stop();

        Debug.Log($"Apply Melee Hit Execution Time: {watch.ElapsedMilliseconds} ms");

        ApplyMeleeWeaponFeatures(wep);

        DisabledLimb(anatomicalHitLocation, disabled, defender, anatomicalHitLocation);
        var injury = new Injury(pd, hitLocation.bloodLossPD, hitLocation.painPoints, hitLocation.shockPD, wep.weaponName,
            attacker.name, this.anatomicalHitLocation, Species.GetDamageLevel(defender.species.speciesType, damagePoints));
        Debug.Log("Damage Points: "+damagePoints+", Hit Zone: "+anatomicalHitLocation);
        Debug.Log(attacker.name + ", hit " + defender.name + " " + injury.ToString());
        CombatLog.Log("Damage Points: " + damagePoints + ", Hit Zone: " + anatomicalHitLocation);
        CombatLog.Log(attacker.name + ", hit " + defender.name + " " + injury.ToString());
        defender.medicalData.AddInjury(injury);
        KnockoutCheck(defender);
        KnockdownCheck(defender);
    }

    private void KnockdownCheck(CharacterSheet defender)
    {
        if(!hitLocation.knockDown) return;

        int success = DiceRoller.GetSuccess(defender.meleeCombatStats.GetKnockDown(defender.attributes) + hitLocation.knockDownMod, 7);
        Debug.Log("Knockdown Check("+defender.name+"): "+success+" success.");
        if(success <= 0) {
            Debug.Log("Knocked Down.");
            knockedDown = true;
        }
    }

    private void ApplyMeleeWeaponFeatures(MeleeWeaponStatBlock wep)
    {
        //throw new NotImplementedException();
    }

    private void KnockoutCheck(CharacterSheet defender) {
        var koThreshold = (int)((0.2) * (double)defender.medicalData.knockoutValue);
        if (pd > koThreshold) {
            defender.medicalData.Knockout(hitLocation.shockPD);
        }
    }

    private void DisabledLimb(string zoneName, bool disabled, CharacterSheet defender, string anatomicalHitLocation) {

        if (ArmorPiece.locationData.armsLower.Contains(anatomicalHitLocation)) 
            defender.species.disabledArms++;
        else if (ArmorPiece.locationData.armsUpper.Contains(anatomicalHitLocation))
            defender.species.disabledArms++;

        if (ArmorPiece.locationData.legsLower.Contains(anatomicalHitLocation))
            defender.species.disabledLegs++;
        else if (ArmorPiece.locationData.legsUpper.Contains(anatomicalHitLocation))
            defender.species.disabledLegs++;

        if (zoneName == "Mouth") 
            defender.medicalData.mute = true;

        if (zoneName == "Eye" && defender.medicalData.oneEye)
            defender.medicalData.blind = true;
        else if (zoneName == "Eye")
            defender.medicalData.oneEye = true;

    }

    private int CalculateArmor(CharacterSheet defender, MeleeHitLocation.MeleeDamageType dmgType, 
        MeleeWeaponStatBlock wep, string anatomicalHitLocation) {

        int av = 0;

        switch (dmgType) { 
            case MeleeHitLocation.MeleeDamageType.CUTTING:
                if (wep.cutAP) av -= 3; 
                break;
            case MeleeHitLocation.MeleeDamageType.PIERICNG: 
                if(wep.thrustAP) av -= 3;
                break;
            case MeleeHitLocation.MeleeDamageType.BLUNT:
                if(wep.bluntAP) av -= 3;
                break;
        }

        foreach (var armor in defender.meleeCombatStats.armorPieces) {
            if (!armor.Protected(anatomicalHitLocation))
                continue;
            av += armor.ArmorValue + armor.GetMod(dmgType);
        }

        if(av < 0) av = 0;

        return av;
    }

    private int CalculateDamagePoints(int success, CharacterSheet attacker, CharacterSheet defender,
        MeleeHitLocation.MeleeDamageType dmgType, MeleeWeaponStatBlock wep) {

        int damagePoints = success;
        damagePoints = DmgPointMod(dmgType, damagePoints, wep);
        damagePoints = DmgPointMult(damagePoints, wep);

        return damagePoints + attacker.attributes.str / 3 - defender.attributes.hlt / 3;
    }

    private int DmgPointMult(int damagePoints, MeleeWeaponStatBlock wep) {

        switch (wep.weaponType)
        {
            case MeleeWeaponStatBlock.MeleeWeaponType.LIGHT:
                return damagePoints * 2;
            case MeleeWeaponStatBlock.MeleeWeaponType.MEDIUM:
                return damagePoints * 3;
            case MeleeWeaponStatBlock.MeleeWeaponType.HEAVY:
                return damagePoints * 4;
        }

        throw new Exception("Weapon type multiplier not implemented for wep type: "+wep.weaponType);
    }

    private int DmgPointMod(MeleeHitLocation.MeleeDamageType dmgType, int damagePoints, MeleeWeaponStatBlock wep) {
        switch ((dmgType))
        {
            case MeleeHitLocation.MeleeDamageType.CUTTING: damagePoints += wep.cutMod; break;
            case MeleeHitLocation.MeleeDamageType.PIERICNG: damagePoints += wep.thrustMod; break;
            case MeleeHitLocation.MeleeDamageType.BLUNT: damagePoints += wep.bluntMod; break;
            default:
                break;
        }
        return damagePoints;
    }


    private void ApplyMeleeHit(CharacterSheet characterSheet, int zone, 
        int damagePoints, int armorValue, MeleeHitLocation.MeleeDamageType dmgType, 
        int d6Roll) {


        int effectiveDamagePoints = GetEffectiveDamagePoints(damagePoints);

        level = Species.GetDamageLevel(characterSheet.species.speciesType, effectiveDamagePoints);

        hitLocation = MeleeHitLocation.GetMeleeHitLocation(dmgType, 
            level, zone, d6Roll);

        hitLocation.painPoints -= characterSheet.attributes.wil / 3;
        hitLocation.shockPD -= characterSheet.attributes.wil / 3 / 2 * 20;

        var (pd, disabled, pcHitLocation) = MeleeHitLocation.GetMeleeHitPD(dmgType, effectiveDamagePoints, armorValue, 
            hitLocation.zoneName);
        this.pd = pd;
        this.disabled = disabled;
    }

    public int GetEffectiveDamagePoints(int damagePoints)
    {
        int damageLevel = Species.GetDamageLevel(speciesType, damagePoints);
        int diffDp = Mathf.Abs(Species.GetDamageValue(SpeciesType.MEDIUM_SIZE, damageLevel) - damagePoints);

        switch (speciesType)
        {
            case SpeciesType.MINISCULE:
                return damagePoints + diffDp;
            case SpeciesType.SMALL:
                return damagePoints + diffDp;
            case SpeciesType.MEDIUM_SIZE:
                return damagePoints;
            case SpeciesType.LARGE:
                return damagePoints - diffDp;
            case SpeciesType.VERY_LARGE:
                return damagePoints - diffDp;
            case SpeciesType.GIANT:
                return damagePoints - diffDp;
        }

        throw new Exception("Effective DP not found for damage points: "+damagePoints+" and species type: "+speciesType);
    }
}