using Mirror;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            Debug.Log("OnStartLocalPlayer", this);
            UnityEngine.Camera.main.GetComponent<ThirdPersonCamera>().SetTarget(transform);
        }
    }
}