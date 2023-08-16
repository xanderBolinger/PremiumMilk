using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class SwingAnimator : MonoBehaviour
{

    public GameObject swing_0;
    public GameObject swing_1;


    public List<GameObject> swingStates;

    CharacterAnimator characterAnimator;

    private void Start()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
        swingStates = new List<GameObject>
        {
            swing_0,
            swing_1
        };
    }

    public void Deactivate()
    {
        foreach (var state in swingStates)
        {
            state.SetActive(false);
        }
    }

    public void Swing(int state)
    {
        if (characterAnimator == null || state >= swingStates.Count)
            return;

        characterAnimator.DeactivateAll();
        swingStates[state].SetActive(true);
    }


}
