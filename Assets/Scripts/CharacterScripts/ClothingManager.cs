using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClothingManager : MonoBehaviour
{

    public SlotType testSlotType;
    public ItemType testItemType;
    public string testItemName = "MailHalberk";
    public Color testColor;

    public enum SlotType { 
        ChestArmor,Body,Legs,LeftHand,RightHand
    }

    public enum ItemType { 
        Armor,Weapon
    }

    public void SetColor(Color color, SlotType slotType) {

        var slots = FindSlots(slotType.ToString());

        foreach(var slot in slots)
        {
            slot.GetComponent<Renderer>().material.color = color;
        }

    }


    public void EquipItem(string name, ItemType itemType, SlotType slotType) {

       var prefab = FindPrefab(name, itemType.ToString());

        var slots = FindSlots(slotType.ToString());

        foreach (var slot in slots) {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
            var instance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instance.transform.parent = slot.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.rotation = Quaternion.identity;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
            
        }
    }


    public GameObject FindPrefab(string name, string itemType)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/"+ itemType + "/"+name+".prefab");

        return asset;
    }

    public List<GameObject> FindSlots(string name) {

        var list = new List<GameObject>();

        foreach (Transform childTransform in transform) {
            foreach (Transform grandchildTransform in childTransform) {
                if (grandchildTransform.gameObject.name == name)
                    list.Add(grandchildTransform.gameObject);
            }
        }

        return list; 
    }

}
