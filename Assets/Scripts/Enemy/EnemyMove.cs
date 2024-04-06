using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyMove : NetworkBehaviour
{
    public GameObject[] allPlayers;
    public GameObject closedPlayer;
    [SyncVar]public bool isAttacking;
    public virtual void Move() { }

    public GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            return closestPlayer;
        }
        else
        {
            return null;
        }
    }

    public GameObject[] FindAllPlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player");
    }
}
