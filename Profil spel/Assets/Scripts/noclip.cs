using UnityEngine;

public class NoClip2D : MonoBehaviour
{
    public bool noClipEnabled = false;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // Tryck 'N' för att aktivera/inaktivera noclip
        {
            noClipEnabled = !noClipEnabled;
            col.enabled = !noClipEnabled; // Stänger av collidern

            if (noClipEnabled)
            {
                rb.gravityScale = 0;  // Ingen gravitation i noclip-läge
                rb.velocity = Vector2.zero; // Stoppar rörelsen
                rb.isKinematic = true; // Hindrar fysikpåverkan
            }
            else
            {
                rb.gravityScale = 1;
                rb.isKinematic = false;
            }

            Debug.Log("NoClip: " + (noClipEnabled ? "ON" : "OFF"));
        }

        if (noClipEnabled)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            transform.position += new Vector3(moveX, moveY, 0) * Time.deltaTime * 5f; // Flytta fritt
        }
    }
}
