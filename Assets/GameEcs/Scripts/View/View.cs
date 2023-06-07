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

    protected virtual void OnTriggerEnter(Collider other)
    {
        var otherView = other.GetComponentInParent<View>();
        if (otherView == this) return;

        var otherEntity = otherView._linkedEntity;

        if (!_linkedEntity.hasTriggerEnter)
        {
            _linkedEntity.AddTriggerEnter(otherEntity);
        }
    }

    public virtual void OnDestroyed(GameEntity entity) => OnDestroy();

    protected virtual void OnDestroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }
}