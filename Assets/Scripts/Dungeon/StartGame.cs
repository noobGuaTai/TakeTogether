using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class StartGame : NetworkBehaviour
{
    private bool isPlayerInside = false;
    private int ready = 0;
    public bool load = false;

    void Update()
    {
        // if (isPlayerInside && Input.GetButtonDown("Attack"))
        // {
        //     LoaclLoadNewScene(); 
        // }
        // if(isServer)
        // {
        //     OnlineLoadNewScene();
        // }
        Test();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 检测进入碰撞的是不是玩家
        if (other.CompareTag("Player"))
        {
            // isPlayerInside = true;
            other.GetComponent<PlayerAttribute>().isReady = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAttribute>().isReady = false;
        }
    }

    void LoaclLoadNewScene()
    {
        SceneManager.LoadScene("Level");
    }

    [Server]
    void OnlineLoadNewScene()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null)
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<PlayerAttribute>().isReady == true)
                    ready++;
            }
            if(ready == players.Length)
            {
                NetworkManager.singleton.ServerChangeScene("Level");
                //Destroy(gameObject);
            }
            
        }

    }

    void Test()
    {
        if(load == true)
        {
            NetworkManager.singleton.ServerChangeScene("Level");
            load = false;
        }
    }
}
