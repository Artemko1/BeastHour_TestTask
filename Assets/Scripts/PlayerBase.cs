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

    private void Awake() =>
        _originMaterials = _characterRenderer.sharedMaterials;

    public void Hit(PlayerBase target)
    {
        if (isServer)
        {
            RPCHitTarget(target);
        }
        else
        {
            HitTarget(target); // для локального отображения без задержки
            CmdHitTarget(target);
        }
    }

    [Command]
    private void CmdHitTarget(PlayerBase target) =>
        RPCHitTarget(target);

    [ClientRpc]
    private void RPCHitTarget(PlayerBase target) // rpc вызывается и на сервере тоже
        => HitTarget(target);

    private void HitTarget(PlayerBase target) =>
        target.TryTakeHit();

    //todo добавить валидацию на сервере, что если цель неуязвима, то не надо эту функцию рассылать
    private void TryTakeHit()
    {
        if (_isInAlteredState) return;

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