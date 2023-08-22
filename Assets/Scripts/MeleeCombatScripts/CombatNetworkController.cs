using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Character;
using static MeleeCombatController;
using static MeleeCombatManager;

public class CombatNetworkController : NetworkBehaviour
{
    public void SendDefenderMessages()
    {
        foreach (var defender in meleeCombatManager.GetDefendersWithoutManuever())
        {
            foreach (var combatant in defender.Value)
            {
                var combatantName = combatant.characterSheet.name;
                foreach (var cObj in GameObject.FindGameObjectsWithTag("Character"))
                {
                    var network = cObj.GetComponent<CharacterNetwork>();
                    var combatNetwork = cObj.GetComponent<CharacterCombatNetwork>();
                    var name = network.GetCharacterSheet().name;
                    var ui = cObj.GetComponent<MeleeCombatUI>();
                    if (combatantName == name)
                    {
                        combatNetwork.RpcSendMessage(name + " must choose defense in bout " + defender.Key.ToString() + ", attack: " + (
                            defender.Key.combatantA == combatant ? defender.Key.combatantB.selectManuever.ToString()
                            : defender.Key.combatantA.selectManuever.ToString()
                            ));
                        var attacker = defender.Key.combatantA == combatant ? defender.Key.combatantB
                            : defender.Key.combatantA;
                        ui.RpcShowDefense(combatant,
                            attacker, defender.Key, meleeCombatManager.firstExchange,
                            attacker.selectManuever.offensiveManuever.GetManeuverName(),
                            attacker.selectManuever.dice);
                    }
                }
            }
        }
    }

    public void SendAttackerMessages()
    {
        foreach (var attacker in meleeCombatManager.GetAttackersWithoutManeuver())
        {
            foreach (var combatant in attacker.Value)
            {
                var combatantName = combatant.characterSheet.name;
                foreach (var cObj in GameObject.FindGameObjectsWithTag("Character"))
                {
                    var network = cObj.GetComponent<CharacterNetwork>();
                    var combatNetwork = cObj.GetComponent<CharacterCombatNetwork>();
                    var name = network.GetCharacterSheet().name;
                    var ui = cObj.GetComponent<MeleeCombatUI>();
                    if (combatantName == name)
                    {
                        combatNetwork.RpcSendMessage(name + " must choose attack in bout " + attacker.Key.ToString());
                        var targetCombatant = attacker.Key.combatantA == combatant ?
                            attacker.Key.combatantB :
                            attacker.Key.combatantA;
                        var bout = meleeCombatManager.FindBout(combatantName, targetCombatant.characterSheet.name);
                        meleeCombatController.selectedBoutIndex = meleeCombatManager.bouts.IndexOf(bout);
                        meleeCombatController.selectedCharacterIndex = meleeCombatController.selectedCharacterList.IndexOf(combatantName);
                        int reachCost = meleeCombatController.GetReachCost();
                        ui.RpcShowAttack(combatant, 
                            targetCombatant, attacker.Key, meleeCombatManager.firstExchange, reachCost);
                    }

                }
            }
        }
    }

    public void SendDeclareMessages()
    {
        foreach (var b in meleeCombatManager.GetDeclareBouts())
        {

            foreach (var cObj in GameObject.FindGameObjectsWithTag("Character"))
            {

                var network = cObj.GetComponent<CharacterNetwork>();
                var combatNetwork = cObj.GetComponent<CharacterCombatNetwork>();
                var name = network.GetCharacterSheet().name;
                var ui = cObj.GetComponent<MeleeCombatUI>();
                if (b.combatantA.characterSheet.name == name && b.combatantA.meleeDecision == MeleeStatus.UNDECIDED)
                {
                    combatNetwork.RpcSendMessage(name + " must declare.");
                    ui.RcpShowDeclare(b.combatantB.characterSheet.name);
                }
                else if (b.combatantB.characterSheet.name == name && b.combatantB.meleeDecision == MeleeStatus.UNDECIDED)
                {
                    combatNetwork.RpcSendMessage(name + " must declare.");
                    ui.RcpShowDeclare(b.combatantA.characterSheet.name);
                }

                
               

            }

        }
    }

