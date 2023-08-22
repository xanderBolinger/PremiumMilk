using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public static CombatManager combatManager;

    public int selectedCharacterIndex;
    public List<CharacterSheet> characterSheets;

    // Start is called before the first frame update
    void Start()
    {
        combatManager = this;
        characterSheets = CharacterSheetLoader.LoadCharacterData();
    }

    public static string CharacterName() {
        return combatManager.characterSheets[combatManager.selectedCharacterIndex].name;
    }

    public List<string> GetCharacterNames() {
        List<string> names = new List<string>();

        foreach (var character in characterSheets) {
            names.Add(character.name);
        }

        return names;
    }
}
