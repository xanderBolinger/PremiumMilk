using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefensiveManuevers;
using static MeleeCombatManager;
using static OffensiveManuevers;
using static ExcelUtillity.MeleeHitLocation;
using static TargetZone;

public class MeleeCombatController : MonoBehaviour
{
    public enum MeleeCombatPhase {
        DECLARE,ATTACK,DEFEND,RESOLVE
    }

    public MeleeCombatPhase meleeCombatPhase { get; private set; }

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
    public int selectedCharacterIndex;
    [HideInInspector]
    public List<string> selectedCharacterList = new List<string>();
    [HideInInspector]
    public int targetCharacterIndex;
    [HideInInspector]
    public List<string> targetCharacterList = new List<string>();
    [HideInInspector]
    public int selectedBoutIndex;
    [HideInInspector]
    public List<string> selectedBoutList = new List<string>();


    public static MeleeCombatController meleeCombatController;

    CombatNetworkController combatNetworkController;

    public bool resolveMeleeCombatFlag = false;

    IEnumerator Start()
    {
        meleeCombatController = this;

        yield return new WaitUntil(() => CombatManager.combatManager != null);

        foreach (var characterSheet in CombatManager.combatManager.characterSheets)
        {
            selectedCharacterList.Add(characterSheet.name);
            targetCharacterList.Add(characterSheet.name);
        }

        combatNetworkController = GetComponent<CombatNetworkController>();

    }

    public bool CharactersInCombat() {
        return meleeCombatManager.bouts.Count > 0;
    }

    public void EnterCombat() {

        var initiator = CharacterController.GetCharacter(selectedCharacterList[selectedCharacterIndex]);
        var target = CharacterController.GetCharacter(targetCharacterList[targetCharacterIndex]);

        meleeCombatManager.EnterMeleeCombat(initiator, target);
        selectedBoutList.Add("A: " + initiator.name + ", B: " + target.name);
        Debug.Log("Created bout between " + initiator.name + ", target: " + target.name);
    }

    public void AssignDice() {
        var combatant = GetCombatant();
        var characterSheet = combatant.characterSheet;
        int cpAssignedToOtherBouts = 0;
        combatant.AssignCP(0, cpAssignedToOtherBouts);

        foreach (var bout in meleeCombatManager.bouts) {
            if (bout.combatantA == combatant)
            {
                cpAssignedToOtherBouts += bout.combatantA.diceAssignedToBout;
            }
            else if(bout.combatantB == combatant){
                cpAssignedToOtherBouts += bout.combatantB.diceAssignedToBout;
            }
        }

        if (combatant.AssignCP(dice, cpAssignedToOtherBouts))
            Debug.Log("Bout " + selectedBoutIndex + ", Combatant: " + combatant.characterSheet.name + ", assign " + dice + " dice.");
        else {
            int pain = characterSheet.medicalData.GetPain();
            int maxCP = characterSheet.meleeCombatStats.GetMaxCp(pain);
            Debug.Log("Could not assign dice. Bout " + selectedBoutIndex + ", Combatant: " + combatant.characterSheet.name + ", failed to assign " + dice + " dice, Max CP: "+ maxCP);
        }

        combatNetworkController.UpdateCharacters();

    }

    public void RefreshWeapons() {

        foreach (var bout in meleeCombatManager.bouts) {

            bout.combatantA.meleeWeaponStatBlock = bout.combatantA.characterSheet.meleeCombatStats.weapon;
            bout.combatantB.meleeWeaponStatBlock = bout.combatantB.characterSheet.meleeCombatStats.weapon;
        }
    
    }

    public void RefilDice() {

        foreach (var bout in meleeCombatManager.bouts) {

            bout.combatantA.currentDice = bout.combatantA.diceAssignedToBout;
            bout.combatantB.currentDice = bout.combatantB.diceAssignedToBout;
        
        }

        Debug.Log("Refil Dice.");
        combatNetworkController.UpdateCharacters();
    }

