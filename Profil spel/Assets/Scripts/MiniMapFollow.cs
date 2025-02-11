using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public float height = 10f; // Adjust the height of the minimap camera

    void LateUpdate()
    {
        if (player != null)
        {
            // Follow the player's X and Y but keep the Z position fixed
            transform.position = new Vector3(player.position.x, player.position.y, height);
        }
    }
}
