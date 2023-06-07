using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameController : MonoBehaviour
{
    private Contexts _contexts;
    private UpdateSystems _updateSystems;
    private FixedUpdateSystems _fixedUpdateSystems;

    [SerializeField] private ScriptableGameConfig _gameConfig;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _contexts = Contexts.sharedInstance;


        _contexts.config.SetGameConfig(_gameConfig);

        _updateSystems = new UpdateSystems(_contexts);
        _fixedUpdateSystems = new FixedUpdateSystems(_contexts);

        _updateSystems.Initialize();
        _fixedUpdateSystems.Initialize();
    }

    private void Update()
    {
        _updateSystems.Execute();
        _updateSystems.Cleanup();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems.Execute();
        _fixedUpdateSystems.Cleanup();
    }

    private void OnDestroy()
    {
        _updateSystems.DeactivateReactiveSystems();
        _updateSystems.ClearReactiveSystems();
        _updateSystems.TearDown();

        _fixedUpdateSystems.DeactivateReactiveSystems();
        _fixedUpdateSystems.ClearReactiveSystems();
        _fixedUpdateSystems.TearDown();
    }
}