    public void ListBouts() {

        var declareBouts = meleeCombatManager.GetDeclareBouts();

        int i = 0;
        foreach (var bout in meleeCombatManager.bouts) {

            string initativeCombatant;

            if (declareBouts.Contains(bout))
            {
                initativeCombatant = "Combatant A: " + bout.combatantA.meleeDecision + ", Combatant B: " + bout.combatantB.meleeDecision;
            }
            else { 
                initativeCombatant = bout.initativeCombatant != null ? (bout.initativeCombatant == bout.combatantA ? "A" : "B") 
                    : "DECLARED "+bout.combatantA.meleeDecision+"/"+bout.combatantB.meleeDecision;
            }

            if (bout.onPause)
            {
                Debug.Log(i + "::Bout A: " + bout.combatantA.characterSheet.name + ", " + bout.combatantA.currentDice + "/" + bout.combatantA.diceAssignedToBout
                    + " B: " + bout.combatantB.characterSheet.name + ", " + bout.combatantB.currentDice + "/" + bout.combatantB.diceAssignedToBout
                    + ", ON PAUSE");
            }
            else {
                Debug.Log(i + "::Bout A: " + bout.combatantA.characterSheet.name + ", " + bout.combatantA.currentDice + "/" + bout.combatantA.diceAssignedToBout
                    + " B: " + bout.combatantB.characterSheet.name + ", " + bout.combatantB.currentDice + "/" + bout.combatantB.diceAssignedToBout
                    + ", I: " + initativeCombatant);
                Debug.Log("--Combatant A Manuever: " + (bout.combatantA.selectManuever == null ? "NONE" : bout.combatantA.selectManuever.ToString())
                    +", Shield Beaten: "+bout.combatantA.shieldBeaten+", Weapon Beaten: "+bout.combatantA.weaponBeaten);
                Debug.Log("--Combatant B Manuever: " + (bout.combatantB.selectManuever == null ? "NONE" : bout.combatantB.selectManuever.ToString()) 
                    + ", Shield Beaten: " + bout.combatantB.shieldBeaten + ", Weapon Beaten: " + bout.combatantB.weaponBeaten);
            }
            
            i++;
        }

    }

    public void RemoveBout(bool update=true) {
        if (selectedBoutIndex >= selectedBoutList.Count || selectedBoutIndex < 0)
            return;

        meleeCombatManager.bouts.RemoveAt(selectedBoutIndex);
        selectedBoutList.RemoveAt(selectedBoutIndex);
        Debug.Log("Remove bout: " + selectedBoutIndex);
        if(update)
            combatNetworkController.UpdateCharacters();
    }

    public void TryAdvance()
    {
        switch (meleeCombatPhase)
        {
            case MeleeCombatPhase.DECLARE:
                if (meleeCombatManager.GetDeclareBouts().Count <= 0)
                    AdvanceCombat();
                else
                    combatNetworkController.SendDeclareMessages();
                break;
            case MeleeCombatPhase.ATTACK:
                if (meleeCombatManager.GetAttackersWithoutManeuver().Count <= 0)
                    AdvanceCombat();
                else
                    combatNetworkController.SendAttackerMessages();
                break;
            case MeleeCombatPhase.DEFEND:
                if (meleeCombatManager.GetDefendersWithoutManuever().Count <= 0)
                    AdvanceCombat();
                else
                    combatNetworkController.SendDefenderMessages();
                break;
        }
    }

    public void AdvanceCombat() {

        if (!resolveMeleeCombatFlag)
            return;

        if(meleeCombatManager.bouts.Count <= 0)
        {
            meleeCombatPhase = MeleeCombatPhase.DECLARE;
            return; 
        }

        //MeleeCombatAIController.DeclareAI();
        var declareBouts = meleeCombatManager.GetDeclareBouts();

        if (declareBouts.Count > 0) {
            combatNetworkController.SendDeclareMessages();
            return;
        }

        CombatLog.LogDeclare();

        meleeCombatPhase = MeleeCombatPhase.ATTACK;
        //MeleeCombatAIController.DeclareAttack();
        var attackers = meleeCombatManager.GetAttackersWithoutManeuver();

        if (attackers.Count > 0) {
            combatNetworkController.SendAttackerMessages();
            return;
        }

        meleeCombatPhase = MeleeCombatPhase.DEFEND;

        //MeleeCombatAIController.DeclareDefense();
        var defenders = meleeCombatManager.GetDefendersWithoutManuever();
        if (defenders.Count > 0) {
            combatNetworkController.SendDefenderMessages();
            return;
        }

        meleeCombatPhase = MeleeCombatPhase.RESOLVE;

        Debug.Log("Resolve Exchanges");
        Resolve();

        meleeCombatPhase = MeleeCombatPhase.DECLARE;
    }

