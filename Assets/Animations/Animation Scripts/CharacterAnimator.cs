using System.Linq;
using UnityEngine;
using Mirror;

public class CharacterAnimator : NetworkBehaviour
{

    public Animator animator;

    public bool attackFinished;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackFinished() { 
        attackFinished = true;
    }


    [ClientRpc]
    public void RpcSwing() {
        SetSwing();
    }

    [ClientRpc]
    public void RpcStab()
    {
        SetStab();
    }

    [ClientRpc]
    public void RpcParry()
    {
        SetParry();
    }

    [ClientRpc]
    public void RpcDead()
    {
        SetDead();
    }

    [ClientRpc]
    public void RpcHit()
    {
        SetHit();
    }

    public void SetIdle() {
       if(animator == null) { return; }

       ClearAnimation();
    }

    public void SetJumping() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsJumping", true);
    }

    public void SetWalk() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsWalking", true);
    }

    public void SetRun()
    {
        SetWalk();
    }

    public void SetSwing() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsSwinging", true);
        attackFinished = false;
    }

    public void SetStab() {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsStabbing", true);
        attackFinished = false;
    }

    public void SetDead()
    {
        if (animator == null) { return; }

        ClearAnimation();
        
        animator.SetBool("IsDead", true);
    }

    public void SetHit()
    {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsHit", true);
    }

    public void SetParry()
    {
        if (animator == null) { return; }

        ClearAnimation();
        animator.SetBool("IsParrying", true);
    }

    private void ClearAnimation() {
        if (animator == null) { return; }
        animator.enabled = true;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSwinging", false);
        animator.SetBool("IsStabbing", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHit", false);
        animator.SetBool("IsParrying", false);
    }

    public void DeactivateAll() { 
        IAnimator[] animators = FindObjectsOfType<MonoBehaviour>(true).OfType<IAnimator>().ToArray();
        foreach(var animator in animators)
        {
            animator.Deactivate();
        }
    }


}
