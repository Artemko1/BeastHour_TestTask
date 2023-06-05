using Entitas;
using UnityEngine;


[Game]
public sealed class PositionComponent : IComponent
{
    public Vector2 Value = Vector2.zero;
}