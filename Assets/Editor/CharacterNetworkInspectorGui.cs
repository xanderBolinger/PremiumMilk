using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterNetwork))]
public class CharacterNetworkInspectorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterNetwork cn = (CharacterNetwork)target;

        if (GUILayout.Button("Print Character"))
        {
            cn.Print();
        }

    }
}
