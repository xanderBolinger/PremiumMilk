using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerFollower : MonoBehaviour
{
    Transform following;
    [Range(0.0f, 1.0f)]
    [SerializeField] float interested = 0.25f;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void LateUpdate()
    {
        if (following == null && NetworkClient.localPlayer != null 
            && NetworkClient.localPlayer.gameObject != null) {
            following = NetworkClient.localPlayer.gameObject.transform;
        }

        _transform.position = Vector3.MoveTowards(_transform.position, following.position, interested);
    }
}
