using Camera;
using UnityEngine;

public class CameraView : View, IAnyLocalPlayerListener
{
    [SerializeField] private Transform _target;

    [SerializeField] private ThirdPersonCamera _thirdPersonCamera;

    private void Start()
    {
        // Assumes that localPlayer is already created
        var entity = Contexts.sharedInstance.game.localPlayerEntity;
        _target = entity.view.Value.transform;
        _thirdPersonCamera.SetTarget(_target);
    }

    public void OnAnyLocalPlayer(GameEntity entity)
    {
        _target = entity.view.Value.transform;
        _thirdPersonCamera.SetTarget(_target);
    }
}