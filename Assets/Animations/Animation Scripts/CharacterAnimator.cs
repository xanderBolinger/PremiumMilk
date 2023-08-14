using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIdle() {
       if(animator == null) { return; }

       ClearAnimation();
    }

    public void SetWalk() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsWalking", true);
    }

    public void SetSwing() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsSwinging", true);
    }

    public void SetStab() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsStabbing", true);
    }

    private void ClearAnimation() {
        if (animator == null) { return; }

          animator.SetBool("IsWalking", false);
          animator.SetBool("IsSwinging", false);
          animator.SetBool("IsStabbing", false);
    }

    public void DeactivateAll() { 
        GetComponent<IdleAnimator>()?.Deactivate();
        GetComponent<SwingAnimator>()?.Deactivate();
        GetComponent<StabAnimator>()?.Deactivate();
        GetComponent<WalkAnimator>()?.Deactivate();
    }



}
