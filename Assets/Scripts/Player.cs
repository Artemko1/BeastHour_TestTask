using System;
using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(CharacterController))]
public class Player : NetworkBehaviour
{
    [SerializeField] private Renderer _characterRenderer;
    [SerializeField] private Material _alteredMaterial;
    [SerializeField] private float _stateChangeDuration;

    [SerializeField] private CharacterController _characterController;

    private bool _isInAlteredState;
    private Material[] _originMaterials;

    private bool CanBeHit => !_isInAlteredState;

    [field: SyncVar(hook = nameof(SyncScore))]
    public int Score { get; [Server] private set; }

    [field: SyncVar(hook = nameof(SyncName))]
    public string Name { get; [Server] set; }

    private void Awake() =>
        _originMaterials = _characterRenderer.sharedMaterials;

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
            HitTarget(target); // для локального отображения на клиенте без задержки
        }

        CmdHitTarget(target);
    }

    [Command]
    private void CmdHitTarget(Player target)
    {
        Debug.Log("CmdHitTarget");

        if (!target.CanBeHit) // server validation
        {
            return;
        }

        if (isServerOnly)
        {
            HitTarget(target); // Чтобы на сервере тоже поменялось
        }

        Score++;

        RpcHitTarget(target);
    }

    [ClientRpc]
    private void RpcHitTarget(Player target) // rpc вызывается и на хосте тоже, но не на чисто сервере
    {
        Debug.Log("RpcHitTarget");
        HitTarget(target);
    }

    private void HitTarget(Player target) =>
        target.TryTakeHit();

    private void TryTakeHit()
    {
        // Debug.Log("TryTakeHit call on target", this);
        if (!CanBeHit) return; // client validation

        ToAlteredState();

        StartCoroutine(ToNormalStateWithDelay());
    }

    private void ToAlteredState()
    {
        _isInAlteredState = true;
        Material[] materials = _characterRenderer.materials;

        for (var i = 0; i < materials.Length; i++)
        {
            materials[i] = _alteredMaterial;
        }

        _characterRenderer.materials = materials;
    }

    private IEnumerator ToNormalStateWithDelay()
    {
        yield return new WaitForSeconds(_stateChangeDuration);
        _characterRenderer.materials = _originMaterials;
        _isInAlteredState = false;
    }
}