using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ApplyDesiredMoveSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ApplyDesiredMoveSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.DesiredMoveDirection.AddedOrRemoved());

    protected override bool Filter(GameEntity entity) => true;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            var playerView = (PlayerView)e.view.Value;
            if (e.hasDesiredMoveDirection)
            {
                DesiredMoveDirectionComponent dir = e.desiredMoveDirection;

                var moveVector = new Vector3(dir.Value.x, 0, dir.Value.y);
                float speed = _contexts.config.gameConfig.value.PlayerSpeed;
                float deltaTime = _contexts.input.deltaTime.value;

                playerView.Move(moveVector * speed * deltaTime);
            }
            else
            {
                playerView.Move(new Vector3());
            }
        }
    }
}