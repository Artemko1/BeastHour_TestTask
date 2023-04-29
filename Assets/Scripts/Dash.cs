using System.Collections;
using Mirror;
using UnityEngine;

public class Dash : NetworkBehaviour
{
    [SerializeField] private float _distance = 4f;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerBase _playerBase;
    [SerializeField] private TriggerObserver _dashCollideObserver;

    private bool _isDashing;

    private void Update()
    {
        if (InputService.LMB && isOwned)
        {
            StartBlinkCoroutine();
        }
    }

    private void OnEnable() =>
        _dashCollideObserver.TriggerEnter += TriggerEnter;

    private void OnDisable() =>
        _dashCollideObserver.TriggerEnter -= TriggerEnter;

    private void TriggerEnter(Collider other)
    {
        if (!_isDashing) return;
        if (!other.TryGetComponent(out PlayerBase playerBase)) return;
        if (_playerBase.Equals(playerBase)) return;

        _playerBase.Hit(playerBase);
    }

    private void StartBlinkCoroutine() =>
        StartCoroutine(DoBlink());

    private IEnumerator DoBlink()
    {
        _isDashing = true;
        // Debug.Log("Set blinking to true!", this);
        Transform tr = transform;

        Vector3 direction = tr.forward;

        for (float i = 0; i < _duration; i += Time.fixedDeltaTime)
        {
            _characterController.Move(Time.fixedDeltaTime * (_distance / _duration) * direction);
            yield return new WaitForFixedUpdate();
        }

        _isDashing = false;
    }
}