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
            // todo Загрузку сцены лучше сделать отдельной системой. 
            // Можно добавить тип сцену - бутстрап, игровая и тд.
            // Реактивная система, инициализирующая матч, должна поднянуться по загрузке игровой сцены

            CreatePlayer();
            CreateCamera();
            CreateDummy();
        }

        private void CreatePlayer()
        {
            Vector3 position = _contexts.config.gameConfig.value.PlayerStartPosition;

            var e = _contexts.game.CreateEntity();

            e.isPlayer = true;
            e.isLocalPlayer = true;
            e.AddAsset("Player");
            e.AddPosition(position);
            e.isCameraTarget = true;
        }

        private void CreateCamera()
        {
            var e = _contexts.game.CreateEntity();

            e.isCamera = true;
            e.AddAsset("MainCamera");
            e.AddPosition(new Vector3());
        }

        private void CreateDummy()
        {
            Vector3 position = _contexts.config.gameConfig.value.DummyStartPosition;

            var e = _contexts.game.CreateEntity();

            e.isPlayer = true;
            e.AddAsset("Player");
            e.AddPosition(position);
            e.isAiCharacter = true;
            e.AddTimer(3f); // Dummy timer before first dash
        }
    }
}