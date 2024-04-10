using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;
    public List<GameObject> allPlayers = new List<GameObject>();
    public GameObject localPlayer;
    public GameObject otherPlayer;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        StartCoroutine(FindPlayer());
    }


    void Update()
    {
    }

    IEnumerator FindPlayer() // 暂时先写成协程，等1s搜索玩家
    {
        yield return new WaitForSeconds(1f);

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var networkIdentity = player.GetComponent<NetworkIdentity>();
            if (networkIdentity.isLocalPlayer)
            {
                localPlayer = player;
            }
            else
            {
                otherPlayer = player;
            }
        }

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            allPlayers.Add(player);
        }
    }
}
