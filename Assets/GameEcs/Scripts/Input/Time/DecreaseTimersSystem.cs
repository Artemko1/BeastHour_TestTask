using Entitas;

public sealed class DecreaseTimersSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public DecreaseTimersSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.Timer);
    }
    public void Execute()
    {
        float deltaTimeValue = _contexts.input.deltaTime.value;
        foreach (GameEntity e in _group.GetEntities())
        {
            float timerValue = e.timer.Value - deltaTimeValue;
            if (timerValue <= 0)
            {
                e.RemoveTimer();
            }
            else
            {
                e.ReplaceTimer(timerValue);
            }
        }
    }
}