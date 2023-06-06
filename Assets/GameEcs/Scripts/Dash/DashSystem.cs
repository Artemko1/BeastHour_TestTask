using Entitas;
using UnityEngine;

public sealed class DashSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public DashSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Dashing, GameMatcher.CharacterController));
    }

    public void Execute()
    {
        foreach (var e in _group.GetEntities())
        {
            var deltaTime = _contexts.input.deltaTime.value;
            DashingComponent dashing = e.dashing;
            Vector3 deltaPos = deltaTime * _contexts.config.gameConfig.value.DashMoveSpeed * dashing.Direction;
            
            CharacterController controller = e.characterController.Value;
            controller.Move(deltaPos);
            e.ReplacePosition(controller.transform.position);
            if (deltaPos.sqrMagnitude > Mathf.Epsilon)
            {
                controller.transform.forward = deltaPos;
            }
            
            dashing.RemainingTime -= deltaTime;
            if (dashing.RemainingTime <= 0)
            {
                e.RemoveDashing();
            }
            else
            {
                e.ReplaceComponent(GameComponentsLookup.Dashing, dashing);
            }
        }
    }
}