    public void Resolve() {

        

        foreach (var bout in meleeCombatManager.bouts) {
            if (bout.combatantA.characterSheet.medicalData.conscious == false)
            {
                Debug.Log("Combatant is unconscious "+bout.combatantA.characterSheet.name);
                return;
            }
            else if (bout.combatantB.characterSheet.medicalData.conscious == false) {
                Debug.Log("Combatant is unconscious " + bout.combatantB.characterSheet.name);
                return;
            }
        }

        var declareBouts = meleeCombatManager.GetDeclareBouts();

        if (declareBouts.Count > 0) {
            Debug.Log("Declare bouts count greater than 0");
            return;
        }

        foreach (var bout in meleeCombatManager.bouts) {
            if (bout.combatantA.meleeDecision == MeleeStatus.BLUE && bout.combatantB.meleeDecision == MeleeStatus.BLUE) {
                bout.combatantA.selectManuever = new SelectManuever();
                bout.combatantB.selectManuever = new SelectManuever();
            }
                
            if (bout.onPause)
                continue;

            if (bout.combatantA.selectManuever == null)
            {
                Debug.Log(bout.combatantA.characterSheet.name + " maneuver is empty.");
                return;
            }
            else if (bout.combatantB.selectManuever == null) {
                Debug.Log(bout.combatantB.characterSheet.name + " maneuver is empty.");
                return;
            }
        }

        var removeBoutIndices = new List<int>();

        foreach (var bout in meleeCombatManager.bouts) {
            if (bout.combatantA.meleeDecision == MeleeStatus.LEAVE_COMBAT || bout.combatantB.meleeDecision == MeleeStatus.LEAVE_COMBAT)
                removeBoutIndices.Add(meleeCombatManager.bouts.IndexOf(bout));
        }


        meleeCombatManager.CreateExchanges();

        meleeCombatManager.ResolveExchanges(true);
        
        meleeCombatPhase = MeleeCombatPhase.DECLARE;
        
        Debug.Log("Next Exchange: " + (meleeCombatManager.firstExchange ? "first exchange" : "second exchange"));

        foreach (var index in removeBoutIndices) { 
            meleeCombatManager.bouts.RemoveAt(index);
            selectedBoutList.RemoveAt(index);
        }

        combatNetworkController.UpdateCharacters();

        resolveMeleeCombatFlag = false;
        //GridManager.gridManager.resolveCombatActionFlag = true;
        //GridManager.gridManager.startingAdvanceCalled = false;

    }

    public void Declare() {
        if (meleeDecision == MeleeStatus.UNDECIDED) {
            Debug.Log("Pick decided melee status.");
            return;
        }

        var initiator = GetCombatant();
        initiator.meleeDecision = meleeDecision;
        Debug.Log("Declare melee for "+initiator.characterSheet.name+", "+meleeDecision);
        combatNetworkController.UpdateCharacters();
    }

    public void SetAttack() {
        if (OnPuase())
            return;
        
        var combatant = GetCombatant();
        var target = GetTargetCombatant();

        if (combatant.weaponBeaten) {
            Debug.Log("Attacker weapon beaten.");
            return;
        }

        int reachCost =  GetReachCost();

        var maneuver = OffensiveManuevers.GetManuever(offensiveManueverType);

        if (maneuver.RequiresShield()
            && combatant.characterSheet.meleeCombatStats.shield == null)
        {
            Debug.Log("Manuever requires shield.");
            return;
        }
        
        int activationCost = maneuver.GetActivationCost();

        bool simultaneous = maneuver.SimultaneousDefense();

        if (dice + reachCost + activationCost + 
            (simultaneous ? secondaryDice : 0) > combatant.currentDice)
        {
            Debug.Log("Too many dice assigned for " + combatant.characterSheet.name + ", Dice: " + dice
                + ", reach cost: "+reachCost
                + ", Activation Cost: "+activationCost);
            // SetDoNothing();
            //return;
            dice = 0;
            reachCost = 0;
            activationCost = 0;
        }

        var targetZone = meleeDamageType == MeleeDamageType.PIERICNG ? (int)targetZonePuncture : (int)targetZoneCutting;

        Debug.Log("Set Attack for " + combatant.characterSheet.name + ", " + offensiveManueverType + ", Dice: " + dice +
            (reachCost != 0 ? (", Reach Cost: "+reachCost) : "") +
            ", Target Zone: "+(meleeDamageType == MeleeDamageType.PIERICNG ? targetZonePuncture : targetZoneCutting));

        combatant.selectManuever = new SelectManuever(offensiveManueverType, dice, targetZone, 
            meleeDamageType, reachCost + activationCost);

        if (simultaneous)
            maneuver.SetSimultaneousDefense(secondaryDice, combatant.selectManuever);
        maneuver.SetWeaponBeat(BeatTargetWeapon);
        combatNetworkController.UpdateCharacters();
    }

