using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterAnimator))]
public class CharacterAnimatorEditorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterAnimator ca = (CharacterAnimator)target;

        if (GUILayout.Button("Idle"))
        {
            ca.SetIdle();
        }

        if (GUILayout.Button("Walk"))
        {
            ca.SetWalk();
        }

        if (GUILayout.Button("Swing"))
        {
            ca.SetSwing();
        }

        if (GUILayout.Button("Stab"))
        {
            ca.SetStab();
        }



    }
}
