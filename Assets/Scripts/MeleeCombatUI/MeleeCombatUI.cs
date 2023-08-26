using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeleeCombatUI : NetworkBehaviour
{

    [SerializeField] private GameObject declareWindowObject;
    [SerializeField] private GameObject attackWindowObject;
    [SerializeField] private GameObject defendWindowObject;

    [SerializeField] private DeclareWindow declareWindow;
    [SerializeField] private AttackWindow attackWindow;
    [SerializeField] private DefendWindow defendWindow;

    [SerializeField] private TextMeshProUGUI combatLog;

    private bool setScrollBarFlag = false;


    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameObject.Find("DeclareWindow") != null &&
         GameObject.Find("AttackWindow") != null &&
          GameObject.Find("DefendWindow") != null);

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

    [ClientRpc]
    public void RpcAddLog(string message)
    {

        AddLog(message);
    }

    public static void AddLogServer(string message)
    {
        foreach (var log in GameObject.FindObjectsOfType<MeleeCombatUI>())
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

    [TargetRpc]
    public void RcpShowDeclare(string target)
    {

        declareWindow.SetTarget(target);
        declareWindowObject.SetActive(true);
    }

    [TargetRpc]
    public void RpcHideDeclare()
    {
        declareWindowObject.SetActive(false);
    }

    [TargetRpc]
    public void RpcShowAttack(Combatant attacker, Combatant defender, Bout bout, bool firstExchange, int reachCost)
    {
        attackWindowObject.SetActive(true);

        attackWindow.Show(attacker, defender, bout, firstExchange, reachCost);
    }

    public void HideAttack()
    {
        attackWindowObject.SetActive(false);
    }

    [TargetRpc]
    public void RpcShowDefense(Combatant defender, Combatant attacker, Bout bout,
        bool firstExchange, string name, int dice)
    {
        defendWindowObject.SetActive(true);
        defendWindow.Show(defender, attacker, bout, firstExchange,
            name,
            dice);
    }

    public void HideDefense()
    {
        defendWindowObject.SetActive(false);
    }

}
