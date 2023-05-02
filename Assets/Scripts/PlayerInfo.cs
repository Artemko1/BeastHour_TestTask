using Mirror;
using UnityEngine;

public class PlayerInfo
{
    public PlayerInfo() // default ctor for Mirror deserialization
    {
    }

    public PlayerInfo(ulong id, string name)
    {
        Id = id;
        Name = name;
        Score = 0;
    }

    public readonly ulong Id;

    public readonly string Name;

    [field:SyncVar(hook = nameof(SyncScore))]
    public int Score { get; [Server] set; }

    [Server]
    public void SetScore(int newValue)
    {
        Score = newValue;
    }

    private void SyncScore(string oldValue, string newValue)
    {
        Debug.Log($"Score changed to {newValue}");
    }
}