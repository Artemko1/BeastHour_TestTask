using Entitas;
using UnityEngine;

public sealed class ReadWasdInputSystem : IExecuteSystem, ICleanupSystem
{
    private readonly Contexts _contexts;

    private readonly IGroup<InputEntity> _moveInputs;

    public ReadWasdInputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _moveInputs = contexts.input.GetGroup(InputMatcher.MoveInput);
    }
    
    public void Execute()
    {
        Vector2 moveAxis = InputService.MovementAxis;

        if (moveAxis != Vector2.zero)
        {
            var entity = _contexts.input.CreateEntity();
            entity.AddMoveInput(moveAxis);
            // inputContext.ReplaceMoveInput(moveAxis);
        }
    }

    public void Cleanup()
    {
        foreach (var e in _moveInputs.GetEntities())
        {
            e.Destroy();
        }
    }
}