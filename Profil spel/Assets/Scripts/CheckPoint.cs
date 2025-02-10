using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] float timeTillLoad = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Capture", timeTillLoad);
        }
    }

    void Capture()
    {
        Debug.Log("Check point captured");
        
    }

}
