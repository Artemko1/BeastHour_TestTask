using Entitas;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        CreateUI();
    }

    private void CreatePlayer()
    {
        Vector3 position = _contexts.config.gameConfig.value.PlayerStartPosition;

        var e = CreateCharacter(position);

        e.isLocalPlayer = true;
        e.isCameraTarget = true;
        e.AddName("Player");
    }

    private void CreateDummy()
    {
        Vector3 position = _contexts.config.gameConfig.value.DummyStartPosition;

        var e = CreateCharacter(position);

        e.isAiCharacter = true;
        e.AddTimer(3f); // Dummy timer before first dash
        e.AddName("Dummy");
    }

    private GameEntity CreateCharacter(Vector3 position)
    {
        var e = _contexts.game.CreateEntity();
        e.isPlayer = true;
        e.AddAsset("Player");
        e.AddPosition(position);
        e.AddScore(0);

        return e;
    }

    private void CreateCamera()
    {
        var e = _contexts.game.CreateEntity();

        e.isCamera = true;
        e.AddAsset("MainCamera");
        e.AddPosition(new Vector3());
    }

    private void CreateUI()
    {
        var e = _contexts.game.CreateEntity();
        e.AddAsset("Canvas");
    }
}