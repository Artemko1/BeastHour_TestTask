using System;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IDestroyedListener
{
    protected GameEntity _linkedEntity;

    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        _linkedEntity = (GameEntity)entity;
        _linkedEntity.AddDestroyedListener(this);

        Vector3 pos = _linkedEntity.position.Value;
        transform.localPosition = pos;
    }

    protected virtual void Update()
    {
        // Чтобы в ecs всегда была актуальная позиция.
        if (_linkedEntity.position.Value != transform.position)
        {
            _linkedEntity.ReplacePosition(transform.position);
        }
    }

    public virtual void OnDestroyed(GameEntity entity) => OnDestroy();

    protected virtual void OnDestroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }
}