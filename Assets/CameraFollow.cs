using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform TargetToFollow;

    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _cameraVelocity;
    private Vector3 _smoothSpeed;
    private Vector3 _desiredPosition;
    private Vector3 _smoothedPosition;

    void Update()
    {
        CalculateCameraPosition();
    }

    private void CalculateCameraPosition()
    {
        _desiredPosition = TargetToFollow.position + _cameraOffset;
        _smoothedPosition = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _smoothSpeed, _cameraVelocity);
        transform.position = _smoothedPosition;
    }
}
