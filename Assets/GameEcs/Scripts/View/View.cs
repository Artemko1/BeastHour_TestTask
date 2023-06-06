using System;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IDestroyedListener
{
    protected GameEntity _linkedEntity;
    // private Vector3 _lastEntityPosition;

    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        _linkedEntity = (GameEntity)entity;
        _linkedEntity.AddDestroyedListener(this);

        Vector3 pos = _linkedEntity.position.Value;
        transform.localPosition = pos;
        // _lastEntityPosition = pos;
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
    
//     Vector3 entityPos = _linkedEntity.position.Value;
//     Vector3 position = transform.position;
//         if (entityPos != _lastEntityPosition) // Позиция сущности изменилась. Синхронизируем позицию вьюхи
//     {
//         transform.position = entityPos;
//         _lastEntityPosition = entityPos;
//     }
// else if (entityPos != position) // Чтобы в ecs всегда была актуальная позиция.
// {
//     _linkedEntity.ReplacePosition(position);
//     _lastEntityPosition = position;
// }
}