using BH.Input.UserInput;

public class UpdateSystems : Feature
{
    public UpdateSystems(Contexts contexts)
    {
        // Input
        Add(new InputSystems(contexts));

        // Update
        Add(new ApplyDesiredMoveSystem(contexts));
        Add(new DashSystem(contexts));

        // View
        Add(new AddViewSystem(contexts));
        //todo добавить playerInitSystem, после того как создали вью. Получаем компоненты
        // Все компоненты с юнити будут раскиданы в компоненты ецсные

        
        // Process
        Add(new ProcessPlayerMoveInputSystem(contexts));
        
        Add(new StartDashSystem(contexts));
        
        
        // Добавить систему, которая будет обновлять позицию камеры. После всех перемещений игрока
        
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
        
    }
}