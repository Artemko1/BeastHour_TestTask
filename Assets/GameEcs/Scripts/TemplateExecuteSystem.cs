using Entitas;

namespace BH.Input.UserInput
{
    public sealed class TemplateExecuteSystem : IExecuteSystem
    {
        private readonly Contexts _contexts;
        private readonly IGroup<GameEntity> _group;
    
        public TemplateExecuteSystem(Contexts contexts)
        {
            _contexts = contexts;
            _group = contexts.game.GetGroup(GameMatcher.Player);
        }
    
        public void Execute()
        {
            foreach (var e in _group.GetEntities())
            {
                
            }
        }
    }
}