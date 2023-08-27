using Mirror;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeclareWindow : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    private string targetName;
    private MeleeCombatUI meleeCombatUI;
    private CharacterCombatNetwork characterCombatNetwork;




    public void SetCharacter(string name) {
        this.characterCombatNetwork = CharacterController.GetCharacterObject(name)
            .GetComponent<CharacterCombatNetwork>();
        this.meleeCombatUI = FindObjectOfType<MeleeCombatUI>();
    }

    public void SetTarget(string targetName) {
        this.targetName = targetName;
        text.text = "Declare attack or defense against target: " + targetName;
    }

    public void Attack() {
        characterCombatNetwork.selectedBoutIndex = characterCombatNetwork.selectedBoutList.IndexOf(targetName);
        characterCombatNetwork.meleeDecision = MeleeCombatManager.MeleeStatus.RED;
        meleeCombatUI.HideDeclare();
        characterCombatNetwork.Declare();
    }


    public void Defend() {
        characterCombatNetwork.selectedBoutIndex = characterCombatNetwork.selectedBoutList.IndexOf(targetName);
        characterCombatNetwork.meleeDecision = MeleeCombatManager.MeleeStatus.BLUE;
        meleeCombatUI.HideDeclare();
        characterCombatNetwork.Declare();
    } 



}
