using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The anchor the camera attach to")]
    private Transform _cameraAnchor;

    [SerializeField]
    [Tooltip("The time it takes for the camera to reach the anchor position")]
    private float _translateDuration;
    [SerializeField]
    [Tooltip("The time it takes for the camera to reach the anchor rotation")]
    private float _rotationDuration;
    private Vector3 _translationSmoothDampVelocity = Vector3.zero;
    private float _xRotationSmoothDampVelocity = 0;
    private float _yRotationSmoothDampVelocity = 0;

    public bool IsTracking;

    private void Start()
    {
        transform.position = _cameraAnchor.position;
        transform.rotation = _cameraAnchor.rotation;
    }

    private void Update()
    {
        if (_cameraAnchor && IsTracking)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _cameraAnchor.position, ref _translationSmoothDampVelocity, _translateDuration);

            float xRotation = Mathf.SmoothDamp(transform.eulerAngles.x,_cameraAnchor.rotation.eulerAngles.x,ref _xRotationSmoothDampVelocity,_rotationDuration);
            float yRotation = Mathf.SmoothDamp(transform.eulerAngles.y, _cameraAnchor.rotation.eulerAngles.y, ref _yRotationSmoothDampVelocity, _rotationDuration);
            transform.rotation = Quaternion.Euler(new Vector3(xRotation, yRotation));
        } 
    }
}
