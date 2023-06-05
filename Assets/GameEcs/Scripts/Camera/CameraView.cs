using Camera;
using UnityEngine;

public class CameraView : View, IAnyLocalPlayerListener, IPositionListener
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
        Debug.Log("OnAnyLocalPlayer!");
        // entity.AddPositionListener(this);
        _target = entity.view.Value.transform;
        _thirdPersonCamera.SetTarget(_target);
    }

    public void OnPosition(GameEntity entity, Vector3 value)
    {
        Debug.Log("OnAnyLocalPlayer OnPosition!");
        // _target.position = value;
    }
}