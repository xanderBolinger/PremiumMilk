using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeleeCombatController))]
public class MeleeCombatControllerGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeleeCombatController ocm = (MeleeCombatController)target;

        GUIContent selectedCharacterList = new GUIContent("Select Character");
        ocm.selectedCharacterIndex = EditorGUILayout.Popup(selectedCharacterList, ocm.selectedCharacterIndex, ocm.selectedCharacterList.ToArray());

        GUIContent targetChracterList = new GUIContent("Select Character 2");
        ocm.targetCharacterIndex = EditorGUILayout.Popup(targetChracterList, ocm.targetCharacterIndex, ocm.targetCharacterList.ToArray());

        GUIContent selectedBoutList = new GUIContent("Select Bout");
        ocm.selectedBoutIndex = EditorGUILayout.Popup(selectedBoutList, ocm.selectedBoutIndex, ocm.selectedBoutList.ToArray());

        if (GUILayout.Button("Enter Combat"))
        {
            ocm.EnterCombat();
        }

        if (GUILayout.Button("Assign Dice"))
        {
            ocm.AssignDice();
        }

        if (GUILayout.Button("Refill Dice"))
        {
            ocm.RefilDice();
        }

        if (GUILayout.Button("List Bouts"))
        {
            ocm.ListBouts();
        }

        if (GUILayout.Button("Remove Bout"))
        {
            ocm.RemoveBout();
        }

        if (GUILayout.Button("Advance Combat"))
        {
            ocm.AdvanceCombat();
        }

        if (GUILayout.Button("Resolve Bouts"))
        {
            ocm.Resolve();
        }

        if (GUILayout.Button("Set Attack"))
        {
            ocm.SetAttack();
        }

        if (GUILayout.Button("Get Reach Cost"))
        {
            ocm.GetReachCost();
        }

        if (GUILayout.Button("Set Defense"))
        {
            ocm.SetDefense();
        }

        if (GUILayout.Button("Set Nothing"))
        {
            ocm.SetDoNothing();
        }

        if (GUILayout.Button("Declare"))
        {
            ocm.Declare();
        }

    }
}
