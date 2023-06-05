using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Cleanup(CleanupMode.RemoveComponent)]
public sealed class VelocityComponent : IComponent
{
    public Vector3 Value = Vector3.zero;
}