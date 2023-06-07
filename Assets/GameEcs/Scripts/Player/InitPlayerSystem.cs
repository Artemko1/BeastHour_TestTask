using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class InitPlayerSystem : ReactiveSystem<GameEntity>
{
    public InitPlayerSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.Player);

    protected override bool Filter(GameEntity entity) => true;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            // Добавляю сюда все компоненты, которые есть на монобехе и могут мне понадобиться в ecs
            e.AddCharacterController(e.view.Value.GetComponent<CharacterController>());
            e.AddAnimator(e.view.Value.GetComponent<Animator>());
        }
    }
}