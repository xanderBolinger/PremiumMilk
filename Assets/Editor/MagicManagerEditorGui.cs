using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagicManager))]
public class MagicManageEditorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MagicManager mm = (MagicManager)target;

        if (GUILayout.Button("Cast Spell"))
        {
            mm.CastSpellTest();
        }


    }
}
