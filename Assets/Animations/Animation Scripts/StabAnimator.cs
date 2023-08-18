using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class StabAnimator : MonoBehaviour, IAnimator
{
    public List<GameObject> states { get; set; }


    public List<GameObject> StabStates;

    public void Start()
    {
        states = StabStates;
    }

    public void Stab(int frame)
    {
        IAnimator animator = this;

        animator.SetState(frame);
    }

}
