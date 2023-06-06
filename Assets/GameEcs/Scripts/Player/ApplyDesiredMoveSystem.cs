using Entitas;
using UnityEngine;

public sealed class ApplyDesiredMoveSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public ApplyDesiredMoveSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.CharacterController);
    }

    public void Execute()
    {
        foreach (GameEntity e in _group.GetEntities())
        {
            CharacterController controller = e.characterController.Value;
            if (!e.hasDesiredMoveDirection)
            {
                // Сделано, потому что controller считает velocity по разныце с последним значением
                // И его нужно задавать каждый кадр
                controller.Move(Vector3.zero);
                return;
            }

            Vector2 dir = e.desiredMoveDirection.Value;
            var moveVector = new Vector3(dir.x, 0, dir.y);

            float speed = _contexts.config.gameConfig.value.PlayerSpeed;
            float deltaTime = _contexts.input.deltaTime.value;

            Vector3 motion = moveVector * speed * deltaTime;
            
            controller.Move(motion);
            e.ReplacePosition(controller.transform.position);

            if (motion.sqrMagnitude > Mathf.Epsilon)
            {
                controller.transform.forward = motion;
            }
        }
    }
}