using System.Collections.Generic;
using Entitas;
using UnityEngine;
using static GameMatcher;

public sealed class AddViewSystem : ReactiveSystem<GameEntity>
{
    private readonly Transform _parent;

    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {
        _parent = new GameObject("Views").transform;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(Asset); // Коллектит все с данным компонентом

    protected override bool Filter(GameEntity entity) => entity.hasAsset && !entity.hasView;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
            e.AddView(InstantiateView(e));
    }

    View InstantiateView(GameEntity entity)
    {
        var prefab = Resources.Load<GameObject>(entity.asset.Value);
        var view = Object.Instantiate(prefab, _parent).GetComponent<View>();
        view.Link(entity);
        return view;
    }
}
