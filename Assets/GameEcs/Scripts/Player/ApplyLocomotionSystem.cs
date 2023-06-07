using Entitas;
using UnityEngine;

public sealed class ApplyLocomotionSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public ApplyLocomotionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.CharacterController);
    }

    public void Execute()
    {
        foreach (GameEntity e in _group.GetEntities())
        {
            CharacterController controller = e.characterController.Value;

            if (!e.hasFrameLocomotion)
            {
                controller.Move(Vector3.zero);
                continue;
            }
            
            Vector3 motion = e.frameLocomotion.Value;

            // Debug.Log($"Desired move is {motion}");
            controller.Move(motion);

            if (motion.sqrMagnitude > Mathf.Epsilon)
            {
                e.ReplacePosition(controller.transform.position);
                controller.transform.forward = motion;
            }
        }
    }
}