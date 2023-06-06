using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Input, Unique]
public sealed class MoveInputComponent : IComponent
{
    public Vector2 Value = Vector2.zero;
}