using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class HitOnDashTriggerSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly IGroup<GameEntity> _group;

    public HitOnDashTriggerSystem(Contexts contexts) : base(contexts.game)
    {
        _group = contexts.game.GetGroup(GameMatcher.TriggerEnter);
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

    public void Cleanup()
    {
        foreach (var e in _group.GetEntities())
        {
            e.RemoveTriggerEnter();
        }
    }
}