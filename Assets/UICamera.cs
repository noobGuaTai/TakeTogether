using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 xy = transform.position;
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        if (xy != center) {
            transform.position = new Vector3(center.x, center.y, -1000);
        }
    }
}
