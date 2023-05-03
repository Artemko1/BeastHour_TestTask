using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Renderer), typeof(CharacterController))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private Renderer _characterRenderer;
        [SerializeField] private Material _alteredMaterial;
        [SerializeField] private float _stateChangeDuration;

        [SerializeField] private CharacterController _characterController;

        private Material[] _originMaterials;


        [field: SyncVar(hook = nameof(SyncIsInvulnerable))]
        public bool IsInvulnerable { get; [Server] private set; }

        [field: SyncVar(hook = nameof(SyncScore))]
        public int Score { get; [Server] private set; }

        [field: SyncVar(hook = nameof(SyncName))]
        public string Name { get; [Server] set; }

        private void Awake() =>
            _originMaterials = _characterRenderer.sharedMaterials;

        private void SyncIsInvulnerable(bool oldValue, bool newValue)
        {
            Debug.Log($"Invulnerable synced from {oldValue} to {newValue}.", this);
            if (newValue)
            {
                ToAlteredState();
            }
            else
            {
                ToNormalState();
            }
        }

        private void SyncScore(int oldValue, int newValue) =>
            // Debug.Log($"Score changed from {oldValue} to {newValue}. Score is {Score}", this);
            OnScoreChanged?.Invoke(this);

        private void SyncName(string oldValue, string newValue)
        {
            Debug.Log($"Name synced from {oldValue} to {newValue}.", this);
            OnNameChanged?.Invoke(this);
        }

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
        public void ResetScore() =>
            Score = 0;

        public void Hit(Player target)
        {
            if (isClientOnly)
            {
                // HitTarget(target); // для локального отображения на клиенте без задержки
                target.IsInvulnerable = true;
                target.SyncIsInvulnerable(false, true);
            }

            CmdHitTarget(target);
        }

        [Command]
        private void CmdHitTarget(Player target)
        {
            if (target.IsInvulnerable) // server validation
            {
                return;
            }

            target.ToInvulnerable();
            Score++;
        }

        [Server]
        private void ToInvulnerable()
        {
            IsInvulnerable = true;
            if (isServerOnly)
            {
                SyncIsInvulnerable(false, true);
            }

            StartCoroutine(ToNotInvulnerableRoutine());
        }

        [Server]
        private IEnumerator ToNotInvulnerableRoutine()
        {
            yield return new WaitForSeconds(_stateChangeDuration);
            IsInvulnerable = false;
            if (isServerOnly)
            {
                SyncIsInvulnerable(true, false);
            }
        }

        private void ToAlteredState()
        {
            Material[] materials = _characterRenderer.materials;

            for (var i = 0; i < materials.Length; i++)
            {
                materials[i] = _alteredMaterial;
            }

            _characterRenderer.materials = materials;
        }

        private void ToNormalState() =>
            _characterRenderer.materials = _originMaterials;
    }
}