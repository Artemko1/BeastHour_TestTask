using Entitas;
using UnityEngine;

public sealed class ApplyDesiredMoveSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public ApplyDesiredMoveSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.CharacterController, GameMatcher.DesiredMoveDirection));
    }

    public void Execute()
    {
        foreach (GameEntity e in _group.GetEntities())
        {
            Vector2 dir = e.desiredMoveDirection.Value;
            var moveVector = new Vector3(dir.x, 0, dir.y);
            float speed = _contexts.config.gameConfig.value.PlayerSpeed;
            float deltaTime = _contexts.input.deltaTime.value;

            Vector3 motion = moveVector * speed * deltaTime;
            
            if (motion == Vector3.zero) continue;
            
            Vector3 newValue = e.hasFrameLocomotion
                ? e.frameLocomotion.Value + motion
                : motion;
            e.ReplaceFrameLocomotion(newValue);
        }
    }
}