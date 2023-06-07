using UnityEngine;

public class View : ViewBase
{
    protected virtual void Update()
    {
        // Чтобы в ecs всегда была актуальная позиция.
        if (LinkedEntity.position.Value != transform.position)
        {
            LinkedEntity.ReplacePosition(transform.position);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var otherView = other.GetComponentInParent<View>();
        if (otherView == this) return;

        var otherEntity = otherView.LinkedEntity;

        if (!LinkedEntity.hasTriggerEnter)
        {
            LinkedEntity.AddTriggerEnter(otherEntity);
        }
    }
}