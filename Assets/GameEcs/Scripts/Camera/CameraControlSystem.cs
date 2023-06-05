using Entitas;

namespace BH.Player.Camera
{
    public class CameraControlSystem : IExecuteSystem
    {
        private readonly Contexts _contexts;
    
        public CameraControlSystem(Contexts contexts)
        {
            _contexts = contexts;
        }
        public void Execute()
        {
            var cameraEntity = _contexts.game.cameraEntity;
            var playerEntity = _contexts.game.localPlayerEntity;

            // cameraEntity.ReplacePosition(playerEntity.position.Value);
        }
    }
}