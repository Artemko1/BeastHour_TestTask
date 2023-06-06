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
            // Загрузку сцены лучше сделать отдельной системой. 
            // Можно добавить тип сцену - бутстрап, игровая и тд.
            // Реактивная система, инициализирующая матч, должна поднянуться по загрузке игровой сцены

            CreatePlayer();
            CreateCamera();
        }

        private void CreatePlayer()
        {
            Vector2 position = _contexts.config.gameConfig.value.PlayerStartPosition;

            var e = _contexts.game.CreateEntity();

            e.isPlayer = true;
            e.isLocalPlayer = true;
            e.AddAsset("Player");
            e.AddPosition(position);
        }

        private void CreateCamera()
        {
            var e = _contexts.game.CreateEntity();

            e.isCamera = true;
            e.AddAsset("MainCamera");
            e.AddPosition(new Vector3());
            // todo добавить AddCameraTarget(playerEntity)
        }
    }
}