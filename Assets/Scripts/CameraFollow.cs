using Mirror;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("OnStartLocalPlayer", this);
        Camera.main.GetComponent<ThirdPersonCamera>().SetTarget(transform);
    }
}