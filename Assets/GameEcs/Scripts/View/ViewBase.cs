using Entitas;
using Entitas.Unity;
using UnityEngine;

public class ViewBase : MonoBehaviour , IDestroyedListener
{
    protected GameEntity LinkedEntity;

    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        LinkedEntity = (GameEntity)entity;
        LinkedEntity.AddDestroyedListener(this);
    }
    
    public virtual void OnDestroyed(GameEntity entity) => OnDestroy();
    protected virtual void OnDestroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }
}