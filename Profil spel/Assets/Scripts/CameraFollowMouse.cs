using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public Transform player;   
    public float followSpeed = 5f; 
    public float maxOffset = 5f;  

    void Update()
    {
        if (player == null) return;

        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; 

        
        Vector3 targetPos = player.position + (mousePos - player.position) / 2;

        
        if (Vector3.Distance(player.position, targetPos) > maxOffset)
        {
            targetPos = player.position + (targetPos - player.position).normalized * maxOffset;
        }

        
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
