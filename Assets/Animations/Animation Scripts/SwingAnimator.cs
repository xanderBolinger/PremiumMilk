using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class SwingAnimator : MonoBehaviour, IAnimator
{
    public List<GameObject> states { get; set; }


    public List<GameObject> SwingStates;

    public void Start()
    {
        states = SwingStates;
    }

    public void Swing(int frame)
    {
        IAnimator animator = this;

        animator.SetState(frame);
    }

}
