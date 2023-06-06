using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace BH.Input.UserInput
{
    public class StartDashSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public StartDashSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) =>
            context.CreateCollector(InputMatcher.LmbInput.AddedOrRemoved());

        protected override bool Filter(InputEntity entity) => true;

        protected override void Execute(List<InputEntity> entities)
        {
            var e = _contexts.game.localPlayerEntity;

            // var playerView = (PlayerView)e.view.Value;
            if (_contexts.input.isLmbInput)
            {
                if (e.hasDashing)
                {
                    Debug.Log("Already has blinking");
                    return;
                }
                StartBlinking(e);
            }
        }

        private void StartBlinking(GameEntity e)
        {
            float duration = _contexts.config.gameConfig.value.DashDuration;
            // Если на сущности есть велосити, то берём его для направления. Иначе берём прямо.
            Vector3 dashDirection;
            if (e.hasVelocity)
            {
                dashDirection = e.velocity.Value;
            }
            else
            {
                dashDirection = Vector3.forward; // bug надо брать forward актуальный (через трансформ)
            }

            e.AddDashing(duration, dashDirection);
        }
    }
}