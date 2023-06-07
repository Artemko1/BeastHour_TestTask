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
            var view = e.view.Value;
            e.AddCharacterController(view.GetComponent<CharacterController>());
            e.AddAnimator(view.GetComponent<Animator>());
            e.AddRenderer(view.GetComponentInChildren<Renderer>());
        }
    }
}