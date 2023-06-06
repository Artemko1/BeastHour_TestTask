using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AiRandomDashSystem : ReactiveSystem<GameEntity>
{
    // Запускает дэш через случайное время
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public AiRandomDashSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.AiCharacter);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.Timer.Removed());

    protected override bool Filter(GameEntity entity) => entity.isAiCharacter;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            StartDashing(e);
        }
    }

    private void StartDashing(GameEntity e)
    {
        float speed = _contexts.config.gameConfig.value.DashMoveSpeed;
        float distance = _contexts.config.gameConfig.value.DashDistance;
        float duration = distance / speed;

        Vector2 insideUnitCircle = Random.insideUnitCircle;

        e.AddDashing(duration, new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y).normalized);
    }
}