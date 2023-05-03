using UnityEngine;

namespace Camera
{
    public class CursorController : MonoBehaviour
    {
        private void Start() =>
            Cursor.lockState = CursorLockMode.Confined;
    }
}