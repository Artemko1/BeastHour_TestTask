using UnityEngine;

[CreateAssetMenu(menuName = "BH/Ai Config")]
public class ScriptableAiConfig : ScriptableObject, IAiConfig
{
    [field: SerializeField] public bool DashEnabled { get; private set; } = true;
    [field: SerializeField] public float DashCooldown { get; private set; } = 3f;
}