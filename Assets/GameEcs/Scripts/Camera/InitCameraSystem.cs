using System.Collections.Generic;
using Camera;
using Entitas;

public class InitCameraSystem : ReactiveSystem<GameEntity>
{
    public InitCameraSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.Camera);

    protected override bool Filter(GameEntity entity) => true;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            // Добавляю сюда все компоненты, которые есть на монобехе и могут мне понадобиться в ecs
            e.AddThirdPersonCamera(e.view.Value.GetComponent<ThirdPersonCamera>());
        }
    }
}