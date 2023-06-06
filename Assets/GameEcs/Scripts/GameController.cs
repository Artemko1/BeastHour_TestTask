using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-10)]
public class GameController : MonoBehaviour
{
    private Contexts _contexts;
    private UpdateSystems _updateSystems;
    private BootstrapSystems _bootstrapSystems;

    [SerializeField] private ScriptableGameConfig _gameConfig;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        _contexts = Contexts.sharedInstance;


        _contexts.config.SetGameConfig(_gameConfig);

        _updateSystems = new UpdateSystems(_contexts);
        _bootstrapSystems = new BootstrapSystems(_contexts);

        _bootstrapSystems.Initialize();
        _updateSystems.Initialize();
    }

    private void Update()
    {
        _updateSystems.Execute();
        _updateSystems.Cleanup();
    }

    private void OnDestroy()
    {
        _updateSystems.DeactivateReactiveSystems();
        _updateSystems.ClearReactiveSystems();
        _updateSystems.TearDown();
    }
}