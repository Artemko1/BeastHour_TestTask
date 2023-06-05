using Entitas;
using UnityEngine;

public sealed class UpdateTimeSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;

    public UpdateTimeSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        //Make it bulletproof
        Execute();
    }

    public void Execute()
    {
        _contexts.input.ReplaceFixedDeltaTime(Time.fixedDeltaTime);
        _contexts.input.ReplaceDeltaTime(Time.deltaTime);
    }
}