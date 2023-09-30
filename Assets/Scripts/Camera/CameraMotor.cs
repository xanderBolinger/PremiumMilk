using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : Follower
{

    [SerializeField]
    private float _mouseSensitivity = 3.0f;
    [SerializeField]
    private float _scrollSpeed = 20.0f;

    private float _rotationY;
    private float _rotationX;
    private float _nextDistanceFromTarget;

    [SerializeField]
    private float _distanceFromTarget = 10.0f;

    [SerializeField]
    private float _minDistanceFromTarget = 5.0f;
    [SerializeField]
    private float _maxDistanceFromTarget = 5.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    bool cursor;

    public override void Awake()
    {
        base.Awake();
        _nextDistanceFromTarget = _distanceFromTarget;
    }

    void LateUpdate()
    {
        if (!Input.GetKey(KeyCode.LeftControl))
            cursor = true;
        else 
            cursor = false;

        SetCursor();

        if (target == null)
            return;

        UpdateRotation();

        UpdateDistanceToTarget();
    }


    void UpdateRotation() {


        float mouseX = !cursor ? Input.GetAxis("Mouse X") * _mouseSensitivity : 0f;
        float mouseY = !cursor ? Input.GetAxis("Mouse Y") * _mouseSensitivity : 0f;

        _rotationY += mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        _transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        _transform.position = target.position - _transform.forward * _distanceFromTarget;
    }

    void SetCursor() {
        if (cursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void UpdateDistanceToTarget()
    {
        _nextDistanceFromTarget += !cursor ? -Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed : 0;
        if (_nextDistanceFromTarget < _minDistanceFromTarget)
            _nextDistanceFromTarget = _minDistanceFromTarget;
        else if (_nextDistanceFromTarget > _maxDistanceFromTarget)
            _nextDistanceFromTarget = _maxDistanceFromTarget;

        _distanceFromTarget = Mathf.Lerp(_distanceFromTarget,
            _nextDistanceFromTarget, 1 * Time.deltaTime);
    }

}
