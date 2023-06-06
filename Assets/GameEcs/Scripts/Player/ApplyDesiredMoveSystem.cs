using Entitas;
using UnityEngine;

public sealed class ApplyDesiredMoveSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public ApplyDesiredMoveSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.DesiredMoveDirection, GameMatcher.CharacterController));
    }

    public void Execute()
    {
        foreach (GameEntity e in _group.GetEntities())
        {
            var dir = e.desiredMoveDirection.Value;

            var moveVector = new Vector3(dir.x, 0, dir.y);
            float speed = _contexts.config.gameConfig.value.PlayerSpeed;
            float deltaTime = _contexts.input.deltaTime.value;

            Vector3 motion = moveVector * speed * deltaTime;
            CharacterController controller = e.characterController.Value;
            controller.Move(motion);
            e.ReplacePosition(controller.transform.position);

            if (motion.sqrMagnitude > Mathf.Epsilon)
            {
                controller.transform.forward = motion;
            }
        }
    }
}