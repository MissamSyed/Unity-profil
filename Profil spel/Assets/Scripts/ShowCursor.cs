using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    void Start()
    {
        // Make the mouse cursor visible
        Cursor.visible = true;

        // Optionally, set the cursor to be unlocked so it can move freely
        Cursor.lockState = CursorLockMode.None;
    }
}
