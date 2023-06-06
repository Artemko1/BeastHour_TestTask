using System.Collections.Generic;
using Camera;
using Entitas;
using UnityEngine;

namespace GameEcs.Scripts.Camera
{
    public class UpdateCameraTargetPositionSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        public UpdateCameraTargetPositionSystem(Contexts contexts) : base(contexts.game)
        {
            _contexts = contexts;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.AllOf(GameMatcher.CameraTarget, GameMatcher.Position));

        protected override bool Filter(GameEntity entity) => true;

        protected override void Execute(List<GameEntity> entities)
        {
            var camera = _contexts.game.cameraEntity;
            ThirdPersonCamera thirdPersonCamera = camera.thirdPersonCamera.Value;

            var target = entities[0];
            Vector3 pos = target.position.Value;
            
            thirdPersonCamera.UpdateTargetPosition(pos);
        }
    }
}