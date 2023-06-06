using Entitas;
using UnityEngine;

public sealed class DashSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public DashSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Dashing, GameMatcher.Position)); //вместо pos будет charContr
    }

    public void Execute()
    {
        foreach (var e in _group.GetEntities())
        {
            var playerView = (PlayerView)e.view.Value;

            if (!playerView)
            {
                continue;
            }

            var deltaTime = _contexts.input.deltaTime.value;
            DashingComponent dashing = e.dashing;
            Vector3 deltaPos = dashing.Direction * deltaTime * _contexts.config.gameConfig.value.DashMoveSpeed;
            playerView.Move(deltaPos); // todo будет characterController.Value.Move
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