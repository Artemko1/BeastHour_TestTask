using UnityEngine;

[CreateAssetMenu(menuName = "Match One/Game Config")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [SerializeField] private Vector3 _playerStartPosition;
    public Vector3 PlayerStartPosition => _playerStartPosition;
}
