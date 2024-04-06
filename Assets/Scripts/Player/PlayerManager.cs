using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;
    public List<GameObject> players = new List<GameObject>();
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
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {

    }

    public void addPlayer(GameObject player, String name = "")
    {
        players.Add(player);
        if (name != "")
            player.name = name;
    }

    public void InitPlayers(Vector3 vector)
    {
        foreach(var player in players)
        {
            var p = Instantiate(player, vector, Quaternion.identity);
            //p.name = "Player";
            NetworkServer.Spawn(p);
        }

    }

}
