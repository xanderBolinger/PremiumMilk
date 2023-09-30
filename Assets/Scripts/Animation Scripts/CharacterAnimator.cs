using System.Linq;
using UnityEngine;
using Mirror;
using System.Collections;

public class CharacterAnimator : NetworkBehaviour
{

    public Animator animator;

    public bool attackFinished;

    bool attacking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackFinished() { 
        attackFinished = true;
        attacking = false;
    }

    [ClientRpc]
    public void RpcAttack(bool swing, string defender) {

        attacking = true;

        RotateTowardsTarget(CharacterController.GetCharacterObject(defender));

        if (swing)
            SetSwing();
        else
            SetStab();

    }

    void RotateTowardsTarget(GameObject target)
    {
        var direction = (target.transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
    }

    [ClientRpc]
    public void RpcParry()
    {
        SetParry();
    }

    [ClientRpc]
    public void RpcDead(string attackerName)
    {
        StartCoroutine(CoroutineDead(attackerName));
    }

    [ClientRpc]
    public void RpcHit(string attackerName)
    {

        StartCoroutine(CoroutineHit(attackerName));

    }

    IEnumerator CoroutineHit(string attackerName) {

        yield return AttackerFinished(attackerName);

        SetHit();

    }

    IEnumerator CoroutineDead(string attackerName) {

        yield return AttackerFinished(attackerName);
        
        SetDead();

    }

    IEnumerator AttackerFinished(string attackerName) {
        var obj = CharacterController.GetCharacterObject(attackerName);
        var anim = obj.GetComponent<CharacterAnimator>();


        yield return new WaitUntil(() => anim.attackFinished);
    }

    public void SetIdle() {
       if(animator == null || attacking) { return; }

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
        animator.Play("hit");
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