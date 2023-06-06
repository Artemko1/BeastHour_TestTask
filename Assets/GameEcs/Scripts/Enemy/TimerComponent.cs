using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class TimerComponent : IComponent
{
    [EntityIndex] public float Value;
}