using BH.Input.UserInput;

public class UpdateSystems : Feature
{
    public UpdateSystems(Contexts contexts)
    {
        
        // Add(new ConfigEventSystems(contexts));
        // Input
        Add(new InputSystems(contexts));
        Add(new ProcessPlayerMoveInputSystem(contexts));
        // Add(new GameStateSystems(contexts));
        // Add(new GameStateEventSystems(contexts));

        // View
        Add(new AddViewSystem(contexts));


        Add(new GameEventSystems(contexts));
    }
}

public class InputSystems : Feature
{
    public InputSystems(Contexts contexts)
    {
        // Add(new InitPointerSystem(contexts));

        Add(new UpdateTimeSystem(contexts));
        Add(new ReadWasdInputSystem(contexts));
    }
}