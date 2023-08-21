using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeleeCombatUI : NetworkBehaviour
{

    private TextMeshProUGUI combatLog;
    private bool setScrollBarFlag = false;


    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameObject.Find("DeclareWindow") != null &&
         GameObject.Find("AttackWindow") != null &&
          GameObject.Find("DefendWindow") != null);

        combatLog = GameObject.Find("CombatLogText").GetComponent<TextMeshProUGUI>();

        AddLog("You see a plate helmet at your feet...");
        AddLog("You should move to it and pick it up (left click)");
        AddLog("You should put the helmet on (tab)");
        AddLog("There are Orcs in the dungeon, beware.");
        AddLog("You will have to kill them if you want to have any chance of escaping.");


    }

    private void Update()
    {
        if (setScrollBarFlag)
        {
            ScrollToBottom();
        }
    }

    public void AddLog(string message)
    {
        combatLog.text += "\n" + message;
        setScrollBarFlag = true;
    }

    [TargetRpc]
    public void RpcAddLog(string message) {

        AddLog(message);
    }

    public static void AddLogServer(string message) { 
        foreach(var log in GameObject.FindObjectsOfType<MeleeCombatUI>())
        {
            log.RpcAddLog(message);
        }
    }

    public void ScrollToBottom()
    {
        if (GameObject.Find("Scrollbar Vertical") == null)
            return;
        GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>().value = 0;
        setScrollBarFlag = false;
        //scrollRect.normalizedPosition = new Vector2(0, 0);
    }

   

}
