using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ClothingManagerTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ClothingManagerTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    private GameObject player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Character/Player.prefab");

    // A Test behaves as an ordinary method
    [Test]
    public void FindSlotTest()
    {
        var playerInstance = Object.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        var clothingManager = playerInstance.GetComponent<ClothingManager>();

        var slots = clothingManager.FindSlots("ChestArmor");

        Assert.AreEqual(4, slots.Count);
    }

    [Test]
    public void FindArmorPrefabTest() {
        var playerInstance = Object.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        var clothingManager = playerInstance.GetComponent<ClothingManager>();
        var armor = clothingManager.FindPrefab("MailHalberk", "Armor");
        Assert.AreEqual("MailHalberk", armor.name);
    }

    [UnityTest]
    public IEnumerator EquipArmorTest() {
        var playerInstance = Object.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        var clothingManager = playerInstance.GetComponent<ClothingManager>();

        clothingManager.EquipItem("MailHalberk", ClothingManager.ItemType.Armor, ClothingManager.SlotType.ChestArmor);

        int count = 0;

        foreach (Transform childTransform in playerInstance.transform)
        {
            foreach (Transform grandchildTransform in childTransform)
            {
                if (grandchildTransform.gameObject.name == "ChestArmor"
                    && grandchildTransform.childCount == 1) {
                   
                    count++;
                }
                   
            }
        }


        Assert.AreEqual(4,count);

        yield return null;

        clothingManager.EquipItem("MailHalberk", ClothingManager.ItemType.Armor, ClothingManager.SlotType.ChestArmor);

        yield return null;

        int count2 = 0;

        foreach (Transform childTransform in playerInstance.transform)
        {
            foreach (Transform grandchildTransform in childTransform)
            {
                if (grandchildTransform.gameObject.name == "ChestArmor"
                    && grandchildTransform.childCount == 1) {
                    
                    count2++;
                }

            }
        }


        Assert.AreEqual(4, count2);

    }

}
