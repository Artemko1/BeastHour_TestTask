using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ApplyDesiredMoveSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public ApplyDesiredMoveSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.DesiredMoveDirection);
    }

    public void Execute()
    {
        foreach (GameEntity e in _group.GetEntities())
        {
            var playerView = (PlayerView)e.view.Value;
            var dir = e.desiredMoveDirection.Value;
            // if (dir == Vector2.zero)
            // {
            //     continue;
            // }

            var moveVector = new Vector3(dir.x, 0, dir.y);
            float speed = _contexts.config.gameConfig.value.PlayerSpeed;
            float deltaTime = _contexts.input.deltaTime.value;

            playerView.Move(moveVector * speed * deltaTime);
            // todo заинлайнить, сделать characterControllerComponent
            // брать transform.Position и пихать в энтити позицию
        }
    }
}