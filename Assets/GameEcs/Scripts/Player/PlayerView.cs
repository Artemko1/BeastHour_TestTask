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

    protected override void Update()
    {
        base.Update();
        _playerAnimator.PlayMove(_characterController.velocity.magnitude);
        
        // Как эту логику переместить в систему?
        if (_linkedEntity.hasVelocity && _linkedEntity.velocity.Value == _characterController.velocity)
        {
            return;
        }

        if (_linkedEntity.hasVelocity &&  _characterController.velocity == Vector3.zero)
        {
            _linkedEntity.RemoveVelocity();
        }
        else if (_characterController.velocity != Vector3.zero)
        {
            _linkedEntity.ReplaceVelocity(_characterController.velocity);
        }
    }
}