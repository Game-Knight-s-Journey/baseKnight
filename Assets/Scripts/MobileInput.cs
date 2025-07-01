using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static bool moveLeft;
    public static bool moveRight;
    public static bool jump;

    public void OnLeftDown() { moveLeft = true; }
    public void OnLeftUp() { moveLeft = false; }

    public void OnRightDown() { moveRight = true; }
    public void OnRightUp() { moveRight = false; }

    public void OnJumpDown() { jump = true; }
    public void OnJumpUp() { jump = false; }
}
