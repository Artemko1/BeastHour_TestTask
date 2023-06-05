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
            context.CreateCollector(InputMatcher.MoveInput.AddedOrRemoved());

        protected override bool Filter(InputEntity entity) => true;

        protected override void Execute(List<InputEntity> entities)
        {
            var e = _contexts.game.localPlayerEntity;

            var playerView = (PlayerView)e.view.Value;
            if (_contexts.input.hasMoveInput)
            {
                var moveInputComponent = _contexts.input.moveInput;
                var camera = _contexts.game.cameraEntity;

                Vector3 moveVector = TransformToCameraLocalCoordinates(camera, moveInputComponent);

                float speed = _contexts.config.gameConfig.value.PlayerSpeed;
                float deltaTime = _contexts.input.deltaTime.value;
                
                playerView.Move(moveVector * speed * deltaTime);
            }
            else
            {
                playerView.Move(new Vector3());
            }
        }

        private static Vector3 TransformToCameraLocalCoordinates(GameEntity camera, MoveInputComponent moveInputComponent)
        {
            Vector3 moveVector = camera.view.Value.transform.TransformDirection(moveInputComponent.Value);
            moveVector.y = 0;
            moveVector.Normalize();
            return moveVector;
        }
    }
}