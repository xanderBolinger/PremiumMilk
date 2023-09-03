using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DefensiveManuevers;
using static ExcelUtillity.MeleeHitLocation;
using static OffensiveManuevers;
using static TargetZone;
using static MeleeCombatController;
using static MeleeCombatManager;
public class AttackWindow : NetworkBehaviour
{

    TMP_Dropdown attackOptions;
    TMP_Dropdown targetOptions;
    Slider slider;
    TextMeshProUGUI header;
    TextMeshProUGUI attackDetails;
    TextMeshProUGUI weaponDetails;

    Combatant attacker;
    Combatant defender;
    Bout bout;
    int reachCost;

    MeleeCombatUI meleeCombatUI;
    CharacterCombatNetwork characterCombatNetwork;

    private void Start()
    {
        header = transform.Find("Header").gameObject.GetComponent<TextMeshProUGUI>();
        attackDetails = transform.Find("AttackDetails").gameObject.GetComponent<TextMeshProUGUI>();
        attackOptions = transform.Find("AttackDropdown").gameObject.GetComponent<TMP_Dropdown>();
        targetOptions = transform.Find("TargetDropdown").gameObject.GetComponent<TMP_Dropdown>();
        slider = transform.Find("Slider").gameObject.GetComponent<Slider>();
        weaponDetails = transform.Find("AttackWeaponDetails").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        meleeCombatUI = FindObjectOfType<MeleeCombatUI>();
        header = transform.Find("Header").gameObject.GetComponent<TextMeshProUGUI>();
        attackDetails = transform.Find("AttackDetails").gameObject.GetComponent<TextMeshProUGUI>();
        attackOptions = transform.Find("AttackDropdown").gameObject.GetComponent<TMP_Dropdown>();
        targetOptions = transform.Find("TargetDropdown").gameObject.GetComponent<TMP_Dropdown>();
        slider = transform.Find("Slider").gameObject.GetComponent<Slider>();
        weaponDetails = transform.Find("AttackWeaponDetails").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Show(Combatant attacker, Combatant defender, Bout bout, bool firstExchange, int reachCost) {
        this.reachCost = reachCost;
        characterCombatNetwork = CharacterController.GetCharacterObject(attacker.characterSheet.name)
            .GetComponent<CharacterCombatNetwork>();
        this.attacker = attacker;
        this.defender = defender;
        this.bout = bout;

        header.text = "Declare Attack On " + defender.characterSheet.name;
        
        attackDetails.text = 
            "Reach Cost: " + reachCost 
            + " dice. Your CP: " + attacker.currentDice+", Target CP: "+defender.currentDice;

        slider.minValue = 0;
        slider.maxValue = attacker.currentDice - reachCost;

        if(!firstExchange)
            slider.value = slider.maxValue;
        else
            slider.value = slider.maxValue / 2;

        SetOptions(attackOptions, new List<string> { "Cut","Thrust","Bash"});
        SetSlashingTargets();
        SetWeaponDetails();

        attackOptions.onValueChanged.AddListener(delegate {
            AttackOptionChanged();
        });
    }

    private void AttackOptionChanged()
    {
        SetWeaponDetails();

        if (attackOptions.value == 1)
            SetStabbingTargets();
        else
            SetSlashingTargets();

    }

    private void SetWeaponDetails() {
        List<OffensiveManueverType> maneuverTypes = new List<OffensiveManueverType>() {
            OffensiveManueverType.CUT, OffensiveManueverType.THRUST, OffensiveManueverType.BASH
        };

        int tn = OffensiveManuevers.GetManuever(maneuverTypes[attackOptions.value]).GetTargetNumber(
            attacker, ExcelUtillity.MeleeHitLocation.MeleeDamageType.CUTTING);

        int mod;

        if (attackOptions.value == 0)
            mod = attacker.meleeWeaponStatBlock.cutMod;
        else if (attackOptions.value == 1)
            mod = attacker.meleeWeaponStatBlock.thrustMod;
        else
            mod = attacker.meleeWeaponStatBlock.bluntMod;

        weaponDetails.text = "Attack Target Number: " + tn + ", Damage Mod: " + mod;
    }

    private void SetSlashingTargets() {
        List<string> zones = new List<string>();

        foreach (var zone in Enum.GetValues(typeof(TargetZoneCutting))) { 
            zones.Add(zone.ToString()); 
        }

        SetOptions(targetOptions, zones);
    }

    private void SetStabbingTargets()
    {
        List<string> zones = new List<string>();

        foreach (var zone in Enum.GetValues(typeof(TargetZonePuncture)))
        {
            zones.Add(zone.ToString());
        }

        SetOptions(targetOptions, zones);
    }

    private void SetOptions(TMP_Dropdown dropdown, List<string> options) {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    
    public void SetAttack() {

        meleeCombatUI.HideAttack();

        List<OffensiveManueverType> maneuverTypes = new List<OffensiveManueverType>() {
            OffensiveManueverType.CUT, OffensiveManueverType.THRUST, OffensiveManueverType.BASH
        };

        List<MeleeDamageType> meleeDamageTypes = new List<MeleeDamageType>() {
            MeleeDamageType.CUTTING, MeleeDamageType.PIERICNG, MeleeDamageType.BLUNT
        };

        SetAttackFields((int)slider.value, maneuverTypes[attackOptions.value], meleeDamageTypes[attackOptions.value],
            (TargetZoneCutting)Enum.GetValues(typeof(TargetZoneCutting)).GetValue(targetOptions.value),
            (TargetZonePuncture)Enum.GetValues(typeof(TargetZonePuncture)).GetValue(targetOptions.value));
        
    }

    private void SetAttackFields(int dice, OffensiveManueverType offensiveManueverType, MeleeDamageType meleeDamageType,
        TargetZoneCutting targetZoneCutting, TargetZonePuncture targetZonePuncture) {
        characterCombatNetwork.selectedBoutIndex = characterCombatNetwork.selectedBoutList
            .IndexOf(defender.characterSheet.name);
        characterCombatNetwork.dice = dice;
        characterCombatNetwork.secondaryDice = 0;
        characterCombatNetwork.offensiveManueverType = offensiveManueverType;
        characterCombatNetwork.defensiveManueverType = DefensiveManueverType.PARRY;
        characterCombatNetwork.meleeDamageType = meleeDamageType;
        characterCombatNetwork.targetZoneCutting = targetZoneCutting;
        characterCombatNetwork.targetZonePuncture = targetZonePuncture;
        characterCombatNetwork.BeatTargetWeapon = false;

        characterCombatNetwork.SetAttack();
    }



}
