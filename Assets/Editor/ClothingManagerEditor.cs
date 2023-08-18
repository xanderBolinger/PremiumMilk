using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClothingManager))]
public class ClothingManagerEditorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ClothingManager cm = (ClothingManager)target;

        if (GUILayout.Button("Equip Item"))
        {
            cm.EquipItem(cm.testItemName, cm.testItemType, cm.testSlotType);
        }


        if (GUILayout.Button("Set Color"))
        {
            cm.SetColor(cm.testColor, cm.testSlotType);
        }


    }
}
