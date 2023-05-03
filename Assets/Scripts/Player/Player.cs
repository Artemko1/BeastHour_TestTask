using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerView), typeof(CharacterController))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private float _stateChangeDuration;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private CharacterController _characterController;

        [SyncVar(hook = nameof(SyncIsInvulnerable))]
        private bool _isInvulnerable;

        [field: SyncVar(hook = nameof(SyncScore))]
        public int Score { get; [Server] private set; }

        [field: SyncVar(hook = nameof(SyncName))]
        public string Name { get; [Server] set; }
        
        private void SyncIsInvulnerable(bool oldValue, bool newValue)
        {
            // Debug.Log($"Invulnerable synced from {oldValue} to {newValue}. Value is {IsInvulnerable}", this);
            _isInvulnerable = newValue;
            if (newValue)
            {
                _playerView.ToAlteredState();
            }
            else
            {
                _playerView.ToNormalState();
            }
        }

        private void SyncScore(int oldValue, int newValue) =>
            OnScoreChanged?.Invoke(this);

        private void SyncName(string oldValue, string newValue) =>
            OnNameChanged?.Invoke(this);

        public event Action<Player> OnScoreChanged;
        public event Action<Player> OnNameChanged;

        [ClientRpc]
        public void SetPosition(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        [Server]
        public void ResetScore()
        {
            int prev = Score;
            Score = 0;
            if (isServerOnly)
            {
                SyncScore(prev, Score);
            }
        }

        public void Hit(Player target)
        {
            if (isClientOnly)
            {
                target.ToInvulnerable();
            }

            CmdHitTarget(target);
        }

        [Command]
        private void CmdHitTarget(Player target)
        {
            if (target._isInvulnerable) return; // server validation

            target.ToInvulnerable();
            target.ToNotInvulnerableWithDelay();
            Score++;
            if (isServerOnly) // because hooks are not called on server-only..
            {
                SyncScore(-1, Score);
            }
        }

        private void ToInvulnerable()
        {
            if (_isInvulnerable) return;

            SyncIsInvulnerable(_isInvulnerable, true);
        }

        [Server]
        private void ToNotInvulnerableWithDelay() =>
            StartCoroutine(ToNotInvulnerableRoutine());

        [Server]
        private IEnumerator ToNotInvulnerableRoutine()
        {
            yield return new WaitForSeconds(_stateChangeDuration);
            SyncIsInvulnerable(_isInvulnerable, false);
        }
    }
}