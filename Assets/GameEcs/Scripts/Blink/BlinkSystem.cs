using Entitas;

    public sealed class BlinkSystem : IExecuteSystem
    {
        private readonly Contexts _contexts;
        private readonly IGroup<GameEntity> _players;
    
        public BlinkSystem(Contexts contexts)
        {
            _contexts = contexts;
            // _players = contexts.game.GetGroup(GameMatcher.);
        }
    
        public void Execute()
        {
            foreach (var e in _players.GetEntities())
            {
                
            }
        }
    }
