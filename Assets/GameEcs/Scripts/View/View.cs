using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IDestroyedListener
{
    private GameEntity _linkedEntity;

    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        _linkedEntity = (GameEntity)entity;
        _linkedEntity.AddDestroyedListener(this);
    }

    protected virtual void Update()
    {
        // Чтобы в ecs всегда была актуальная позиция.
        if (_linkedEntity.position.Value != transform.position)
        {
            _linkedEntity.ReplacePosition(transform.position);
        }
    }

    //todo посмотреть можно ли как-то добавлять компонент в AddViewSystem
    protected virtual void OnTriggerEnter(Collider other) 
    {
        var otherView = other.GetComponentInParent<View>();
        if (otherView == this) return;

        var otherEntity = otherView._linkedEntity;
        
        // var e = _linkedEntity;
        // Debug.Log($"TriggerEnter FrameCount {Time.frameCount}. E has localPlayer:{e.isLocalPlayer}. Other isLocalPlayer:{otherEntity.isLocalPlayer}. Other isAiCharacter:{otherEntity.isAiCharacter}");
        if (!_linkedEntity.hasTriggerEnter)
        {
            _linkedEntity.AddTriggerEnter(otherEntity);
            // Debug.Log("Add trigger", this);
        }
        else
        {
            // Debug.LogWarning("Already has trigger");
        }
        
    }

    public virtual void OnDestroyed(GameEntity entity) => OnDestroy();

    protected virtual void OnDestroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }
}