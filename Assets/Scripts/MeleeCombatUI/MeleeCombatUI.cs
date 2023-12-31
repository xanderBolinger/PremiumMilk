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


    private void Start()
    {
        AddLog("Press control to move camera.");
        AddLog("Click to move.");
        AddLog("Click toolbar to use spells, for Magic Missile you need to click on a target.");
        AddLog("In combat pick manuevers and assign dice from a dice pool. " +
            "Perform the Full Evasion defense successfully to leave combat.");
        AddLog("To recover Fatigue points from casting, click on your square to pass your turn and rest.");
        AddLog("Reach the end of the maze to survive. Look for a bright light.");
        /* AddLog("You see a plate helmet at your feet...");
         AddLog("You should move to it and pick it up (left click)");
         AddLog("You should put the helmet on (tab)");
         AddLog("There are Orcs in the dungeon, beware.");
         AddLog("You will have to kill them if you want to have any chance of escaping.");*/
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
    public void RcpShowDeclare(NetworkConnectionToClient conn, string character, string target)
    {
        declareWindow.SetCharacter(character);
        declareWindow.SetTarget(target);
        declareWindowObject.SetActive(true);
    }

    
    public void HideDeclare()
    {
        declareWindowObject.SetActive(false);
    }

    [TargetRpc]
    public void RpcShowAttack(NetworkConnectionToClient conn, Combatant attacker, Combatant defender, Bout bout, bool firstExchange, int reachCost)
    {
        attackWindowObject.SetActive(true);

        attackWindow.Show(attacker, defender, bout, firstExchange, reachCost);
    }

    public void HideAttack()
    {
        attackWindowObject.SetActive(false);
    }

    [TargetRpc]
    public void RpcShowDefense(NetworkConnectionToClient conn, Combatant defender, Combatant attacker, Bout bout,
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
