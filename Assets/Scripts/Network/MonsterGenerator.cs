using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using Mirror.Examples.CCU;
using UnityEngine;

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
        LoadEnemyPrefabs();

        if (isServer)
        {
            StartCoroutine(SpawnMonster());
        }
    }

    void Update()
    {

    }

    void LoadEnemyPrefabs()
    {
        enemyPrefabs = new Dictionary<string, GameObject>();
        var folderPath = "Assets/Resources/Prefabs/Enemies";
        var resPrefix = "Prefabs/Enemies/";
        string[] files =
            Directory.GetFiles(folderPath, "*.prefab");
        foreach (var filePath in files)
        {
            string enemyName = Path.GetFileNameWithoutExtension(filePath);
            GameObject prefab = Resources.Load<GameObject>(
                resPrefix + enemyName);
            enemyPrefabs[enemyName] = prefab;
        }
    }

    void GenerateEnemies(Vector2Int bottomLeft, Vector2Int topRight, int nums, String enemyName)
    {
        GenerateEnemies(bottomLeft, topRight, nums, enemyPrefabs[enemyName]);
    }
    void GenerateEnemies(Vector2Int bottomLeft, Vector2Int topRight, int nums, GameObject enemyPrefab)
    {
        for (int i = 0; i < nums; i++)
        {
            int x = UnityEngine.Random.Range(bottomLeft.x + 1, topRight.x - 1);
            int y = UnityEngine.Random.Range(bottomLeft.y + 1, topRight.y - 1);
            if (mg.map[x, y] == MapGenerator.GridType.FLOOR && !mg.IsBorder(x, y)) // 确保选定位置是地板
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0); // 创建一个Tilemap坐标
                Vector3 worldPosition = mg.groundTilemap.CellToWorld(tilePosition); // 将Tilemap坐标转换为世界坐标
                var enemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity); // 在转换后的世界坐标处实例化敌人预制体
                NetworkServer.Spawn(enemy);
                
            }
        }
    }

    void GenerateBoss(Vector2Int bottomLeft, Vector2Int topRight, int nums, String enemyName)
    {
        GenerateBoss(bottomLeft, topRight, nums, enemyPrefabs[enemyName]);
    }

    void GenerateBoss(Vector2Int bottomLeft, Vector2Int topRight, int nums, GameObject bossPrefab)
    {
        while (nums > 0)
        {
            int x = UnityEngine.Random.Range(bottomLeft.x + 1, topRight.x - 1);
            int y = UnityEngine.Random.Range(bottomLeft.y + 1, topRight.y - 1);
            if (mg.map[x, y] == MapGenerator.GridType.FLOOR && !mg.IsBorder(x, y)) // 确保选定位置是地板
            {
                Debug.Log("map[x, y]" + mg.map[x, y]);
                Vector3Int tilePosition = new Vector3Int(x, y, 0); // 创建一个Tilemap坐标
                Vector3 worldPosition = mg.groundTilemap.CellToWorld(tilePosition); // 将Tilemap坐标转换为世界坐标
                var boss = Instantiate(bossPrefab, worldPosition, Quaternion.identity); // 在转换后的世界坐标处实例化敌人预制体
                NetworkServer.Spawn(boss);
                nums--;
            }
        }
    }



    [Server]
    IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(2f);
        foreach (var room in mg.rooms)
        {
            GenerateEnemies(room.bottomLeft, room.topRight, 10, "Enemy1");
            GenerateEnemies(room.bottomLeft, room.topRight, 5, "Enemy2");
        }
        GenerateEnemies(mg.farRoom.bottomLeft, mg.farRoom.topRight, 1, "Boss1");
    }


}
