using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestCombatNetwork))]
public class TestCombatNetworkGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestCombatNetwork tcc = (TestCombatNetwork)target;

        if (GUILayout.Button("Set Selected Character"))
        {
            tcc.SetCharacter(GridManager.gridManager.selectedCharacter, CombatManager.combatManager.selectedCharacterIndex);
        }

        if (GUILayout.Button("Print Selected Character"))
        {
            tcc.PrintCharacter(GridManager.gridManager.selectedCharacter);
        }

    }
}
