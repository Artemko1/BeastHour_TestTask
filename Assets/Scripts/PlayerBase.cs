using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PlayerBase : NetworkBehaviour
{
    [SerializeField] private Renderer _characterRenderer;
    [SerializeField] private Material _alteredMaterial;
    [SerializeField] private float _stateChangeDuration;

    private bool _isInAlteredState;
    private Material[] _originMaterials;
    private bool CanBeHit => !_isInAlteredState;

    private void Awake() =>
        _originMaterials = _characterRenderer.sharedMaterials;

    public void Hit(PlayerBase target)
    {
        if (isClientOnly)
        {
            HitTarget(target); // для локального отображения на клиенте без задержки
        }

        CmdHitTarget(target);
    }

    [Command]
    private void CmdHitTarget(PlayerBase target)
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

        RPCHitTarget(target);
    }

    [ClientRpc]
    private void RPCHitTarget(PlayerBase target) // rpc вызывается и на хосте тоже, но не на чисто сервере
    {
        Debug.Log("RPCHitTarget");
        HitTarget(target);
    }

    private void HitTarget(PlayerBase target) =>
        target.TryTakeHit();

    private void TryTakeHit()
    {
        Debug.Log("TryTakeHit call on target", this);
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