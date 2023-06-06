using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Config, Unique, ComponentName("GameConfig")]
public interface IGameConfig
{
    Vector3 PlayerStartPosition { get; }
    float PlayerSpeed { get; }
    float DashDistance { get; }
    float DashDuration { get; } // убрать
    float DashMoveSpeed { get; }
}
