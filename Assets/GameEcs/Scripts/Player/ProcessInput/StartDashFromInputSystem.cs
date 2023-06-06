using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class StartDashFromInputSystem : ReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;

    public StartDashFromInputSystem(Contexts contexts) : base(contexts.input)
    {
        _contexts = contexts;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) =>
        context.CreateCollector(InputMatcher.LmbInput);

    protected override bool Filter(InputEntity entity) => entity.isLmbInput;

    protected override void Execute(List<InputEntity> entities)
    {
        var e = _contexts.game.localPlayerEntity;

        if (e.hasDashing)
        {
            Debug.Log("Already has blinking");
            return;
        }

        StartBlinking(e);
    }

    private void StartBlinking(GameEntity e)
    {
        float speed = _contexts.config.gameConfig.value.DashMoveSpeed;
        float distance = _contexts.config.gameConfig.value.DashDistance;
        float duration = distance / speed;

        // Если на сущности есть велосити, то берём его для направления. Иначе берём прямо.
        CharacterController controller = e.characterController.Value;
        Vector3 dashDirection = controller.velocity.sqrMagnitude > Mathf.Epsilon
            ? controller.velocity.normalized
            : controller.transform.forward;

        e.AddDashing(duration, dashDirection);
    }
}