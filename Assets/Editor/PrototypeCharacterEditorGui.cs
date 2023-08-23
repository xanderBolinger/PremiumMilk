using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrototypeCharacter))]
public class PrototypeCharacterEditorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PrototypeCharacter pc = (PrototypeCharacter)target;

        if (GUILayout.Button("Regular Set Syncvar"))
        {
            pc.SetSyncVar(pc.futureSyncVarValue);
        }

        if (GUILayout.Button("Cmd Set Syncvar"))
        {
            pc.CmdSetSyncVar(pc.futureSyncVarValue);
        }


    }
}
