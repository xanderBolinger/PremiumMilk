using UnityEngine;
using Mirror;
using Character;
using static MeleeProficiencies;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterSoundEffects))]
public class CharacterNetwork : NetworkBehaviour
{
    public string characterName;

    private static int characterSheetIndex;

    [SyncVar(hook = nameof(CharacterSheetHook))]
    CharacterSheet characterSheet;

    CharacterSoundEffects characterSoundEffects;
    CharacterController characterController;

    public CharacterSheet GetCharacterSheet() { return characterSheet; }

    private void Start()
    {
        SetFields();

        if (isServer) { 
            ClearFields();
            CreateCharacter();
        }

        Debug.Log("Loaded Character: "+characterSheet.name);

        name = characterSheet.name;

        characterSheetIndex++;
       
    }

    private void CreateCharacter() {
        CharacterSheet character = CharacterSheetLoader.GetCharacterSheetByName(characterName);
        CopyCharacter(character);
        this.characterSheet = new CharacterSheet(character);
    }

    private void SetFields() {
        characterSoundEffects = GetComponent<CharacterSoundEffects>();
        characterController = GetComponent<CharacterController>();
    }

    public override void OnStartLocalPlayer()
    {
        //GameObject.Find("Main Camera").GetComponent<CameraController>().target = transform;
    }

    private void CharacterSheetHook(CharacterSheet oldCharacterSheet, CharacterSheet newCharacterSheet)
    {
        this.characterSheet = newCharacterSheet;
        characterSheet.medicalData.GetInjures().Clear();
        foreach (var injury in injuries)
            characterSheet.medicalData.GetInjures().Add(injury);

        characterSheet.meleeCombatStats.armorPieces.Clear();
        foreach (var armorPiece in armor)
            characterSheet.meleeCombatStats.armorPieces.Add(ArmorLoader.GetArmorPieceByName(armorPiece));

        characterSheet.meleeCombatStats.GetProficiencies().Clear();
        foreach (var prof in profs)
            characterSheet.meleeCombatStats.GetProficiencies().Add(prof.Key, prof.Value);

        if(characterSheet.meleeCombatStats.GetProficiencies().Count > 0)
            characterSheet.meleeCombatStats.SetCurrProf(characterSheet.meleeCombatStats.currProf);
    }

    private readonly SyncList<Injury> injuries = new SyncList<Injury>();

    private readonly SyncList<string> armor = new SyncList<string>();

    private readonly SyncDictionary<MeleeProfType, int> profs = new SyncDictionary<MeleeProfType, int>();

    [TargetRpc]
    public void RpcCallDead() {
        CallDead();
    }

    [ClientRpc]
    public void RpcCallDeadNpc() {
        CallDead();
    }

    private void CallDead() {
        Debug.Log(characterSheet.name + " has died.");
        //var npcController = GetComponent<NpcController>();
        tag = "Dead";
        //if (npcController != null)
        //    npcController.DeathDrop();
        //Destroy(gameObject);
        //characterSoundEffects.Death();

        //characterAnimator.AnmateDeath();

        if (isLocalPlayer)
            CombatLog.Log("<b style=\"color: red;\">You have died, click to restart the level.</b>"); ;
    }

    public void Print()
    {
        CharacterController.PrintCharacter(characterSheet);
    }

    public void SetCharacterSheet(CharacterSheet characterSheet)
    {
        ClearFields();

        CopyCharacter(characterSheet);

        //Debug.Log("Set Character: "+characterSheet.name);
    }

    private void ClearFields() {
        injuries.Clear();
        armor.Clear();
        profs.Clear();
    }

    private void CopyCharacter(CharacterSheet characterSheet) {


        foreach (var armorPiece in characterSheet.meleeCombatStats.armorPieces)
        {
            armor.Add(armorPiece.Name);
        }

        foreach (var injury in characterSheet.medicalData.GetInjures())
        {
            injuries.Add(injury);
        }

        foreach (var prof in characterSheet.meleeCombatStats.GetProficiencies())
        {
            profs.Add(prof);
        }

        this.characterSheet = new CharacterSheet(characterSheet);
    }


    [Command]
    private void CmdUpdateCharacterSheet(CharacterSheet characterSheet) {
        this.characterSheet = new CharacterSheet(characterSheet);
        CharacterSheet updateCs = null;
        foreach (var cs in CombatManager.combatManager.characterSheets) { 
            if (cs.name == characterSheet.name) {
                updateCs = cs;
                break;
            }
        }

        if(updateCs == null) { throw new Exception("Character not found for name: "+characterSheet.name); }

        var index = CombatManager.combatManager.characterSheets.IndexOf(updateCs);
        CombatManager.combatManager.characterSheets[index] = this.characterSheet;
    }

    [Command]
    private void CmdClearArmor() {
        armor.Clear();
    }

    [Command]
    private void CmdAddArmor(string armorName) {
        armor.Add(armorName);
    }

    [Command]
    private void CmdClearProf() { 
        profs.Clear();
    }

    [Command]
    private void CmdAddProf(MeleeProfType profType, int value) {
        profs.Add(profType, value);
    }

    [Command]
    private void CmdClearInjuries() { 
        injuries.Clear();
    }

    [Command]
    private void CmdAddInjury(Injury injury) {
        injuries.Add(injury);
    }


    public void UpdateCharacterSheet(CharacterSheet characterSheet) {

        CmdClearArmor();
        CmdClearProf();
        CmdClearInjuries();

        foreach (var injury in characterSheet.medicalData.GetInjures())
            CmdAddInjury(injury);

        foreach(var prof in characterSheet.meleeCombatStats.GetProficiencies())
            CmdAddProf(prof.Key, prof.Value);

        foreach (var ap in characterSheet.meleeCombatStats.armorPieces)
            CmdAddArmor(ap.Name);

        CmdUpdateCharacterSheet(characterSheet);

    }
    

}
