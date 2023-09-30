using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField] float speed;

    Transform targetTransform;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void FixedUpdate()
    {
        if (targetTransform == null)
            return;


        var direction = (targetTransform.position - _transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
        _transform.position = Vector3.MoveTowards(_transform.position, targetTransform.position, Time.fixedDeltaTime * speed);
    }


    public void Launch(GameObject target) {
        targetTransform = target.transform.Find("MagicTargets").Find("SeekerTarget").transform;

    }


    private void OnCollisionEnter(Collision collision)
    {
        

        Destroy(gameObject);
    }

}
