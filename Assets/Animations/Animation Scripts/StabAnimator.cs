using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabAnimator : MonoBehaviour
{
    public GameObject stab_0;
    public GameObject stab_1;


    public List<GameObject> stabStates;

    CharacterAnimator characterAnimator;

    private void Start()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
        stabStates = new List<GameObject>
        {
            stab_0,
            stab_1,
        };
    }

    public void Deactivate()
    {
        foreach (var state in stabStates)
        {
            state.SetActive(false);
        }
    }

    public void Stab(int state)
    {
        if (characterAnimator == null || state >= stabStates.Count)
            return;

        characterAnimator.DeactivateAll();
        stabStates[state].SetActive(true);
    }
}
