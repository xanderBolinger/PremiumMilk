using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class WalkAnimator : MonoBehaviour, IAnimator
{
    public List<GameObject> states { get; set; }


    public List<GameObject> WalkStates;

    public void Start()
    {
        states = WalkStates;
    }

    public void Walk(int frame) {
        IAnimator animator = this;
        animator.SetState(frame);
    }

}
