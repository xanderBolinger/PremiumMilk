using UnityEngine;
using System.Collections.Generic;

namespace Character {
    public class CharacterSheetLoader
    {

        public static List<CharacterSheet> LoadCharacterData()
        {
            List<CharacterSheet> characterSheets = new List<CharacterSheet>();
            TextAsset csvFile = Resources.Load<TextAsset>("CharacterData");

            string[] data = csvFile.text.Split(new char[] { '\n' });

            for (int i = 1; i < data.Length; i++) // Start from index 1 to skip the header row
            {
                string[] row = data[i].Split(new char[] { ',' });

                string name = row[0];
                string speciesName = row[1];
                int str = int.Parse(row[2]);
                int hlt = int.Parse(row[3]);
                int agl = int.Parse(row[4]);
                int per = int.Parse(row[5]);
                int wil = int.Parse(row[6]);

                string[] armorPieces = row[7].Split(new char[] { ';' });
                string weapon = row[8];
                string shield = row[9];
                string[] proficiencies = row[10].Split(new char[] { ';' });
                string[] values = row[11].Split(new char[] { ';' });

                Species species = SpeciesLoader.GetSpeciesByName(speciesName);

                Attributes attributes = new Attributes(str, hlt, agl, per, wil);

                MedicalData medicalData = new MedicalData(attributes, species);

                MeleeCombatStats meleeCombatStats = new MeleeCombatStats();

                foreach (string armorPieceName in armorPieces)
                {
                    if (armorPieceName == "")
                        continue;

                    var armorPiece = ArmorLoader.GetArmorPieceByName(armorPieceName);

                    meleeCombatStats.armorPieces.Add(armorPiece);
                }


                if(weapon != "")
                    meleeCombatStats.weapon = MeleeWeaponLoader.GetWeaponByName(weapon);
                if(shield != "")
                    meleeCombatStats.shield = MeleeShieldLoader.GetShieldByName(shield);

                for (int j = 0; j < proficiencies.Length; j++)
                {
                    string proficiencyName = proficiencies[j];
                    int value = int.Parse(values[j]);
                    System.Enum.TryParse(proficiencyName, out MeleeProficiencies.MeleeProfType meleeProfType);
                    meleeCombatStats.LearnProficiency(MeleeProficiencies.GetProfByType(meleeProfType), value);
                }

                CharacterSheet characterSheet = new CharacterSheet(name, species, attributes, medicalData, meleeCombatStats);
                characterSheet.meleeCombatStats.CalcReflexes(attributes);
                characterSheets.Add(characterSheet);
            }

            return characterSheets;
        }

        public static CharacterSheet GetCharacterSheetByName(string characterName)
        {
            List<CharacterSheet> characterSheets = LoadCharacterData();

            foreach (CharacterSheet characterSheet in characterSheets)
            {
                if (characterSheet.name == characterName)
                {
                    return characterSheet;
                }
            }

            throw new System.Exception("Character sheet not found: " + characterName);
        }

    }
}


