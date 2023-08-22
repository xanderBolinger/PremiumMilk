using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterCreator))]
public class CharacterCreatorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterCreator cc = (CharacterCreator)target;

        if (GUILayout.Button("Create Character"))
        {
            cc.CreateCharacter();
        }

        if (GUILayout.Button("Learn Prof"))
        {
            cc.LearnProf();
        }

        

    }
}
