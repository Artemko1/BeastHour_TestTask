using BH.Input.UserInput;

public class UpdateSystems : Feature
{
    public UpdateSystems(Contexts contexts)
    {
        // Input
        Add(new InputSystems(contexts));

        // Update
        Add(new ApplyDesiredMoveSystem(contexts));

        // View
        Add(new AddViewSystem(contexts));

        // Events
        Add(new GameEventSystems(contexts));

        // Cleanup
        Add(new InputCleanupSystems(contexts)); // Почему не удаляются сущности всё равно?
        Add(new GameCleanupSystems(contexts));
    }
}

public class InputSystems : Feature
{
    public InputSystems(Contexts contexts)
    {
        // Update time
        Add(new UpdateTimeSystem(contexts));
        
        // Read
        Add(new ReadWasdInputSystem(contexts));
        Add(new ReadLmbInputSystem(contexts));
        
        // Process
        Add(new ProcessPlayerMoveInputSystem(contexts));
        Add(new ProcessLmbInputSystem(contexts));
    }
}