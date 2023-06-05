using UnityEngine;

public class CameraView : MonoBehaviour, IAnyLocalPlayerListener, IPositionListener
{
    [SerializeField] private Transform _target;

    private void Start()
    {
        var entity = Contexts.sharedInstance.game.CreateEntity();
        entity.AddAnyLocalPlayerListener(this);
    }

    public void OnAnyLocalPlayer(GameEntity entity)
    {
        Debug.Log("OnAnyLocalPlayer!");
        entity.AddPositionListener(this);
    }

    public void OnPosition(GameEntity entity, Vector3 value)
    {
        Debug.Log("OnAnyLocalPlayer OnPosition!");
        _target.position = value;
    }
}