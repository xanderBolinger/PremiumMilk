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
        HeadArmor,ChestArmor,Body,Legs,LeftHand,RightHand
    }

    public enum ItemType { 
        Armor,Weapons,Shield
    }

    public void SetColor(BodyColor bodyColor, SlotType slotType) {

        if (slotType == SlotType.ChestArmor)
            slotType = SlotType.Body;

        var slots = FindSlots(slotType.ToString());

        foreach(var slot in slots)
        {
            slot.GetComponent<Renderer>().material.color = bodyColor.color;
        }

    }

    public void UnequipItem(SlotType slotType) {
        var slots = FindSlots(slotType.ToString());

        foreach (var slot in slots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void EquipItem(string name, ItemType itemType, SlotType slotType) {
        if (itemType == ItemType.Shield) // TODO shield
            return;

       var prefab = FindPrefab(name, itemType.ToString());

        if (prefab == null)
            return;

        var bodyColor = prefab.GetComponent<BodyColor>();

        if(bodyColor != null)
            SetColor(bodyColor, slotType);

        if (slotType == SlotType.Legs) 
            return;


       var slots = FindSlots(slotType.ToString());

       UnequipItem(slotType);

        foreach (var slot in slots) {
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
        GameObject asset = Resources.Load<GameObject>("/Prefabs/"+ itemType + "/"+name+".prefab");

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
