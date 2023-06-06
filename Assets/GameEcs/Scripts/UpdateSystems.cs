using GameEcs.Scripts.Camera;
using GameEcs.Scripts.Player;

public class UpdateSystems : Feature
{
    public UpdateSystems(Contexts contexts)
    {
        // Input
        Add(new InputSystems(contexts));

        // Update positions
        Add(new ApplyDesiredMoveSystem(contexts));
        Add(new DashSystem(contexts));

        // View create
        Add(new AddViewSystem(contexts));
        
        // Init Entities from view
        Add(new InitPlayerSystem(contexts));
        Add(new InitCameraSystem(contexts));


        // Process // todo переместить между Input и Update. Но сначала перевести камеру на ецс
        // bug PlayerMoveInputIntoDesiredMoveSystem зависит от камеры. Наlо как-то пофиксить наверно
        Add(new PlayerMoveInputIntoDesiredMoveSystem(contexts));
        Add(new StartDashFromInputSystem(contexts));
        
        // Process for AI
        Add(new AiRandomDashSystem(contexts));

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