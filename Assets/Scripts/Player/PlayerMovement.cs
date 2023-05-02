using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerAnimator))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float _movementSpeed = 4.0f;
        private UnityEngine.Camera _camera;

        private CharacterController _characterController;
        private PlayerAnimator _playerAnimator;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (!isOwned)
            {
                return;
            }

            Vector3 movementVector = Vector3.zero;

            Vector2 moveInput = InputService.MovementAxis;

            if (moveInput.sqrMagnitude > Mathf.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(moveInput);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            _characterController.Move(_movementSpeed * Time.deltaTime * movementVector);
            _playerAnimator.PlayMove(_characterController.velocity.magnitude);
        }
    }
}