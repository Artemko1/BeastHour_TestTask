using Camera;
using UnityEngine;

public class CameraView : View, IPositionListener
{
    [SerializeField] private Transform _target;
    [SerializeField] private ThirdPersonCamera _thirdPersonCamera;

    private void Start()
    {
        // Assumes that localPlayer is already created
        var localPlayerEntity = Contexts.sharedInstance.game.localPlayerEntity;
        localPlayerEntity.AddPositionListener(this);
    }

    public void OnPosition(GameEntity entity, Vector3 value)
    {
        Debug.Log("OnAnyLocalPlayer OnPosition!");
        _thirdPersonCamera.UpdateTargetPosition(value);
    }
}