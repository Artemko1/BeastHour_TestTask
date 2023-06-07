using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Config, Unique, ComponentName("GameConfig")]
public interface IGameConfig
{
    Vector3 PlayerStartPosition { get; }
    Vector3 DummyStartPosition { get; }
    float PlayerSpeed { get; }
    float DashDistance { get; }
    float DashMoveSpeed { get; }
    Material[] CharacterOriginMaterials { get; }
    Material CharacterAlteredMaterial { get; }
    float InvulDuration { get; }
}