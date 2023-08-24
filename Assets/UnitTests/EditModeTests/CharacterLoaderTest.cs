using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Character;

public class CharacterLoaderTest
{

    [Test]
    public void LoadCharacterData_ValidCSVFile_ReturnsCharacterSheetList()
    {
        var characterSheets = CharacterSheetLoader.LoadCharacterData();

        Assert.IsNotNull(characterSheets);
        Assert.AreEqual(true, characterSheets.Count >= 2);

        // Assert the properties of the first character sheet
        var characterSheet1 = characterSheets[0];
        Assert.AreEqual("Test1", characterSheet1.name);
        Assert.AreEqual("Human", characterSheet1.species.speciesName);
        Assert.AreEqual(10, characterSheet1.attributes.str);
        Assert.AreEqual(11, characterSheet1.attributes.hlt);
        Assert.AreEqual(12, characterSheet1.attributes.agl);
        Assert.AreEqual(13, characterSheet1.attributes.per);
        Assert.AreEqual(14, characterSheet1.attributes.wil);
        Assert.AreEqual("Chain Coif", characterSheet1.meleeCombatStats.armorPieces[0].Name);
        Assert.AreEqual("Cuirass", characterSheet1.meleeCombatStats.armorPieces[1].Name);
        Assert.AreEqual("Light Mace", characterSheet1.meleeCombatStats.weapon.weaponName);
        Assert.AreEqual("Medium Shield", characterSheet1.meleeCombatStats.shield.shieldName);
        Assert.AreEqual("Cut and Thrust", characterSheet1.meleeCombatStats.GetProf().name);
        Assert.AreEqual(10, characterSheet1.meleeCombatStats.GetProficiencies()[characterSheet1.meleeCombatStats.currProf]);

        // Assert the properties of the second character sheet
        var characterSheet2 = characterSheets[1];
        Assert.AreEqual("Test2", characterSheet2.name);
        Assert.AreEqual("Goblin", characterSheet2.species.speciesName);
        Assert.AreEqual(9, characterSheet2.attributes.str);
        Assert.AreEqual(8, characterSheet2.attributes.hlt);
        Assert.AreEqual(9, characterSheet2.attributes.agl);
        Assert.AreEqual(8, characterSheet2.attributes.per);
        Assert.AreEqual(7, characterSheet2.attributes.wil);
        Assert.AreEqual("Padded Coif", characterSheet2.meleeCombatStats.armorPieces[0].Name);
        Assert.AreEqual("Chausses", characterSheet2.meleeCombatStats.armorPieces[1].Name);
        Assert.AreEqual("Spear One Handed", characterSheet2.meleeCombatStats.weapon.weaponName);
        Assert.AreEqual("Round Shield", characterSheet2.meleeCombatStats.shield.shieldName);
        Assert.AreEqual("Cut and Thrust", characterSheet2.meleeCombatStats.GetProf().name);
        Assert.AreEqual(6, characterSheet2.meleeCombatStats.GetProficiencies()[characterSheet2.meleeCombatStats.currProf]);
    }

    [Test]
    public void GetCharacterSheetByName_ValidCharacterName_ReturnsCharacterSheet()
    {
        var characterSheet1 = CharacterSheetLoader.GetCharacterSheetByName("Test1");

        Assert.IsNotNull(characterSheet1);
        Assert.AreEqual("Test1", characterSheet1.name);
        Assert.AreEqual("Human", characterSheet1.species.speciesName);

        var characterSheet2 = CharacterSheetLoader.GetCharacterSheetByName("Test2");

        Assert.IsNotNull(characterSheet2);
        Assert.AreEqual("Test2", characterSheet2.name);
        Assert.AreEqual("Goblin", characterSheet2.species.speciesName);
    }

    [Test]
    public void GetCharacterSheetByName_InvalidCharacterName_ThrowsException()
    {
        Assert.Throws<System.Exception>(() => CharacterSheetLoader.GetCharacterSheetByName("NonExistentCharacter"));
    }

}
