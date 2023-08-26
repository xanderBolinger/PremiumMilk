using Mirror;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeclareWindow : MonoBehaviour
{

    private string targetName;
    private MeleeCombatUI meleeCombatUI;
    private CharacterCombatNetwork characterCombatNetwork;
    private TextMeshProUGUI text;

    private void Start()

    {
        text = transform.Find("Body").gameObject.GetComponent<TextMeshProUGUI>();

    }

    public void SetCharacter(CharacterCombatNetwork characterCombatNetwork, MeleeCombatUI meleeCombatUI) {
        this.characterCombatNetwork = characterCombatNetwork;
        this.meleeCombatUI = meleeCombatUI;
    }

    public void SetTarget(string targetName) {
        this.targetName = targetName;
        text.text = "Declare attack or defense against target: " + targetName;
    }

    public void Attack() {
        characterCombatNetwork.selectedBoutIndex = characterCombatNetwork.selectedBoutList.IndexOf(targetName);
        characterCombatNetwork.meleeDecision = MeleeCombatManager.MeleeStatus.RED;
        meleeCombatUI.RpcHideDeclare();
        characterCombatNetwork.Declare();
    }


    public void Defend() {
        characterCombatNetwork.selectedBoutIndex = characterCombatNetwork.selectedBoutList.IndexOf(targetName);
        characterCombatNetwork.meleeDecision = MeleeCombatManager.MeleeStatus.BLUE;
        meleeCombatUI.RpcHideDeclare();
        characterCombatNetwork.Declare();
    } 



}
