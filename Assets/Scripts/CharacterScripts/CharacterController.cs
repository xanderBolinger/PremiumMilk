using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

[RequireComponent(typeof(CharacterSoundEffects))]
public class CharacterController : MonoBehaviour
{
    
    public CharacterSoundEffects characterSoundEffects;
    public bool player = false;
    public bool npcTarget = false;


    public void Start()
    {
        characterSoundEffects = GetComponent<CharacterSoundEffects>();
    }

    public static GameObject GetCharacterObject(string characterName) {

        foreach (var c in GameObject.FindGameObjectsWithTag("Character")) {
            if (c.GetComponent<CharacterNetwork>() != null &&
                c.GetComponent<CharacterNetwork>().GetCharacterSheet() != null &&
                c.GetComponent<CharacterNetwork>().GetCharacterSheet().name == characterName) {
                return c;
            }
        }

        return null; 
    }
    
}
