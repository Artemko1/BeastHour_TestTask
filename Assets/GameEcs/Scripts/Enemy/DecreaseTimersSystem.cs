using Entitas;
using UnityEngine;

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
                var time = Random.Range(2f, 6f);
                e.ReplaceTimer(time);
            }
            else
            {
                // e.ReplaceTimer(timerValue);
                e.timer.Value = timerValue; // Норм ли сделано, если я хочу ловить removed ивент только 1 раз?
            }
        }
    }
}