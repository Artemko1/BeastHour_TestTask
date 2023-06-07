using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

// Нужно двигать только один раз за кадр, чтобы аниматор адекватно подтягивал velocity с контроллера
[Game, Cleanup(CleanupMode.RemoveComponent)]
public sealed class FrameLocomotionComponent : IComponent
{
    public Vector3 Value = Vector3.zero;
}