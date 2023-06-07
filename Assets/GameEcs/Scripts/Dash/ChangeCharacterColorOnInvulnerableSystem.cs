using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ChangeCharacterColorOnInvulnerableSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ChangeCharacterColorOnInvulnerableSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.InvulnerableEntityLinked.AddedOrRemoved());

    protected override bool Filter(GameEntity entity) => entity.hasRenderer;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            Renderer rend = e.renderer.Value;
            if (e.isInvulnerableEntityLinked)
            {
                // turn red
                Material alteredMaterial = _contexts.config.gameConfig.value.CharacterAlteredMaterial;
                Material[] materials = rend.materials;

                for (var i = 0; i < materials.Length; i++)
                {
                    materials[i] = alteredMaterial;
                }

                rend.materials = materials;
            }
            else
            {
                // turn normal
                Material[] originMaterials = _contexts.config.gameConfig.value.CharacterOriginMaterials;
                rend.materials = originMaterials;
            }
        }
    }
}