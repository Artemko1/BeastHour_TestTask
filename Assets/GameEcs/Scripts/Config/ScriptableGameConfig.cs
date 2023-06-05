using UnityEngine;

[CreateAssetMenu(menuName = "Match One/Game Config")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [field: SerializeField] public Vector3 PlayerStartPosition { get; private set; }
    [field: SerializeField] public float PlayerSpeed { get; private set; } = 1;
}
