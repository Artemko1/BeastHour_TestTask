using GameEcs.Scripts.Camera;
using GameEcs.Scripts.Player;

public class UpdateSystems : Feature
{
    public UpdateSystems(Contexts contexts)
    {
        // Bootstrap
        Add(new BootstrapSystems(contexts));

        // Input
        Add(new InputSystems(contexts));


        // View create
        Add(new AddViewSystem(contexts));
        // Init Entities from view
        Add(new InitViewPositionSystem(contexts));
        Add(new InitPlayerSystem(contexts));
        Add(new InitCameraSystem(contexts));

        // AI logic
        Add(new AiRandomDashSystem(contexts));

        // Logic
        Add(new RemoveInvulnerabilitySystem(contexts));
        
        // Dash
        Add(new StartDashFromInputSystem(contexts));
        Add(new DashSystem(contexts));

        // Movement
        Add(new PlayerMoveInputIntoDesiredMoveSystem(contexts));
        Add(new ApplyDesiredMoveSystem(contexts));
        
        Add(new ApplyLocomotionSystem(contexts)); // final move - calling controller.Move


        // View update
        Add(new AnimatePlayerMoveSystem(contexts));
        Add(new UpdateCameraTargetPositionSystem(contexts));
        

        // Events
        Add(new GameEventSystems(contexts));

        // Cleanup
        Add(new InputCleanupSystems(contexts));
        Add(new GameCleanupSystems(contexts));
    }
}

public class InputSystems : Feature
{
    public InputSystems(Contexts contexts)
    {
        // Update time
        Add(new UpdateTimeSystem(contexts));
        Add(new DecreaseTimersSystem(contexts));

        // Read
        Add(new ReadWasdInputSystem(contexts));
        Add(new ReadLmbInputSystem(contexts));
    }
}