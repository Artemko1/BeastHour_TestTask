using Entitas;
using UnityEngine;

public sealed class ReadLmbInputSystem : IExecuteSystem, ICleanupSystem
{
    private readonly Contexts _contexts;

    private readonly IGroup<InputEntity> _inputs;

    public ReadLmbInputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _inputs = contexts.input.GetGroup(InputMatcher.LmbInput);
    }
    
    public void Execute()
    {
        if (InputService.LMB)
        {
            var entity = _contexts.input.CreateEntity();
            entity.isLmbInput = true;
            // inputContext.isLmbInput = true; // В чем тут была бы разница?
        }
    }

    public void Cleanup()
    {
        foreach (var e in _inputs.GetEntities())
        {
            e.Destroy();
        }
    }
}