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
        return entity.hasDashing && triggerEnterOther.isPlayer && !triggerEnterOther.isInvulnerableEntityLinked;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var invulDuration = _contexts.config.gameConfig.value.InvulDuration;
        foreach (GameEntity e in entities)
        {
            GameEntity otherEntity = e.triggerEnter.Other;
            Debug.Log(
                $"Hit other player while dashing! FrameCount {Time.frameCount}. E has localPlayer:{e.isLocalPlayer}. Other isLocalPlayer:{otherEntity.isLocalPlayer}. Other isAiCharacter:{otherEntity.isAiCharacter}");

            GameEntity invulnerabilityEntity = _contexts.game.CreateEntity();

            invulnerabilityEntity.AddOwner(otherEntity);
            invulnerabilityEntity.AddTimer(invulDuration);

            otherEntity.isInvulnerableEntityLinked = true;

            e.ReplaceScore(e.score.Value + 1);
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