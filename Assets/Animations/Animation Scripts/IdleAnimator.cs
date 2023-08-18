using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class IdleAnimator : MonoBehaviour, IAnimator
{
    public List<GameObject> states { get; set; }


    public List<GameObject> IdleStates;

    public void Start()
    {
        states = IdleStates;
    }

    public void Idle(int frame)
    {
        IAnimator animator = this;
       
        animator.SetState(frame);
    }

}
