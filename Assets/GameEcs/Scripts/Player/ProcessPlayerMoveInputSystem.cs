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
            context.CreateCollector(InputMatcher.MoveInput.AddedOrRemoved());

        protected override bool Filter(InputEntity entity) => true;

        protected override void Execute(List<InputEntity> entities)
        {
            var e = _contexts.game.localPlayerEntity;

            if (_contexts.input.hasMoveInput)
            {
                var moveInputComponent = _contexts.input.moveInput;
                var camera = _contexts.game.cameraEntity;

                Vector3 moveVector = TransformToCameraLocalCoordinates(camera, moveInputComponent);

                e.ReplaceDesiredMoveDirection(new Vector2(moveVector.x, moveVector.z));
            }
            else
            {
                // Лучше убирать тут (на след. кадре), но ещё можно добавить cleaup атрибут компоненту
                // и тогда его будет убирать авто-клинап-система в конце кадра
                // Разница как понимаю только в том, что в инспекторе становится видно компонент
                e.RemoveDesiredMoveDirection();
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