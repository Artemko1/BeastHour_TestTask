using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetRoomManager : NetworkRoomManager
{
    private List<Transform> _unusedStartPositions = new List<Transform>();
    public event Action<NetworkIdentity> ServerReady;
    public event Action<NetworkIdentity> ServerPlayerDisconnected;

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

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
        // Resetting unused positions to prevent them keeping state on return to lobby
        if (sceneName == RoomScene)
        {
            ResetUnusedStartPositions();
        }
    }

    public void ResetUnusedStartPositions() =>
        _unusedStartPositions = new List<Transform>(startPositions);

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        ServerReady?.Invoke(conn.identity);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient disconnectedClient)
    {
        ServerPlayerDisconnected?.Invoke(disconnectedClient.identity);
        base.OnServerDisconnect(disconnectedClient);
    }
}