using Player;
using UnityEngine;

namespace Camera
{
    public class CursorController : MonoBehaviour
    {
        private void Update()
        {
            if (InputService.Esc)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            if (InputService.RMB)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        private void OnEnable() =>
            Cursor.lockState = CursorLockMode.Confined;

        private void OnDisable() =>
            Cursor.lockState = CursorLockMode.None;
    }
}