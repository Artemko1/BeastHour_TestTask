using UnityEngine;

public static class InputService
{
    public static Vector2 MovementAxis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public static Vector2 MouseAxis => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    public static bool LMB => Input.GetMouseButtonDown(0);
}