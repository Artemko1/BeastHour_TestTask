using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerAnimator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 4.0f;
    private Camera _camera;

    private CharacterController _characterController;
    private PlayerAnimator _playerAnimator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 movementVector = Vector3.zero;

        var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (moveInput.sqrMagnitude > Mathf.Epsilon)
        {
            movementVector = _camera.transform.TransformDirection(moveInput);
            movementVector.y = 0;
            movementVector.Normalize();

            transform.forward = movementVector;
        }

        movementVector += Physics.gravity;

        _characterController.Move(_movementSpeed * Time.deltaTime * movementVector);
        _playerAnimator.PlayMove(_characterController.velocity.magnitude);
    }

    private void Warp(Vector3 to)
    {
        _characterController.enabled = false;

        to.y += _characterController.height * 2f;
        transform.position = to;

        _characterController.enabled = true;
    }
}