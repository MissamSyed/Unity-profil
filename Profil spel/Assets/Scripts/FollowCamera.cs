using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; 
    public float smoothSpeed = 10f; 
    public Vector3 offset = new Vector3(0f, 0f, -10f); 

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("FollowCamera: Ingen target har satts!");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
