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

            var playerView = (PlayerView)e.view.Value;
            if (_contexts.input.hasMoveInput)
            {
                var moveInputComponent = _contexts.input.moveInput;
                float speed = _contexts.config.gameConfig.value.PlayerSpeed;
                float deltaTime = _contexts.input.deltaTime.value;
                playerView.Move(moveInputComponent.Value * speed * deltaTime);
            }
            else
            {
                playerView.Move(new Vector3());
            }
            
            // if (moveInput.sqrMagnitude > Mathf.Epsilon)
            // {
            //     movementVector = _camera.transform.TransformDirection(moveInput);
            //     movementVector.y = 0;
            //     movementVector.Normalize();
            //
            //     transform.forward = movementVector;
            // }
        }
    }
    
    
    
    // public sealed class AnimateCharactersSystem : IExecuteSystem
    // {
    //     private readonly Contexts _contexts;
    //     private readonly IGroup<GameEntity> _players;
    //
    //     public AnimateCharactersSystem(Contexts contexts)
    //     {
    //         _contexts = contexts;
    //         _players = contexts.game.GetGroup(GameMatcher.Player);
    //     }
    //
    //     public void Execute()
    //     {
    //         foreach (var e in _players.GetEntities())
    //         {
    //             e.view
    //         }
    //     }
    // }
}