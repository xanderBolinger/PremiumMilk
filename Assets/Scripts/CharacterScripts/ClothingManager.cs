using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingManager : MonoBehaviour
{


    public void EquipChestArmor() { 
    
    }


    public List<GameObject> FindSlots(string name) {

        var list = new List<GameObject>();

        foreach (Transform childTransform in transform) {
            if (childTransform.gameObject.name == name)
                list.Add(childTransform.gameObject);

        }

        return list; 
    }

}
