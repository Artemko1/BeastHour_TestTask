using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace BH.Input.UserInput
{
    public class ProcessPlayerMoveInputSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ProcessPlayerMoveInputSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) =>
            context.CreateCollector(InputMatcher.MoveInput);

        protected override bool Filter(InputEntity entity) => true;

        protected override void Execute(List<InputEntity> entities)
        {
            var e = _contexts.game.localPlayerEntity;

            var moveInputComponent = _contexts.input.moveInput;
            var camera = _contexts.game.cameraEntity;

            Vector3 moveVector = TransformToCameraLocalCoordinates(camera, moveInputComponent);

            e.ReplaceDesiredMoveDirection(new Vector2(moveVector.x, moveVector.z));
        }

        private static Vector3 TransformToCameraLocalCoordinates(GameEntity camera,
            MoveInputComponent moveInputComponent)
        {
            Vector3 moveVector = camera.view.Value.transform.TransformDirection(moveInputComponent.Value);
            moveVector.y = 0;
            moveVector.Normalize();
            return moveVector;
        }
    }
}