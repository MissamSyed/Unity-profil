using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public GameObject innerCrosshair;  // Reference to the inner crosshair GameObject
    public GameObject outerCrosshair;  // Reference to the outer crosshair GameObject
    public float innerOffset = 0f;     // Offset for the inner crosshair (if needed)
    public float outerOffset = 1f;     // Offset for the outer crosshair (if needed)

    void Start()
    {
        // Hide the default system cursor (optional)
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert mouse position to world space
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Ensure the Z position is set to 0 to stay in the 2D plane
        mousePosition.z = 0;

        // Position the inner crosshair slightly offset from the mouse position (if needed)
        innerCrosshair.transform.position = mousePosition + new Vector3(innerOffset, innerOffset, 0);

        // Position the outer crosshair with a different offset (if needed)
        outerCrosshair.transform.position = mousePosition + new Vector3(outerOffset, outerOffset, 0);
    }
}
