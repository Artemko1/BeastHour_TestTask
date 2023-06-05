using Entitas;
using UnityEngine;

public sealed class UpdateUserInputSystem : IExecuteSystem, ICleanupSystem
{
    private readonly Contexts _contexts;

    private readonly IGroup<InputEntity> _moveInputs;

    public UpdateUserInputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _moveInputs = contexts.input.GetGroup(InputMatcher.MoveInput);
    }
    
    public void Execute()
    {
        // var entity = _contexts.input.CreateEntity();
        var inputContext = _contexts.input;
        
        Vector2 moveAxis = InputService.MovementAxis;

        if (moveAxis != Vector2.zero)
        {
            // entity.AddMoveInput(moveAxis);
            inputContext.ReplaceMoveInput(moveAxis);
        }
        
        bool lmb = InputService.LMB;
        if (lmb)
        {
            // entity.isLmbInput = true;
            inputContext.isLmbInput = true;
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