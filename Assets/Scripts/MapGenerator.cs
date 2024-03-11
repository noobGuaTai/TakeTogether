using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public string seed;
    public bool useRandomSeed = true;
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tile wallTile;
    public Tile floorTile;

    [Range(0, 100)]
    public int randomFillPercent = 45;

    int[,] map;
    public List<Room> rooms;

    void Start()
    {
        StartCoroutine(GenerateMapCoroutine());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(GenerateMapCoroutine());
        }
    }

    void PrintMap()
    {
        string mapString = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapString += map[x, y] + " ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }


    IEnumerator GenerateMapCoroutine()
    {
        map = new int[width, height];
        // 初始化map为全-1
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = -1;
            }
        }
        wallTilemap.ClearAllTiles();
        groundTilemap.ClearAllTiles();

        foreach (var room in rooms)
        {
            RandomFillMap(room.bottomLeft, room.topRight);

            for (int i = 0; i < 5; i++)
            {
                SmoothMap(room.bottomLeft, room.topRight);
            }
            FillTilemap(room.bottomLeft, room.topRight);

            yield return new WaitForSeconds(0.5f); // 暂停0.5秒
        }

        //在所有房间生成完毕后连接它们
        ConnectRooms();

        // //重新填充地图，以确保路径被正确渲染
        // foreach (var room in rooms)
        // {
        //FillTilemap(new Vector2Int(width, width), new Vector2Int(height, height));
        // }
        // PrintMap();
    }


    void RandomFillMap(Vector2Int bottomLeft, Vector2Int topRight)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = bottomLeft.x; x < topRight.x; x++)
        {
            for (int y = bottomLeft.y; y < topRight.y; y++)
            {
                if (x == bottomLeft.x || x == topRight.x - 1 || y == bottomLeft.y || y == topRight.y - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap(Vector2Int bottomLeft, Vector2Int topRight)
    {
        for (int x = bottomLeft.x; x < topRight.x; x++)
        {
            for (int y = bottomLeft.y; y < topRight.y; y++)
            {
                if (x == bottomLeft.x || x == topRight.x - 1 || y == bottomLeft.y || y == topRight.y - 1)
                    continue; // Skip border tiles

                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;
            }
        }
    }


    void FillTilemap(Vector2Int bottomLeft, Vector2Int topRight)
    {
        for (int x = bottomLeft.x; x < topRight.x; x++)
        {
            for (int y = bottomLeft.y; y < topRight.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (map[x, y] == 0) // 如果当前位置是地板
                {
                    if (IsBorder(x, y))
                    {
                        // 如果当前位置是边缘，则放置墙壁
                        // wallTilemap.SetTile(position, null); // 首先清除该位置上可能存在的瓦片
                        wallTilemap.SetTile(position, wallTile); // 然后放置新的墙壁瓦片
                    }
                    else
                    {
                        // 否则，放置地板
                        // wallTilemap.SetTile(position, null);
                        // groundTilemap.SetTile(position, null); // 首先清除该位置上可能存在的瓦片
                        groundTilemap.SetTile(position, floorTile); // 然后放置新的地板瓦片
                    }

                }

            }
        }
    }



    bool IsBorder(int x, int y)
    {
        if (map[x, y] != 0) return false; // 当前位置如果不是地板，则直接返回false

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;
                // 跳过自己和确保检查点在地图范围内
                if (dx == 0 && dy == 0) continue;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (map[nx, ny] == 1) // 如果相邻位置是墙壁，则当前位置是边缘
                    {
                        return true;
                    }
                }
            }
        }
        return false; // 如果周围没有墙壁，则不是边缘
    }


    void SetBorderWalls(int x, int y)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        wallTilemap.SetTile(position, wallTile); // 在当前地板位置放置墙壁
    }


    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void ConnectRooms()
    {
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int roomCenterA = rooms[i].Center;
            Vector2Int roomCenterB = rooms[i + 1].Center;

            // 生成两个中心点之间的路径
            GeneratePath(roomCenterA, roomCenterB);
        }
    }

    void GeneratePath(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start;

        // 先水平后垂直移动
        while (current.x != end.x)
        {
            SetFloorAndWalls(current, true); // true 表示水平移动
            current.x += (end.x > current.x) ? 1 : -1;
        }

        while (current.y != end.y)
        {
            SetFloorAndWalls(current, false); // false 表示垂直移动
            current.y += (end.y > current.y) ? 1 : -1;
        }
    }


    void SetFloorAndWalls(Vector2Int position, bool isHorizontal)
    {
        // 设置中间的路为地板
        wallTilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
        groundTilemap.SetTile(new Vector3Int(position.x, position.y, 0), floorTile);
        map[position.x, position.y] = 0;

        // 设置当前方向的墙壁
        Vector2Int[] directions = {
        new Vector2Int(0, 1), // 上
        new Vector2Int(1, 0), // 右
        new Vector2Int(0, -1), // 下
        new Vector2Int(-1, 0) // 左
    };

        foreach (var dir in directions)
        {
            Vector3Int wallPos = new Vector3Int(position.x + dir.x, position.y + dir.y, 0);
            if (wallPos.x >= 0 && wallPos.x < width && wallPos.y >= 0 && wallPos.y < height)
            {
                if (map[wallPos.x, wallPos.y] != 0) // 如果不是地板，则设置为墙壁
                {
                    map[wallPos.x, wallPos.y] = 1; // 墙壁标记为1
                    wallTilemap.SetTile(wallPos, wallTile);
                }
            }
        }

        // 额外设置拐角的墙壁，以确保完全覆盖
        SetCornerWalls(position);
    }

    void SetCornerWalls(Vector2Int position)
    {
        Vector2Int[] cornerDirections = {
        new Vector2Int(1, 1),   // 右上
        new Vector2Int(1, -1),  // 右下
        new Vector2Int(-1, -1), // 左下
        new Vector2Int(-1, 1)   // 左上
    };

        foreach (var dir in cornerDirections)
        {
            Vector3Int cornerPos = new Vector3Int(position.x + dir.x, position.y + dir.y, 0);
            if (cornerPos.x >= 0 && cornerPos.x < width && cornerPos.y >= 0 && cornerPos.y < height)
            {
                if (map[cornerPos.x, cornerPos.y] != 0) // 如果不是地板，则设置为墙壁
                {
                    map[cornerPos.x, cornerPos.y] = 1; // 墙壁标记为1
                    wallTilemap.SetTile(cornerPos, wallTile);
                }
            }
        }
    }











    // void OnDrawGizmos()
    // {
    //     if (map != null)
    //     {
    //         for (int x = 0; x < width; x++)
    //         {
    //             for (int y = 0; y < height; y++)
    //             {
    //                 Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                 Vector3 pos = new Vector3(-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f);
    //                 Gizmos.DrawCube(pos, Vector3.one);
    //             }
    //         }
    //     }
    // }

    [System.Serializable]
    public class Room
    {
        public Vector2Int bottomLeft; // 房间左下角坐标
        public Vector2Int topRight;   // 房间右上角坐标
        public string seed; // 为每个房间指定的种子，以生成随机填充

        public Room(Vector2Int _bottomLeft, Vector2Int _topRight, string _seed)
        {
            bottomLeft = _bottomLeft;
            topRight = _topRight;
            seed = _seed;
        }

        public Vector2Int Center
        {
            get
            {
                return new Vector2Int((bottomLeft.x + topRight.x) / 2, (bottomLeft.y + topRight.y) / 2);
            }
        }
    }
}
