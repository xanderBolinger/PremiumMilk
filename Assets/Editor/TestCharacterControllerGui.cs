using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TestCharacterController))]
public class TestCharacterControllerGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestCharacterController tcc = (TestCharacterController)target;

        if (CombatManager.combatManager == null)
            return;


        GUIContent selectedCharacterList = new GUIContent("Select Character");
        CombatManager.combatManager.selectedCharacterIndex = EditorGUILayout.Popup(selectedCharacterList, CombatManager.combatManager.selectedCharacterIndex,
            CombatManager.combatManager.GetCharacterNames().ToArray());

        GUIContent selectedWeaponList = new GUIContent("Select Weapon");
        tcc.selectedWeaponIndex = EditorGUILayout.Popup(selectedWeaponList, tcc.selectedWeaponIndex,
           tcc.weaponNames.ToArray());

        GUIContent selectedShieldList = new GUIContent("Select Shield");
        tcc.selectedShieldIndex = EditorGUILayout.Popup(selectedShieldList, tcc.selectedShieldIndex,
           tcc.shieldNames.ToArray());

        GUIContent selectedArmorList = new GUIContent("Select Armor");
        tcc.selectedArmorIndex = EditorGUILayout.Popup(selectedArmorList, tcc.selectedArmorIndex,
           tcc.armorNames.ToArray());

        if (GUILayout.Button("Set Current Prof"))
        {
            tcc.SetCurrProf();
        }

        if (GUILayout.Button("Set Weapon"))
        {
            tcc.SetWeapon();
        }

        if (GUILayout.Button("Set Shield"))
        {
            tcc.SetShield();
        }

        if (GUILayout.Button("Set KO"))
        {
            tcc.SetKO();
        }

        if (GUILayout.Button("Add Armor Piece to List"))
        {
            tcc.AddArmorPieceToList();
        }

        if (GUILayout.Button("Set Armor"))
        {
            tcc.SetArmor();
        }

        if (GUILayout.Button("Remove Injury"))
        {
            tcc.RemoveInjury();
        }

        if (GUILayout.Button("Add Injury"))
        {
            tcc.AddInjury();
        }

        if (GUILayout.Button("List Characters"))
        {
            tcc.ListCharacters();
        }

        if (GUILayout.Button("List Selected Character"))
        {
            tcc.ListSelectedCharacter();
        }


        
    }
}
