using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimator
{

    List<GameObject> states { get; set; }

    void Deactivate() {
        foreach (var state in states) { 
            state.SetActive(false);
        }
    }

    void SetState(int i) { 

        Deactivate();

        if(i < 0 || i > states.Count)
        {
            throw new System.Exception("State i: "+i+", out of bounds for states count: "+states.Count);
        }

        states[i].SetActive(true);
    }
}
