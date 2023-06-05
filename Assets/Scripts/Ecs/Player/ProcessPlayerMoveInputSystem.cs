using System.Collections.Generic;
using Entitas;

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
            context.CreateCollector(InputMatcher.MoveInput.Added());
        
        protected override bool Filter(InputEntity entity) => entity.hasMoveInput;

        protected override void Execute(List<InputEntity> entities)
        {
            var e = _contexts.game.localPlayerEntity;
            
            var moveInput =  _contexts.input.moveInput;
            
            // Можно ли как-то кешировать GetComponent?
            var playerView = (PlayerView)e.view.Value;
            playerView.Move(moveInput.Value);
        }
    }
}