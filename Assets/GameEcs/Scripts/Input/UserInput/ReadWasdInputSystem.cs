using Entitas;
using UnityEngine;

public sealed class ReadWasdInputSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;

    public ReadWasdInputSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        _contexts.input.SetMoveInput(new Vector2());
    }

    public void Execute()
    {
        Vector2 moveAxis = InputService.MovementAxis;

        if (moveAxis != _contexts.input.moveInput.Value)
        {
            _contexts.input.ReplaceMoveInput(moveAxis);
        }
    }
}