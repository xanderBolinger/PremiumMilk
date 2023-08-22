using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterCombatNetwork))]
public class CharacterCombatNetworkGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterCombatNetwork ccn = (CharacterCombatNetwork)target;

        GUIContent selectedBoutList = new GUIContent("Select Bout");
        ccn.selectedBoutIndex = EditorGUILayout.Popup(selectedBoutList, ccn.selectedBoutIndex, ccn.selectedBoutList.ToArray());

        if (GUILayout.Button("Declare"))
        {
            ccn.Declare();
        }

        if (GUILayout.Button("Assign Dice"))
        {
            ccn.AssignDice();
        }

        if (GUILayout.Button("Set Attack"))
        {
            ccn.SetAttack();
        }

        if (GUILayout.Button("Set Defense"))
        {
            ccn.SetDefense();
        }

        if (GUILayout.Button("Set Nothing"))
        {
            ccn.SetDoNothing();
        }


    }
}
