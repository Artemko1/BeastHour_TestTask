using System.Collections;
using Mirror;
using UnityEngine;

public class Dash : NetworkBehaviour
{
    [SerializeField] private float _distance = 4f;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerBase _playerBase;
    [SerializeField] private LayerMask _blinkHitLayerMask;

    [SerializeField] private TriggerObserver _dashCollideObserver;


    private bool _isBlinking;

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
        // Debug.Log("Enter!", this);

        // if(!_isBlinking) return;
        if (_isBlinking && other.TryGetComponent(out PlayerBase playerBase))
        {
            if (_playerBase.Equals(playerBase)) return;

            _playerBase.Hit(playerBase);
        }
    }

    private void StartBlinkCoroutine() =>
        StartCoroutine(DoBlink());

    private IEnumerator DoBlink()
    {
        _isBlinking = true;
        Debug.Log("Set blinking to true!", this);
        Transform tr = transform;

        Vector3 direction = tr.forward;

        for (float i = 0; i < _duration; i += Time.fixedDeltaTime)
        {
            _characterController.Move(Time.fixedDeltaTime * (_distance / _duration) * direction);
            yield return new WaitForFixedUpdate();
        }

        _isBlinking = false;

        //
        // Vector3 startBlinkPos = tr.position + Vector3.up;
        // _characterController.Move(direction * _distance);
        //
        // Vector3 endBlinkPos = tr.position + Vector3.up;
        //
        // if (!Physics.Linecast(startBlinkPos, endBlinkPos, out RaycastHit hit, _blinkHitLayerMask)) return;
        //
        // Debug.Log("Intersected player!");
        //     
        // if (hit.transform.TryGetComponent(out PlayerBase playerBase))
        // {
        //     _playerBase.Hit(playerBase);
        // }

        // Debug.DrawLine(startBlinkPos, endBlinkPos, Color.red, 1.5f);
        // Debug.Log($"Pos diff: {endBlinkPos - startBlinkPos}");
    }
}