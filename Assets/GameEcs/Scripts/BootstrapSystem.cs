using Entitas;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH.Player
{
    public sealed class BootstrapSystem : IInitializeSystem
    {
        private const string SceneName = "GameScene";
        private readonly Contexts _contexts;

        public BootstrapSystem(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
            SceneManager.LoadScene(SceneName);

            CreatePlayer();
            CreateCamera();
        }

        private void CreatePlayer()
        {
            // var id = _services.IdService.GetNext();
            Vector2 position = _contexts.config.gameConfig.value.PlayerStartPosition;

            var e = _contexts.game.CreateEntity();

            e.isPlayer = true;
            e.isLocalPlayer = true;
            // e.AddId((int)id);
            e.AddAsset("Player");
            e.AddPosition(position);
        }

        private void CreateCamera()
        {
            var e = _contexts.game.CreateEntity();

            e.isCamera = true;
            e.AddAsset("MainCamera");
            e.AddPosition(new Vector3());
        }
    }
}