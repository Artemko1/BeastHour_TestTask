using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetManager : NetworkManager
{
    private List<Transform> _unusedStartPositions = new List<Transform>();

    public override Transform GetStartPosition()
    {
        startPositions.RemoveAll(t => t == null);

        if (startPositions.Count == 0) return null;

        if (_unusedStartPositions.Count == 0)
        {
            _unusedStartPositions = new List<Transform>(startPositions);
        }

        int index = Random.Range(0, _unusedStartPositions.Count);
        Transform position = _unusedStartPositions[index];
        _unusedStartPositions.RemoveAt(index);

        return position;
    }
}