using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{

    private GameObject hoveredCharacter;


    private void Update()
    {
        CheckCharacterSelection();
    }

    public void CheckCharacterSelection()
    {
        var character = PlayerMouseController.GetHoveredCharacter();
        if (character == null && hoveredCharacter != null)
        {
            Deselect(hoveredCharacter);
            return;
        }
        else if (character == null)
            return;
        else if (character == hoveredCharacter)
            return;

        if(hoveredCharacter != null)
            Deselect(hoveredCharacter);
        Select(character);

    }

    private void PlaySelectSound() {
        var source = GameObject.Find("MovedTargetSoundEffect").GetComponent<AudioSource>();
        source.Play();
    }

    private void Select(GameObject character)
    {
        PlaySelectSound();
        hoveredCharacter = character;
        character.GetComponent<OutlineController>().TurnOnSelector();
    }

    private void Deselect(GameObject character) {
        hoveredCharacter = null;
        character.GetComponent<OutlineController>().TurnOffSelector();
    }


}
