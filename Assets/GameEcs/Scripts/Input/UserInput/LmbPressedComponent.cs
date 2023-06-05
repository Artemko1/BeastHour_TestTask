using Entitas;
using Entitas.CodeGeneration.Attributes;

[Input, Unique, Cleanup(CleanupMode.DestroyEntity)]
public sealed class LmbInputComponent : IComponent
{
}