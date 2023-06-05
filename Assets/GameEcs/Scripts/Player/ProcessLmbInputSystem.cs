using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace BH.Input.UserInput
{
    public class ProcessLmbInputSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ProcessLmbInputSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) =>
            context.CreateCollector(InputMatcher.LmbInput.AddedOrRemoved());

        protected override bool Filter(InputEntity entity) => true;

        protected override void Execute(List<InputEntity> entities)
        {
            var e = _contexts.game.localPlayerEntity;

            var playerView = (PlayerView)e.view.Value;
            if (_contexts.input.isLmbInput)
            {
                StartBlinking(e);
            }
        }

        private void StartBlinking(GameEntity e)
        {
            float duration = _contexts.config.gameConfig.value.BlinkDuration;
            // Если на сущности есть велосити, то берём его для направления. Иначе берём прямо.
            // if (e.)
            // {
            //     
            // }
            // e.AddBlinking(duration, );
        }
    }
}