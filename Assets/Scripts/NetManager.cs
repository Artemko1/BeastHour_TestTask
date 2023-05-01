using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetManager : NetworkManager
{
    private List<Transform> _unusedStartPositions = new List<Transform>();

    public IList<PlayerBase> CurrentPlayers { get; } = new SyncList<PlayerBase>();
    public event Action<PlayerBase> PlayerConnected;
    public event Action<PlayerBase> PlayerDisonnected;

    public override Transform GetStartPosition()
    {
        startPositions.RemoveAll(t => t == null);

        if (startPositions.Count == 0) return null;

        if (_unusedStartPositions.Count == 0)
        {
            ResetUnusedStartPositions();
        }

        int index = Random.Range(0, _unusedStartPositions.Count);
        Transform position = _unusedStartPositions[index];
        _unusedStartPositions.RemoveAt(index);

        return position;
    }

    private void ResetUnusedStartPositions() =>
        _unusedStartPositions = new List<Transform>(startPositions);

    public override void OnServerAddPlayer(NetworkConnectionToClient someConnection)
    {
        base.OnServerAddPlayer(someConnection);
        var player = someConnection.identity.GetComponent<PlayerBase>();
        CurrentPlayers.Add(player);
        PlayerConnected?.Invoke(player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient disconnectedClient)
    {
        var player = disconnectedClient.identity.GetComponent<PlayerBase>();
        CurrentPlayers.Remove(player);
        PlayerDisonnected?.Invoke(player);
        base.OnServerDisconnect(disconnectedClient);
    }

    public void RespawnCurrentPlayers()
    {
        ResetUnusedStartPositions();
        foreach (PlayerBase somePlayer in CurrentPlayers)
        {
            Transform randomSpot = GetStartPosition();
            somePlayer.SetPosition(randomSpot.position);
        }
    }
}