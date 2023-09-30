using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public Transform target;

    public virtual void Awake()
    {
        _transform = transform;

    }

    public void SetFollowing(Transform transform) {
        target = transform;
    }
}
