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

        var pos = _linkedEntity.position.Value;
        transform.localPosition = new Vector3(pos.x, pos.y);
    }

    private void Update()
    {
        // Чтобы в ecs всегда была актуальная позиция. 
        _linkedEntity.ReplacePosition(transform.position);
    }

    public virtual void OnDestroyed(GameEntity entity) => OnDestroy();

    protected virtual void OnDestroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }
}