    public int GetReachCost() {
        Combatant combatant = GetCombatant();
        var targetCombatant = GetTargetCombatant();
        var bout = meleeCombatManager.bouts[selectedBoutIndex];
        int reachCost = MeleeCombatManager.GetReachCost(combatant, targetCombatant, bout);
        Debug.Log("Reach cost: " + reachCost);
        return reachCost;
    }

    public void SetDefense() {
        if (OnPuase())
            return;

        var combatant = GetCombatant();
        var target = GetTargetCombatant();
        var maneuver = DefensiveManuevers.GetManuever(defensiveManueverType);

        if (maneuver.RequiresShield() 
            && (combatant.characterSheet.meleeCombatStats.shield == null || combatant.shieldBeaten))
        {
            Debug.Log("Manuever requires unbeaten shield.");
            return;
        }

        var activationCost = DefensiveManuevers.GetManuever(defensiveManueverType).GetActivationCost();

        if (dice + activationCost + (maneuver.SimultaneousAttack() ? secondaryDice : 0) > combatant.currentDice) {
            Debug.Log("Too many dice assigned for "+combatant.characterSheet.name+", Dice: "+dice);
            SetDoNothing();
            return; 
        }

        Debug.Log("Set Defense for "+combatant.characterSheet.name+", "+defensiveManueverType+", Dice: "+dice);

        CombatLog.Log(combatant.characterSheet.name + " is defending against "+target.characterSheet.name+
            " by performing a "+defensiveManueverType+" defense with "+dice+" dice.");

        combatant.selectManuever = new SelectManuever(defensiveManueverType, dice, activationCost);

        if (maneuver.SimultaneousAttack()) { 
            var targetZone = meleeDamageType == MeleeDamageType.PIERICNG ? (int)targetZonePuncture : (int)targetZoneCutting;
            maneuver.SetSimultaneousAttack(combatant.selectManuever, meleeDamageType, secondaryDice, targetZone);
        }
        combatNetworkController.UpdateCharacters();
    }

    private bool OnPuase()
    {
        var bout = meleeCombatManager.bouts[selectedBoutIndex];
        if (bout.onPause)
        {
            Debug.Log("Bout on pause, can't set manuever.");
            return true;
        }
        return false;
    }

    public void SetDoNothing() {
        if (OnPuase())
            return;

        var combatant = GetCombatant();

        Debug.Log("Set Do nothing for " + combatant.characterSheet.name);
        combatant.selectManuever = new SelectManuever();
        combatNetworkController.UpdateCharacters();
    }

    public Combatant GetCombatant()
    {

        var bout = meleeCombatManager.bouts[selectedBoutIndex];

        if (bout.combatantA.characterSheet.name == selectedCharacterList[selectedCharacterIndex])
        {
            return bout.combatantA;
        }
        else if (bout.combatantB.characterSheet.name == selectedCharacterList[selectedCharacterIndex])
        {
            return bout.combatantB;
        }

        throw new System.Exception("Combatant not found for name: " + selectedCharacterList[selectedCharacterIndex] + ", index: " + selectedBoutIndex);

    }

    public Combatant GetTargetCombatant() {
        var combatant = GetCombatant();

        var bout = meleeCombatManager.bouts[selectedBoutIndex];

        return bout.combatantA == combatant ? bout.combatantB : bout.combatantA;
    }
}
