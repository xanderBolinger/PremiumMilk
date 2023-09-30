using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterMagic))]
public class CharacterMagicEditorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterMagic cm = (CharacterMagic)target;

        if (GUILayout.Button("Cast Spell"))
        {
            cm.CastSpell();
        }


    }
}
