using System.Collections.Generic;
using Entitas;

public sealed class InitViewPositionSystem : ReactiveSystem<GameEntity>
{
    public InitViewPositionSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.View);

    protected override bool Filter(GameEntity entity) => entity.hasPosition;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.view.Value.transform.position = e.position.Value;
        }
    }
}