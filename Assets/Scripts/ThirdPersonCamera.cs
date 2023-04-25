using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private const float MinTurnAngle = -90.0f;
    private const float MaxTurnAngle = 0.0f;

    [SerializeField] private float _targetDistance = 5;
    [SerializeField] private float _turnSpeed = 4.0f;
    [SerializeField] private GameObject _target;

    [SerializeField] private Vector3 _offset;

    private float _horizontalDelta;
    private float _verticalDelta;
    private float _xRotation;

    private void Update() =>
        ReadInput();

    private void LateUpdate()
    {
        _xRotation += _verticalDelta;
        _xRotation = Mathf.Clamp(_xRotation, MinTurnAngle, MaxTurnAngle);

        Transform cameraTransform = transform;
        cameraTransform.eulerAngles = new Vector3(-_xRotation, cameraTransform.eulerAngles.y + _horizontalDelta, 0);
        cameraTransform.position = _target.transform.position - cameraTransform.forward * _targetDistance + _offset;
    }

    private void ReadInput()
    {
        _horizontalDelta = Input.GetAxis("Mouse X") * _turnSpeed;
        _verticalDelta = Input.GetAxis("Mouse Y") * _turnSpeed;
    }
}