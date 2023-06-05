using UnityEngine;

[CreateAssetMenu(menuName = "Match One/Game Config")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [field: SerializeField] public Vector3 PlayerStartPosition { get; private set; }
    [field: SerializeField] public float PlayerSpeed { get; private set; } = 1;
    [field: SerializeField] public float BlinkDistance { get; private set; } = 4;
    [field: SerializeField] public float BlinkDuration { get; private set; } = 0.2f;
}
