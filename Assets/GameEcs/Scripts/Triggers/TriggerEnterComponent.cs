using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Cleanup(CleanupMode.RemoveComponent)]
public sealed class TriggerEnterComponent : IComponent
{
    public GameEntity Other;
}