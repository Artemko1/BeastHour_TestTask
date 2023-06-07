using Entitas;
using UnityEngine;

namespace GameEcs.Scripts.Player
{
    public class AnimatePlayerMoveSystem : IExecuteSystem
    {
        private readonly int _animIDSpeed = Animator.StringToHash("Speed"); //todo переместить из системы
        
        private readonly Contexts _contexts;
        private readonly IGroup<GameEntity> _group;

        public AnimatePlayerMoveSystem(Contexts contexts)
        {
            _contexts = contexts;
            _group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Animator, GameMatcher.CharacterController));
        }

        public void Execute()
        {
            var deltaTime = _contexts.input.deltaTime.value;
            foreach (GameEntity e in _group.GetEntities())
            {
                Animator animator = e.animator.Value;
                CharacterController controller = e.characterController.Value;
                
                float speed = controller.velocity.magnitude;
                // Debug.Log($"Magnitude is {speed}");
                animator.SetFloat(_animIDSpeed, speed, 0.1f, deltaTime);
            }
        }
    }
}