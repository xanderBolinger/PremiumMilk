using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static ExcelUtillity.MeleeHitLocation;
using static MeleeCombatController;
using static MeleeCombatManager;
using static OffensiveManuevers;
using static DefensiveManuevers;
using static TargetZone;

public class CharacterCombatNetwork : NetworkBehaviour
{
    public MeleeDamageType meleeDamageType;
    public MeleeStatus meleeDecision;
    public OffensiveManueverType offensiveManueverType;
    public DefensiveManueverType defensiveManueverType;
    public TargetZoneCutting targetZoneCutting = TargetZoneCutting.VerticalDown;
    public TargetZonePuncture targetZonePuncture = TargetZonePuncture.Chest;


    public int dice;
    public int secondaryDice;
    public bool BeatTargetWeapon;

    [HideInInspector]
    public int selectedBoutIndex;

    [HideInInspector]
    public List<string> selectedBoutList = new List<string>();

    public readonly SyncList<string> syncBoutList = new SyncList<string>();

    private CharacterNetwork characterNetwork;

    private void Start()
    {
        characterNetwork = GetComponent<CharacterNetwork>();
    }

    private void Update()
    {
        
    }

    public override void OnStartClient()
    {
        syncBoutList.Callback += SyncBoutList_Callback;
    }

    public override void OnStartServer()
    {
        syncBoutList.Callback += SyncBoutList_Callback;
    }

    private void SyncBoutList_Callback(SyncList<string>.Operation op, int itemIndex, string oldItem, string newItem)
    {
        switch (op)
        {
            case SyncList<string>.Operation.OP_ADD:
                selectedBoutList.Add(newItem);
                break;
            /*case SyncList<Item>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                break;
            case SyncList<Item>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                break;
            case SyncList<Item>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                break;*/
            case SyncList<string>.Operation.OP_CLEAR:
                selectedBoutList.Clear();
                break;
            default:
                throw new System.Exception("Operation not caught for bout sync list update, operation: "+op);
        }
    }

    [ClientRpc]
    public void RpcSendMessage(string message) {
        Debug.Log(message);
    }

    public bool InCombat() {
        return selectedBoutList.Count > 0;
    }

    public void Declare() {
        CmdDeclare(meleeDecision, selectedBoutList[selectedBoutIndex]);
    }

    public void AssignDice() {
        CmdAssignDice(dice, selectedBoutList[selectedBoutIndex]);
    }

    public void SetAttack()
    {
        CmdSetAttack(dice, secondaryDice, offensiveManueverType, defensiveManueverType,
            meleeDamageType, targetZoneCutting, targetZonePuncture, BeatTargetWeapon,
            selectedBoutList[selectedBoutIndex]);
    }

    public void SetDefense()
    {
        CmdSetDefense(dice, secondaryDice, offensiveManueverType, defensiveManueverType,
             meleeDamageType, targetZoneCutting, targetZonePuncture, BeatTargetWeapon,
             selectedBoutList[selectedBoutIndex]);
    }

    public void SetDoNothing()
    {
        CmdSetDoNothing(selectedBoutList[selectedBoutIndex]);
    }

    [Command]
    private void CmdSetDoNothing(string boutTarget) {
        SetSelected(boutTarget);
        meleeCombatController.SetDoNothing();
        meleeCombatController.TryAdvance();
    }

    [Command]
    private void CmdSetDefense(int dice, int secondaryDice, OffensiveManueverType offensiveManueverType, DefensiveManueverType defensiveManueverType,
        MeleeDamageType meleeDamageType,
        TargetZoneCutting targetZoneCutting,
        TargetZonePuncture targetZonePuncture,
        bool beatTarget,
        string boutTarget) {
        SetSelected(boutTarget);

        meleeCombatController.dice = dice;
        meleeCombatController.secondaryDice = secondaryDice;
        meleeCombatController.offensiveManueverType = offensiveManueverType;
        meleeCombatController.defensiveManueverType = defensiveManueverType;
        meleeCombatController.meleeDamageType = meleeDamageType;
        meleeCombatController.targetZoneCutting = targetZoneCutting;
        meleeCombatController.targetZonePuncture = targetZonePuncture;
        meleeCombatController.BeatTargetWeapon = beatTarget;

        meleeCombatController.SetDefense();
        meleeCombatController.TryAdvance();
    }

    [Command]
    private void CmdSetAttack(int dice, int secondaryDice, OffensiveManueverType offensiveManueverType, DefensiveManueverType defensiveManueverType,
        MeleeDamageType meleeDamageType,
        TargetZoneCutting targetZoneCutting, 
        TargetZonePuncture targetZonePuncture, 
        bool beatTarget, 
        string boutTarget) {
        SetSelected(boutTarget);

        meleeCombatController.dice = dice;
        meleeCombatController.secondaryDice = secondaryDice;
        meleeCombatController.offensiveManueverType = offensiveManueverType;
        meleeCombatController.defensiveManueverType = defensiveManueverType;
        meleeCombatController.meleeDamageType = meleeDamageType;
        meleeCombatController.targetZoneCutting = targetZoneCutting;
        meleeCombatController.targetZonePuncture = targetZonePuncture;
        meleeCombatController.BeatTargetWeapon = beatTarget;

        meleeCombatController.SetAttack();
        meleeCombatController.TryAdvance();
    }


    [Command]
    private void CmdAssignDice(int dice, string boutTarget) {
        SetSelected(boutTarget);
        meleeCombatController.dice = dice;
        meleeCombatController.AssignDice();
    }

    [Command]
    private void CmdDeclare(MeleeStatus meleeDecision, string boutTarget) {
        SetSelected(boutTarget);
        meleeCombatController.meleeDecision = meleeDecision;
        meleeCombatController.Declare();
        meleeCombatController.TryAdvance();
    }

    public void SetSelected(string boutTarget)
    {
        var name = characterNetwork.GetCharacterSheet().name;
        var bout = meleeCombatManager.FindBout(name, boutTarget);

        meleeCombatController.selectedBoutIndex = meleeCombatManager.bouts.IndexOf(bout);
        meleeCombatController.selectedCharacterIndex = meleeCombatController.selectedCharacterList.IndexOf(name);
    }




}
