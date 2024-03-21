using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    // public Vector2 minPosition;
    // public Vector2 maxPosition;
    // public MapGenerator mapGenerator;
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        // if(mapGenerator.rooms != null)
        // {
        //     minPosition = mapGenerator.rooms[0].bottomLeft;
        //     maxPosition = mapGenerator.rooms[0].topRight;
        // }
        
    }

    void LateUpdate()
    {
        if(target != null)
        {
            if(transform.position != target.position)
            {
                Vector3 targetPosition = target.position;
                //targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                //targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            }
        }
        else
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
    
}
