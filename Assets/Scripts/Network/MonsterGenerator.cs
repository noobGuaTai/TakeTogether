using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using Mirror.Examples.CCU;
using UnityEditor;
using UnityEngine;
using YooAsset;

public class MonsterGenerator : NetworkBehaviour
{
    public GameObject[] monsterPrefab;
    public GameObject mapGen;
    public Dictionary<string, GameObject> enemyPrefabs;
    public GameObject enemies;

    private MapGenerator mg;

    void Start()
    {
        mg = mapGen.GetComponent<MapGenerator>();
        enemies = transform.Find("/Enemies").gameObject;
        StartCoroutine(LoadEnemyPrefabs());

        if (isServer)
        {
            StartCoroutine(SpawnMonster());
        }
    }

    IEnumerator LoadEnemyPrefabs()
    {
        yield return null;
        enemyPrefabs = new Dictionary<string, GameObject>();
        List<GameObject> prefabs = new List<GameObject>();

        AssetInfo[] ass = YooAssets.GetAssetInfos("enemy");
        foreach (AssetInfo info in ass)
        {
            if(info.AssetPath.Contains("EnemyPrefab"))
            {
                AssetHandle ah = YooAssets.LoadAssetAsync<GameObject>(info.AssetPath);
                yield return ah;
                GameObject prefab = ah.AssetObject as GameObject;
                prefabs.Add(prefab);
            }
            
        }


        foreach (GameObject prefab in prefabs)
        {
            if (!enemyPrefabs.ContainsKey(prefab.name))
            {
                enemyPrefabs[prefab.name] = prefab;
            }
            else
            {
                Debug.LogWarning("Duplicate prefab name found in 'Prefabs/Enemies': " + prefab.name);
            }
        }
    }

    void GenerateEnemy(Vector2Int bottomLeft, Vector2Int topRight, int nums, String enemyName)
    {
        StartCoroutine(GenerateEnemyCoroutine(bottomLeft, topRight, nums, enemyPrefabs[enemyName]));
    }

    IEnumerator GenerateEnemyCoroutine(Vector2Int bottomLeft, Vector2Int topRight, int nums, GameObject enemyPrefab)
    {
        int temp = 200;
        while (nums > 0 && temp-- > 0)
        {
            int x = UnityEngine.Random.Range(bottomLeft.x + 1, topRight.x - 1);
            int y = UnityEngine.Random.Range(bottomLeft.y + 1, topRight.y - 1);
            if (mg.map[x, y] == MapGenerator.GridType.FLOOR && !mg.IsBorder(x, y)) // 确保选定位置是地板
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0); // 创建一个Tilemap坐标
                Vector3 worldPosition = mg.groundTilemap.CellToWorld(tilePosition); // 将Tilemap坐标转换为世界坐标
                var enemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity); // 在转换后的世界坐标处实例化敌人预制体
                NetworkServer.Spawn(enemy);
                nums--;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


    [Server]
    IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(2f);
        foreach (var room in mg.rooms)
        {
            GenerateEnemy(room.bottomLeft, room.topRight, 10, "Enemy1");
            GenerateEnemy(room.bottomLeft, room.topRight, 5, "Enemy2");
        }
        GenerateEnemy(mg.farRoom.bottomLeft, mg.farRoom.topRight, 1, "Boss1");
    }


}
