using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class HitOnDashTriggerSystem : ReactiveSystem<GameEntity>
{
    public HitOnDashTriggerSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.TriggerEnter);

    protected override bool Filter(GameEntity entity) => entity.hasDashing && entity.triggerEnter.Other.isPlayer;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            Debug.Log("Hit other player while dashing!");
        }
    }
}