using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    // Start is called before the first frame update

    List<GameObject> gameObjects = new List<GameObject>();
    void Start()
    {
        gameObjects.Add(transform.Find("MapGrids").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchMap")) {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                var gameObject = gameObjects[i];
                var flag = gameObject.activeSelf;
                gameObject.SetActive(!flag);
            }
        }
    }
}
