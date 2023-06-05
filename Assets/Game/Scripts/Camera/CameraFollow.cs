using Mirror;

namespace Camera
{
    public class CameraFollow : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            UnityEngine.Camera.main.GetComponent<ThirdPersonCamera>().SetTarget(transform);
        }
    }
}