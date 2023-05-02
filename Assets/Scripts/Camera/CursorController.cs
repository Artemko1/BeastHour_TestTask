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
        }

        private void OnEnable() =>
            Cursor.lockState = CursorLockMode.Locked;

        private void OnDisable() =>
            Cursor.lockState = CursorLockMode.None;
    }
}