using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Character.Species;
using Character;
using static MeleeProficiencies;

public class CharacterCreator : MonoBehaviour
{
    public Color color;
    public string characterName = "New Character Name";
    public int str = 10;
    public int per = 10;
    public int agl = 10;
    public int hlt = 10;
    public int wil = 10;
    public SpeciesType speciesType = SpeciesType.MEDIUM_SIZE;
    public int arms = 2;
    public int legs = 2;
    public string weaponName;
    public List<string> armorPieces;

    public MeleeProfType meleeProfType;
    public int profValue;

    public void CreateCharacter() {
        foreach (var sheet in CombatManager.combatManager.characterSheets) {
            if (sheet.name == characterName) {
                Debug.Log("Pick a name that isn't already in use.");
                return;
            }
        }

        var attributes = new Attributes(str, hlt, agl, per, wil);
        Species species = new Species(speciesType, arms, legs);
        MedicalData medical = new MedicalData(attributes, species);
        MeleeCombatStats meleeCombatStats = new MeleeCombatStats();
        MeleeWeaponStatBlock wep = null;

        if (weaponName != "") {
            wep = MeleeWeaponLoader.GetWeaponByName(weaponName);
        }

        meleeCombatStats.weapon = wep;

        foreach (var armor in armorPieces) {
            var piece = ArmorLoader.GetArmorPieceByName(armor);
            if (piece != null)
                meleeCombatStats.armorPieces.Add(piece);
        }

        CharacterSheet characterSheet = new CharacterSheet(characterName, species, attributes, medical, meleeCombatStats);

        CombatManager.combatManager.characterSheets.Add(characterSheet);
        Debug.Log("Created character "+characterSheet.name);
        characterSheet.meleeCombatStats.CalcReflexes(characterSheet.attributes);

        GetComponent<MeleeCombatController>().selectedCharacterList.Add(characterName);
        GetComponent<MeleeCombatController>().targetCharacterList.Add(characterName);
    }

    public void LearnProf() {
        var character = CharacterController.GetCharacter(characterName);

        character.meleeCombatStats.LearnProficiency(MeleeProficiencies.GetProfByType(meleeProfType), profValue);
        character.meleeCombatStats.SetCurrProf(meleeProfType);

        Debug.Log(character.name+" learned prof "+ MeleeProficiencies.GetProfByType(meleeProfType).name+ " at " + profValue +" ranks");
    }

    

}
