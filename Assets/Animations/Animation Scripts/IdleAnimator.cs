using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterAnimator))]
public class IdleAnimator : MonoBehaviour
{

    public GameObject idle_0;
    public GameObject idle_1;


    public List<GameObject> idleStates;

    public CharacterAnimator characterAnimator;

    private void Start()
    {
        characterAnimator = GetComponent<CharacterAnimator>();

        idleStates = new List<GameObject>
        {
            idle_0,
            idle_1
        };

    }

    public void Deactivate() { 
        foreach(var state in idleStates)
        {
            state.SetActive(false); 
        }
    }

    public void Idle(int state) {
        if (characterAnimator == null || state >= idleStates.Count)
            return;

        characterAnimator.DeactivateAll();
        idleStates[state].SetActive(true);
    }


}
