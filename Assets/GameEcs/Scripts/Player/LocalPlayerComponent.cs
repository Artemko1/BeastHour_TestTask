using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Game, Event(EventTarget.Any)]
public class LocalPlayerComponent : IComponent
{
}