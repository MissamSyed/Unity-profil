using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP40 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); 

            Debug.Log("Picked up MP-40!");
        }
    }
}
