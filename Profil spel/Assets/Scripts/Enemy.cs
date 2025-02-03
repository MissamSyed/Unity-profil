using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enenmy : MonoBehaviour
{
    GameObject player;
    [SerializeField] float enemySpeed = 0f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * enemySpeed * Time.fixedDeltaTime);

        }


    }
}
