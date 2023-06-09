using UnityEngine;

namespace Player
{
    public static class InputService
    {
        public static Vector2 MovementAxis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        public static Vector2 MouseAxis => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        public static bool LMB => Input.GetMouseButtonDown(0);
        public static bool RMB => Input.GetMouseButtonDown(1);
        public static bool Esc => Input.GetKeyDown(KeyCode.Escape);
    }
}