    public void CheckDead() {
        var characterObjects = GameObject.FindGameObjectsWithTag("Character");
        List<string> removeBoutList = new List<string>();
        var deadList = new List<GameObject>();

        foreach (var character in characterObjects)
        {
            var network = character.GetComponent<CharacterNetwork>();
            var characterSheet = network.GetCharacterSheet();
            if (characterSheet == null)
            {
                Debug.LogError(character.name + " does not have a character sheet.");
                continue;
            }

            if (!characterSheet.medicalData.alive || !characterSheet.medicalData.conscious) {

                foreach (var boutString in meleeCombatController.selectedBoutList) {
                    var bout = meleeCombatManager.bouts[meleeCombatController.selectedBoutList.IndexOf(boutString)];
                    if (bout.combatantA.characterSheet.name == characterSheet.name || bout.combatantB.characterSheet.name == characterSheet.name)
                    {
                        int boutIndex = meleeCombatController.selectedBoutList.IndexOf(boutString);
                        removeBoutList.Add(meleeCombatController.selectedBoutList[boutIndex]);
                    }

                }

                deadList.Add(character);

            }

            
        }

        foreach (var dead in deadList) {
            var network = dead.GetComponent<CharacterNetwork>();
            CombatLog.LogDead(network.GetCharacterSheet());
            dead.tag = "Dead";

            if (network.isLocalPlayer)
                network.RpcCallDead();
            else
                network.RpcCallDeadNpc();
            //Destroy(dead);
        }

        foreach (var bout in removeBoutList)
        {
            meleeCombatController.selectedBoutIndex = meleeCombatController.selectedBoutList.IndexOf(bout);
            meleeCombatController.RemoveBout(false);
        }
    }

    public void UpdateCharacters() {

        CheckDead();

        var characterObjects = GameObject.FindGameObjectsWithTag("Character");
        
        foreach(var character in characterObjects)
        {
            var network = character.GetComponent<CharacterNetwork>();
            if (network.GetCharacterSheet() == null) {
                Debug.LogError(character.name+" does not have a character sheet.");
                continue; 
            }

            int characterSheetIndex = CharacterSheetIndex(network.GetCharacterSheet().name);
            SetCharacter(character, characterSheetIndex);
            UpdateBouts(character, network.GetCharacterSheet());
        }

    }

    private int CharacterSheetIndex(string characterName) { 
        foreach(var characterSheet in CombatManager.combatManager.characterSheets)
            if(characterSheet.name == characterName)
                return CombatManager.combatManager.characterSheets.IndexOf(characterSheet);
        throw new System.Exception("Character name not found in combat manager character sheets for character name: "+characterName);
    }
    
    public void PrintCharacter(GameObject character) {
        if (GetNetwork(character) == null) {
            Debug.Log("Select a character");
            return;
        }

        GetNetwork(character).Print();
        //GridManager.gridManager.selectedCharacterNetwork.RcpPrintName();
        //GridManager.gridManager.selectedCharacterNetwork.PrintName();
    }

    public void SetCharacter(GameObject character, int characterSheetIndex)
    {
        if (character == null)
        {
            Debug.Log("Select a character");
            return;
        }

        var characterSheet = CombatManager.combatManager.characterSheets[characterSheetIndex];
        GetNetwork(character).SetCharacterSheet(characterSheet);
        //GridManager.gridManager.GetNetwork(character).Print();

        UpdateBouts(character, characterSheet);

       
        
        //GridManager.gridManager.selectedCharacterNetwork.RpcSetCharacterSheet(character.name, character.attributes);
        //GridManager.gridManager.selectedCharacterNetwork.SetCharacterSheet(character.name, character.attributes);
    }

    public void UpdateBouts(GameObject character, CharacterSheet characterSheet) {

        GetCombatNetwork(character).syncBoutList.Clear();

        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts)
        {
            if (bout.combatantA.characterSheet.name == characterSheet.name)
            {
                GetCombatNetwork(character).syncBoutList.Add(
                    bout.combatantB.characterSheet.name);

            }
            else if (bout.combatantB.characterSheet.name == characterSheet.name)
            {
                GetCombatNetwork(character).syncBoutList.Add(
                    bout.combatantA.characterSheet.name);
            }
        }
    }

    public static CharacterNetwork GetNetwork(GameObject character)
    {
        return character.GetComponent<CharacterNetwork>();
    }

    public static CharacterCombatNetwork GetCombatNetwork(GameObject character)
    {
        return character.GetComponent<CharacterCombatNetwork>();
    }


}
