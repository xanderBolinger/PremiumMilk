using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    CharacterAnimator characterAnimator;
    Rigidbody rb; 

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    void Update() {

        if (rb.velocity.magnitude > 0) {
            characterAnimator.SetWalk();
        } else { 
            characterAnimator.SetIdle();
        }

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);

        transform.Rotate(0, rotation, 0);
    }
}
