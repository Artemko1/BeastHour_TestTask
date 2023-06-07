using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class HitOnDashTriggerSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly Contexts _contexts;

    public HitOnDashTriggerSystem(Contexts contexts) : base(contexts.game)
    {
        _group = contexts.game.GetGroup(GameMatcher.TriggerEnter);
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.TriggerEnter);

    protected override bool Filter(GameEntity entity)
    {
        GameEntity triggerEnterOther = entity.triggerEnter.Other;
        return entity.hasDashing && triggerEnterOther.isPlayer && !triggerEnterOther.hasInvulnerableEntityLink;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        for (var i = 0; i < entities.Count; i++)
        {
            GameEntity e = entities[i];
            GameEntity triggerEnterOther = e.triggerEnter.Other;
            Debug.Log($"Hit other player while dashing! FrameCount {Time.frameCount}. E has localPlayer:{e.isLocalPlayer}. Other isLocalPlayer:{triggerEnterOther.isLocalPlayer}. Other isAiCharacter:{triggerEnterOther.isAiCharacter}");

            GameEntity invulnerabilityEntity = _contexts.game.CreateEntity();

            invulnerabilityEntity.AddOwner(triggerEnterOther);
            // invulnerabilityEntity.isInvulnerable = true;
            invulnerabilityEntity.AddTimer(15f);


            triggerEnterOther.AddInvulnerableEntityLink(invulnerabilityEntity);
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