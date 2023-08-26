using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DefensiveManuevers;
using static OffensiveManuevers;

public class DefendWindow : MonoBehaviour
{
    TMP_Dropdown defenseDropdown;
    TextMeshProUGUI defenseDetails;
    TextMeshProUGUI body;
    Slider slider;

    Combatant defender;
    Combatant attacker;
    Bout bout;

    CharacterCombatNetwork characterCombatNetwork;
    MeleeCombatUI meleeCombatUI;

    private void Start()
    {
        defenseDropdown = transform.Find("DefenseDropdown").gameObject.GetComponent<TMP_Dropdown>();
        slider = transform.Find("Slider").gameObject.GetComponent<Slider>();
        body = transform.Find("Body").gameObject.GetComponent<TextMeshProUGUI>();
        defenseDetails = transform.Find("DefenseDetails").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Show(Combatant defender, Combatant attacker, Bout bout, bool firstExchange, 
        string attackingManueverName,
        int attackingDice) {
        this.defender = defender;
        this.attacker = attacker;
        this.bout = bout;
        meleeCombatUI = CharacterController.GetCharacterObject(defender.characterSheet.name)
            .GetComponent<MeleeCombatUI>();
        characterCombatNetwork = CharacterController.GetCharacterObject(defender.characterSheet.name)
            .GetComponent<CharacterCombatNetwork>();

        body.text = "Incoming Attack from: " + attacker.characterSheet.name + ", Maneuver: " 
            + attackingManueverName
            + ", Dice: " + attackingDice;

        var manuevers = new List<string> {
            "PARRY", "DODGE", "DUCK & WEAVE", "FULL EVASION"
        };

        if (defender.characterSheet.meleeCombatStats.shield != null)
            manuevers.Insert(0, "BLOCK");

        SetOptions(defenseDropdown, manuevers);
        SetDefenseDetails();

        slider.minValue = 0;
        slider.maxValue = defender.currentDice;
        if(firstExchange)
            slider.value = slider.maxValue/2;
        else 
            slider.value = slider.maxValue;

        defenseDropdown.onValueChanged.AddListener(delegate {
            DefenseOptionChanged();
        });
    }

    private void DefenseOptionChanged()
    {
        SetDefenseDetails();
    }

    private void SetDefenseDetails() {

        var manuever = GetManuever();
        var tn = manuever.GetTargetNumber(defender);

        defenseDetails.text = "Defense: " + manuever.GetManeuverName() + ", Target Number: " + tn;

        if (manuever.manueverType == DefensiveManueverType.FULLEVASION) {
            slider.maxValue = defender.currentDice - 1;
            slider.value = slider.maxValue;
        }

    }

    private IDefensiveManuever GetManuever() {
        bool shield = defender.characterSheet.meleeCombatStats.shield != null;

        var manueverTypes = new List<DefensiveManueverType>() { DefensiveManueverType.PARRY, DefensiveManueverType.DODGE,
        DefensiveManueverType.DUCKANDWEAVE, DefensiveManueverType.FULLEVASION};

        if (shield)
            manueverTypes.Insert(0, DefensiveManueverType.BLOCK);

        var manuever = DefensiveManuevers.GetManuever(manueverTypes[defenseDropdown.value]);

        return manuever;
    }

    private void SetOptions(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    public void Defend() {
        characterCombatNetwork.selectedBoutIndex = characterCombatNetwork.selectedBoutList
            .IndexOf(attacker.characterSheet.name);
        characterCombatNetwork.dice = (int)slider.value;
        characterCombatNetwork.secondaryDice = 0;
        characterCombatNetwork.defensiveManueverType = GetManuever().manueverType;
        meleeCombatUI.HideDefense();
        characterCombatNetwork.SetDefense();
    }

}
