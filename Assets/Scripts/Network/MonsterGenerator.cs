using Mirror;
using Mirror.Examples.CCU;
using UnityEngine;

public class MonsterGenerator : NetworkBehaviour
{
    public GameObject[] monsterPrefab;
    public bool startSpawning = false;

    void Update()
    {
        if(startSpawning && isServer)
        {
            SpawnMonster();
            startSpawning = false;
        }
    }

    [Server]
    void SpawnMonster()
    {
        foreach (GameObject m in monsterPrefab)
        {
            GameObject monster = Instantiate(m, GetSpawnPosition(), Quaternion.identity);
            NetworkServer.Spawn(monster);
        }

    }

    Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
    }
}
