using UnityEngine;

public class simplemovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
