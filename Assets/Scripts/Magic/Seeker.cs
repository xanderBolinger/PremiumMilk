using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Seeker : NetworkBehaviour
{
    [SerializeField] float speed;

    Transform targetTransform;

    Transform _transform;

    GameObject casterObj;

    string targetName;
    string casterName;

    private void Awake()
    {
        _transform = transform;
    }

    void FixedUpdate()
    {
        if (targetTransform == null)
            return;


        var direction = (targetTransform.position - _transform.position).normalized;

        if (direction == Vector3.zero)
            return;

        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
        _transform.position = Vector3.MoveTowards(_transform.position, targetTransform.position, Time.fixedDeltaTime * speed);
    }


    public void Launch(GameObject target, string casterName, string targetName) {
        casterObj = CharacterController.GetCharacterObject(casterName);
        targetTransform = target.transform.Find("MagicTargets").Find("SeekerTarget").transform;
        this.casterName = casterName;
        this.targetName = targetName;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject == casterObj) {
            //Debug.LogError("Spell collision hit same game object as caster.");
            return; 
        }

        if (isServer) {
            MagicManager.magicManager.magicDamage.ApplyPhysicalDamage("Magic Missile", 200, casterName, targetName);
        }


        Destroy(gameObject);
    }


}
