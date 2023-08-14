using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnimator : MonoBehaviour
{
    public GameObject walk_0;
    public GameObject walk_1;


    public List<GameObject> walkStates;

    CharacterAnimator characterAnimator;

    private void Start()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
        walkStates = new List<GameObject>
        {
            walk_0,
            walk_1,
        };
    }

    public void Deactivate()
    {
        foreach (var state in walkStates)
        {
            state.SetActive(false);
        }
    }

    public void Walk(int state)
    {
        if (characterAnimator == null || state >= walkStates.Count)
            return;

        characterAnimator.DeactivateAll();
        walkStates[state].SetActive(true);
    }
}
