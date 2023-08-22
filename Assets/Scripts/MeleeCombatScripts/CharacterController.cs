using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using static MeleeProficiencies;

public class CharacterController : MonoBehaviour
{
    public List<string> weaponNames;
    public List<string> shieldNames;
    public List<string> armorNames;
    public List<string> armorPieces;

    public int selectedWeaponIndex;
    public int selectedShieldIndex;
    public int selectedArmorIndex;

    public int setKO;

    public int injuryIndex;
    public string weaponDealt;
    public string characterDealt;
    public int shock;
    public int pD;
    public int bloodlessPD;
    public int pain;
    
    public MeleeProfType meleeProfType;

    private void Start()
    {
        weaponNames = MeleeWeaponLoader.GetWeaponNames();
        shieldNames = MeleeShieldLoader.GetShieldNames();
        armorNames = ArmorLoader.GetArmorNames();
        armorPieces = new List<string>();
    }

    public static CharacterSheet GetCharacter(string characterName) {

        foreach (var character in CombatManager.combatManager.characterSheets) {
            if (character.name == characterName) {
                return character;
            }
        }

        throw new System.Exception("character not found for character name: "+characterName); 
    }

    public void SetWeapon() {
        var character = GetCharacter(CombatManager.CharacterName());

        MeleeWeaponStatBlock stats = MeleeWeaponLoader.GetWeaponByName(weaponNames[selectedWeaponIndex]);

        character.meleeCombatStats.weapon = stats;

        Debug.Log("Set Weapon");

    }

    public void SetShield()
    {
        var character = GetCharacter(CombatManager.CharacterName());

        var shield = MeleeShieldLoader.GetShieldByName(shieldNames[selectedShieldIndex]);

        character.meleeCombatStats.shield = shield;

        Debug.Log("Set Shield");

    }

    public void AddArmorPieceToList() {
        armorPieces.Add(armorNames[selectedArmorIndex]);
    }

    public void SetArmor() {
        var character = GetCharacter(CombatManager.CharacterName());

        List<ArmorPiece> pieces = new List<ArmorPiece>();

        foreach (var armorName in armorPieces) {
            pieces.Add(ArmorLoader.GetArmorPieceByName(armorName));
        }

        character.meleeCombatStats.armorPieces.Clear();

        character.meleeCombatStats.armorPieces = pieces;

        Debug.Log("Set Armor");
    }

    public void RemoveInjury() {
        var character = GetCharacter(CombatManager.CharacterName());

        character.medicalData.RemoveInjury(injuryIndex);
    }

    public void AddInjury() {
        var character = GetCharacter(CombatManager.CharacterName());

        Injury injury = new Injury(pD, bloodlessPD, pain, shock, weaponDealt, characterDealt, "Inspector Injury", 1);
        character.medicalData.AddInjury(injury);

        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts) {
            if (character == bout.combatantA.characterSheet)
            {
                bout.combatantA.ApplyShock(shock);
                bout.combatantA.ApplyPain(pain);
            }
            else if (character == bout.combatantB.characterSheet) {
                bout.combatantB.ApplyShock(shock);
                bout.combatantB.ApplyPain(pain);
            }
        }

    }

    public void SetKO() {
        var character = GetCharacter(CombatManager.CharacterName());
        character.medicalData.knockoutValue = setKO;
    }

    public void SetCurrProf()
    {
        var character = CombatManager.combatManager.characterSheets[CombatManager.combatManager.selectedCharacterIndex];

        character.meleeCombatStats.SetCurrProf(meleeProfType);

        foreach (var bout in MeleeCombatManager.meleeCombatManager.bouts) {
            if (bout.combatantA.characterSheet == character)
            {
                bout.combatantA.diceAssignedToBout = 0;
            }
            else if (bout.combatantB.characterSheet == character) {
                bout.combatantB.diceAssignedToBout = 0;
            }   
        }

        Debug.Log(character.name + " set current prof " + MeleeProficiencies.GetProfByType(meleeProfType).name + " at " + character.meleeCombatStats.profValue + " ranks");
    }

    public void ListCharacters() {

        var i = 0; 
        foreach (var sheet in CombatManager.combatManager.characterSheets) {

            PrintCharacter(sheet, i);
        }

    }

    public void ListSelectedCharacter() {
        var character = CombatManager.combatManager.characterSheets[CombatManager.combatManager.selectedCharacterIndex];
        PrintCharacter(character);
    }

    public static void PrintCharacter(CharacterSheet sheet, int i=1) {
        string weapon = sheet.meleeCombatStats.weapon != null ?
                sheet.meleeCombatStats.weapon.weaponName : "None";

        string armor = "";

        foreach (var piece in sheet.meleeCombatStats.armorPieces)
        {
            armor += piece.Name + ", ";
        }
        int pain = sheet.medicalData.GetPain();
        int maxCP = sheet.meleeCombatStats.GetMaxCp(pain);
        Debug.Log(i + ":: " + sheet.name + " Alive: " + sheet.medicalData.alive + ", Conscious: " + sheet.medicalData.conscious
            + ", Prof(" + sheet.meleeCombatStats.profValue + "): " + sheet.meleeCombatStats.currProf
            + ", Weapon: " + weapon + ", Shield: " + (sheet.meleeCombatStats.shield != null ? sheet.meleeCombatStats.shield.shieldName : "None")
            + ", Armor: " + armor + ", Max CP: " + maxCP + ", KO: " + sheet.medicalData.knockoutValue);
        Debug.Log("PD: " + sheet.medicalData.GetPD()
            + ", BLPD: " + sheet.medicalData.GetBloodlossPD()
            + ", PAIN: " + sheet.medicalData.GetPain());
        sheet.medicalData.PrintInjuries();
        i++;
    }

    public static GameObject GetCharacterObject(string characterName)
    {

        foreach (var c in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (c.GetComponent<CharacterNetwork>() != null &&
                c.GetComponent<CharacterNetwork>().GetCharacterSheet() != null &&
                c.GetComponent<CharacterNetwork>().GetCharacterSheet().name == characterName)
            {
                return c;
            }
        }

        return null;
    }

}
