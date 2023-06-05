using Player;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerAnimator))]
public class PlayerView : View
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerAnimator _playerAnimator;

    public void Move(Vector3 moveVector)
    {
        _characterController.Move(moveVector);
        if (moveVector.sqrMagnitude > Mathf.Epsilon)
        {
            transform.forward = moveVector;    
        }
    }

    private void Update()
    {
        _playerAnimator.PlayMove(_characterController.velocity.magnitude);
    }
}