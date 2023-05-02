using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetManager : NetworkManager
{
    private List<Transform> _unusedStartPositions = new List<Transform>();

    // public readonly SyncList<PlayerBase> CurrentPlayers = new SyncList<PlayerBase>();
    // public event Action ClientConnected;
    // public event Action ClientDisconnected;
    public event Action<PlayerBase> ServerPlayerConnected;
    public event Action<PlayerBase> ServerPlayerDisconnected;

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

    public void ResetUnusedStartPositions() =>
        _unusedStartPositions = new List<Transform>(startPositions);

    public override void OnServerAddPlayer(NetworkConnectionToClient someConnection)
    {
        base.OnServerAddPlayer(someConnection);
        var player = someConnection.identity.GetComponent<PlayerBase>();
        // CurrentPlayers.Add(player);
        // player.name = "Player " + CurrentPlayers.Count;
        ServerPlayerConnected?.Invoke(player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient disconnectedClient)
    {
        var player = disconnectedClient.identity.GetComponent<PlayerBase>();
        // CurrentPlayers.Remove(player);
        ServerPlayerDisconnected?.Invoke(player);
        base.OnServerDisconnect(disconnectedClient);
    }

    // public override void OnClientConnect()
    // {
    //     base.OnClientConnect();
    //     ClientConnected?.Invoke();
    // }
}