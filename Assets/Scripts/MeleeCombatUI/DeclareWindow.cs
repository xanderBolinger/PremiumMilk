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
    private TextMeshProUGUI text;

    private void Start()

    {
        text = transform.Find("Body").gameObject.GetComponent<TextMeshProUGUI>();

    }

    public void SetCharacter(MeleeCombatUI meleeCombatUI) {
        this.meleeCombatUI = meleeCombatUI;
    }

    public void SetTarget(string targetName) {
        this.targetName = targetName;
        text.text = "Declare attack or defense against target: " + targetName;
    }

    public void Attack() {
        meleeCombatUI.RpcHideDeclare();
    }


    public void Defend() {
        meleeCombatUI.RpcHideDeclare();
    } 



}
