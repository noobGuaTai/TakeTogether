
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Mirror;
using TMPro;

public class MapGenerator : NetworkBehaviour
{
	public enum GridType
	{
		VOID = -1,
		FLOOR,
		WALL
	}
	public int width = 100;
	public int height = 100;
	[SyncVar] public string publicSeed;
	public bool useRandomSeed = true;
	public Tilemap groundTilemap;
	public Tilemap wallTilemap;
	public Tile wallTile;
	public Tile floorTile;
	[Range(0, 100)]
	public int randomFillPercent = 45;
	public int roomNums = 5;
	public int roomSize = 30;
	[SyncVar] public List<Room> rooms;

	public List<GameObject> players = new List<GameObject>();
	public Room furthestRoom;
	public float mapGenerateProcess = 0;
	public MapGridsRenderer mapGridsRenderer;

	public Vector3 gridSize;
	public GameObject gridObject;
	public Grid grid = null;
	public GridType[,] map;
	public Room farRoom;

	public bool isFinish = false;

	private BossHPUI bossHPUI;
	private string privateSeed;
	private System.Random pseudoRandom;
	private PlayerManager playerManager;

	private void Awake()
	{

	}

	void Start()
	{
		gridObject = transform.Find("/Grid").gameObject;
		grid = gridObject.GetComponent<Grid>();
		mapGridsRenderer = transform.Find("/UI/MapView/MapGrids").GetComponent<MapGridsRenderer>();
		gridSize = grid.cellSize;
		gridSize.Scale(gridObject.transform.localScale);

		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

		if (isServer)
		{
			publicSeed = NetworkTime.time.ToString();
		}
		privateSeed = publicSeed;
		pseudoRandom = new System.Random(privateSeed.GetHashCode());

		StartCoroutine(GenerateMapCoroutine());
	}

	void Update()
	{
		if (isServer && Input.GetKeyDown(KeyCode.G))
		{
			NetworkManager.singleton.ServerChangeScene("Level");
		}

		// 等待加载后再开始搜索
		if (isFinish)//启动boss血条
		{
			if (bossHPUI == null)
			{
				bossHPUI = playerManager.localPlayer.GetComponent<PlayerAttribute>().bossHPUI.GetComponent<BossHPUI>();

			}
			else if (bossHPUI != null && bossHPUI.isDie == false && IsPlayerInRoom(farRoom, playerManager.localPlayer.transform.position))
			{
				// bossHPUI.ActivateBossHPUI();
				playerManager.localPlayer.GetComponent<PlayerMove>().isPlayerInBossRoom = true;
			}
			else if (bossHPUI != null && bossHPUI.isDie == true)
			{
				bossHPUI.HideBossHPUI();
			}
		}
	}

	List<Room> GenerateRooms(int numberOfRooms)
	{
		List<Room> rooms = new List<Room>();
		int attemptLimit = 1000;//最大1000次循环

		for (int i = 0; i < numberOfRooms; i++)
		{
			bool isPlaced = false;
			int attempts = 0;
			while (!isPlaced && attempts < attemptLimit)
			{
				Vector2Int center = new Vector2Int(UnityEngine.Random.Range(roomSize / 2, Math.Min(width, height) - roomSize / 2), UnityEngine.Random.Range(roomSize / 2, Math.Min(width, height) - roomSize / 2));
				Vector2Int bottomLeft = new Vector2Int(center.x - roomSize / 2, center.y - roomSize / 2);
				Vector2Int topRight = new Vector2Int(center.x + roomSize / 2, center.y + roomSize / 2);
				Room newRoom = new Room(bottomLeft, topRight);

				isPlaced = true;
				foreach (Room room in rooms)
				{
					if ((newRoom.Center - room.Center).magnitude <= roomSize)
					{
						isPlaced = false;
						break;
					}
				}

				if (isPlaced)
				{
					rooms.Add(newRoom);
					break;
				}

				attempts++;
			}

			if (attempts == attemptLimit)
			{
				Debug.LogError("Failed to place room after " + attemptLimit + " attempts.");
				return rooms;
			}
		}
		return rooms;
	}

	Room FindFurthestRoomFromRoom0(List<Room> rooms)
	{
		Room room0 = rooms[0];
		furthestRoom = null;
		float maxDistance = 0;

		for (int i = 1; i < rooms.Count; i++) // 从第二个房间开始比较
		{
			float distance = (rooms[i].Center - room0.Center).magnitude; // 计算每个房间与房间0的距离
			if (distance > maxDistance) // 如果当前房间离房间0更远
			{
				maxDistance = distance; // 更新最大距离
				furthestRoom = rooms[i]; // 更新最远的房间
			}
		}

		return furthestRoom; // 返回最远的房间
	}

	public bool IsPlayerInRoom(Room room, Vector3 playerPosition)
	{
		// 将房间的Tilemap坐标转换为世界坐标
		Vector3Int bottomLeftTilePosition = new Vector3Int(room.bottomLeft.x, room.bottomLeft.y, 0);
		Vector3Int topRightTilePosition = new Vector3Int(room.topRight.x, room.topRight.y, 0);

		Vector3 roomBottomLeftWorld = groundTilemap.CellToWorld(bottomLeftTilePosition);
		Vector3 roomTopRightWorld = groundTilemap.CellToWorld(topRightTilePosition);

		roomTopRightWorld += new Vector3(groundTilemap.cellSize.x, groundTilemap.cellSize.y, 0);

		// 在世界坐标中检查玩家位置
		return playerPosition.x >= roomBottomLeftWorld.x &&
			   playerPosition.x <= roomTopRightWorld.x &&
			   playerPosition.y >= roomBottomLeftWorld.y &&
			   playerPosition.y <= roomTopRightWorld.y;
	}




	IEnumerator GenerateMapCoroutine()
	{
		mapGenerateProcess = 0;
		map = new GridType[width, height];
		// 初始化map为全-1
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				map[x, y] = GridType.VOID;
			}
		}
		wallTilemap.ClearAllTiles();
		groundTilemap.ClearAllTiles();

		if (isServer)
		{
			rooms = GenerateRooms(roomNums);
		}


		mapGenerateProcess = 10;
		foreach (var room in rooms)
		{

			RandomFillMap(room.bottomLeft, room.topRight);

			for (int i = 0; i < 5; i++)
			{
				SmoothMap(room.bottomLeft, room.topRight);
			}
			FillTilemap(room.bottomLeft, room.topRight);

			//PrintMap();
			mapGenerateProcess += 80.0f / rooms.Count;
			yield return new WaitForSeconds(0.1f); // 暂停0.1秒
		}

		mapGenerateProcess = 90;
		//在所有房间生成完毕后连接它们
		ConnectRooms();

		farRoom = FindFurthestRoomFromRoom0(rooms);

		var waitLocalPlayerTime = 200;
		var waitLocalPlayerFlag = false;
		while (waitLocalPlayerTime-- > 0)
		{
			if (playerManager.localPlayer != null)
			{
				var center = rooms[0].Center;
				var worldPostion = groundTilemap.CellToWorld(new Vector3Int(center.x, center.y, 0));
				Debug.Log("local player spawn at:\n" + worldPostion.ToString() + "\n" + map[center.x, center.y].ToString());
				playerManager.localPlayer.transform.position = worldPostion;
				waitLocalPlayerFlag = true;
				break;
			}
			yield return new WaitForSeconds(0.1f); // 暂停0.1秒
		}
		if (!waitLocalPlayerFlag)
			Debug.Log("Fail to initialize player location");


		mapGenerateProcess = 100;
		isFinish = true;

		mapGridsRenderer.GenMap(map);
		// PrintMap();
	}

	void PrintMap()
	{
		string mapString = "";
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				mapString += (int)map[x, y] + " ";
			}
			mapString += "\n";
		}
		Debug.Log(mapString);
	}

	void RandomFillMap(Vector2Int bottomLeft, Vector2Int topRight)
	{

		for (int x = bottomLeft.x; x < topRight.x; x++)
		{
			for (int y = bottomLeft.y; y < topRight.y; y++)
			{
				if (x == bottomLeft.x || x == topRight.x - 1 || y == bottomLeft.y || y == topRight.y - 1)
				{
					map[x, y] = GridType.WALL;
				}
				else
				{
					map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? GridType.WALL : GridType.FLOOR;
				}
			}
		}

		privateSeed = (double.Parse(privateSeed) * 2).ToString();//调用一次就*2，这样只需在start初始化一次种子
	}

	void SmoothMap(Vector2Int bottomLeft, Vector2Int topRight)
	{
		for (int x = bottomLeft.x; x < topRight.x; x++)
		{
			for (int y = bottomLeft.y; y < topRight.y; y++)
			{
				if (x == bottomLeft.x || x == topRight.x - 1 || y == bottomLeft.y || y == topRight.y - 1)
					continue;

				int neighbourWallTiles = GetSurroundingWallCount(x, y);

				if (neighbourWallTiles > 4)
					map[x, y] = GridType.WALL;
				else if (neighbourWallTiles < 4)
					map[x, y] = GridType.FLOOR;
			}
		}
	}

	void FillTilemap(Vector2Int bottomLeft, Vector2Int topRight)
	{
		for (int x = bottomLeft.x; x < topRight.x; x++)
		{
			for (int y = bottomLeft.y; y < topRight.y; y++)
			{
				//TODO: fix tile pos
				Vector3Int position = new Vector3Int(x, y, 0);
				if (map[x, y] == GridType.WALL) // 如果当前位置不是地板
				{
					if (IsBorder(x, y))
					{
						// 如果当前位置是边缘，则放置墙壁
						// wallTilemap.SetTile(position, null); // 首先清除该位置上可能存在的瓦片
						wallTilemap.SetTile(position, wallTile); // 然后放置新的墙壁瓦片
					}

				}
				if (map[x, y] == GridType.FLOOR)
				{
					// 放置地板
					//     // wallTilemap.SetTile(position, null);
					//     // groundTilemap.SetTile(position, null); // 首先清除该位置上可能存在的瓦片
					groundTilemap.SetTile(position, floorTile); // 然后放置新的地板瓦片
				}

			}
		}
	}

	public bool IsBorder(int x, int y)
	{
		if (map[x, y] == GridType.FLOOR) return false; // 当前位置如果是地板，则直接返回false

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
					if (map[nx, ny] == GridType.FLOOR) // 如果相邻位置是地板，则当前位置是边缘
					{
						return true;
					}
				}
			}
		}
		return false; // 如果周围没有地板，则不是边缘
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
						wallCount += (map[neighbourX, neighbourY] == GridType.WALL ? 1 : 0);
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
		// 使用Prim算法生成最小生成树
		if (rooms.Count < 2) return;

		bool[] inMST = new bool[rooms.Count];
		float[] key = new float[rooms.Count];
		int[] parent = new int[rooms.Count];

		for (int i = 0; i < rooms.Count; i++)
		{
			key[i] = float.MaxValue;
			inMST[i] = false;
		}

		key[0] = 0;
		parent[0] = -1; // 第一个点没有父节点

		for (int count = 0; count < rooms.Count - 1; count++)
		{
			// 选取未在MST中且键值最小的顶点u
			float min = float.MaxValue;
			int u = -1;

			for (int v = 0; v < rooms.Count; v++)
			{
				if (!inMST[v] && key[v] < min)
				{
					min = key[v];
					u = v;
				}
			}

			inMST[u] = true;

			for (int v = 0; v < rooms.Count; v++)
			{
				// 对每个顶点，如果v未在MST中且u-v权重小于v的key值，则更新v的parent为u，key值为该权重
				float weight = Vector2.Distance(rooms[u].Center, rooms[v].Center);
				if (!inMST[v] && weight < key[v])
				{
					parent[v] = u;
					key[v] = weight;
				}
			}
		}

		// 根据生成的最小生成树连接房间
		for (int i = 1; i < rooms.Count; i++)
		{
			GeneratePath(rooms[parent[i]].Center, rooms[i].Center);
		}
	}

	// void GeneratePath(Vector2Int start, Vector2Int end)
	// {
	// 	Vector2Int current = start;

	// 	// 先水平后垂直移动
	// 	while (current.x != end.x)
	// 	{
	// 		SetFloorAndWalls(current, true); // true 表示水平移动
	// 		current.x += (end.x > current.x) ? 1 : -1;
	// 	}

	// 	while (current.y != end.y)
	// 	{
	// 		SetFloorAndWalls(current, false); // false 表示垂直移动
	// 		current.y += (end.y > current.y) ? 1 : -1;
	// 	}
	// }

	void GeneratePath(Vector2Int start, Vector2Int end)
	{
		Vector2Int current = start;
		bool horizontal = Random.Range(0, 2) == 0; // 初始随机方向，true为水平，false为垂直

		while (current != end)
		{
			// 在每次循环开始时随机决定是否改变方向
			if (Random.Range(0, 100) < 50) // 有30%的概率改变方向
			{
				horizontal = !horizontal;
			}

			// 水平或垂直移动
			if (horizontal)
			{
				SetFloorAndWalls(current, true);
				// SetFloorAndWalls(new Vector2Int(current.x, current.y + 1), true); // 宽度为2的路径
				current.x += (end.x > current.x) ? 1 : -1;
			}
			else
			{
				SetFloorAndWalls(current, false);
				// SetFloorAndWalls(new Vector2Int(current.x + 1, current.y), false); // 宽度为2的路径
				current.y += (end.y > current.y) ? 1 : -1;
			}

			// 检查是否达到目标点的一条直线上
			if (current.x == end.x) horizontal = false;
			if (current.y == end.y) horizontal = true;
		}
	}

	void SetFloorAndWalls(Vector2Int position, bool isHorizontal)
	{
		// 设置地板
		wallTilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
		groundTilemap.SetTile(new Vector3Int(position.x, position.y, 0), floorTile);
		map[position.x, position.y] = GridType.FLOOR;

		// 根据移动方向设置墙壁
		Vector2Int[] directions = {
		new Vector2Int(0, 1), // 上
        new Vector2Int(1, 0), // 右
        new Vector2Int(0, -1), // 下
        new Vector2Int(-1, 0) // 左
    };
		// 设置墙壁
		foreach (var dir in directions)
		{
			Vector3Int wallPos = new Vector3Int(position.x + dir.x, position.y + dir.y, 0);
			if (map[wallPos.x, wallPos.y] != GridType.FLOOR)
			{
				map[wallPos.x, wallPos.y] = GridType.WALL;
				wallTilemap.SetTile(wallPos, wallTile);
			}
		}

		// 额外设置拐角的墙壁，以确保完全覆盖
		SetCornerWalls(position);
	}

	void SetCornerWalls(Vector2Int position)
	{
		// 检查当前位置四个角的墙壁是否需要设置
		Vector2Int[] cornerDirections = { new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1) };
		foreach (var dir in cornerDirections)
		{
			Vector3Int cornerPos = new Vector3Int(position.x + dir.x, position.y + dir.y, 0);
			if (map[cornerPos.x, cornerPos.y] != GridType.FLOOR)
			{
				map[cornerPos.x, cornerPos.y] = GridType.WALL;
				wallTilemap.SetTile(cornerPos, wallTile);
			}
		}
	}

	// bool IsInBounds(Vector3Int pos)
	// {
	// 	return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
	// }



	// void SetFloorAndWalls(Vector2Int position, bool isHorizontal)
	// {
	// 	// 设置中间的路为地板
	// 	wallTilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
	// 	groundTilemap.SetTile(new Vector3Int(position.x, position.y, 0), floorTile);
	// 	map[position.x, position.y] = GridType.FLOOR;

	// 	// 设置当前方向的墙壁
	// 	Vector2Int[] directions = {
	// 	new Vector2Int(0, 1), // 上
	// 	new Vector2Int(1, 0), // 右
	// 	new Vector2Int(0, -1), // 下
	// 	new Vector2Int(-1, 0) // 左
	// 	};

	// 	foreach (var dir in directions)
	// 	{
	// 		Vector3Int wallPos = new Vector3Int(position.x + dir.x, position.y + dir.y, 0);
	// 		if (wallPos.x >= 0 && wallPos.x < width && wallPos.y >= 0 && wallPos.y < height)
	// 		{
	// 			if (map[wallPos.x, wallPos.y] != GridType.FLOOR) // 如果不是地板，则设置为墙壁
	// 			{
	// 				map[wallPos.x, wallPos.y] = GridType.WALL; // 墙壁标记为1
	// 				wallTilemap.SetTile(wallPos, wallTile);
	// 			}
	// 		}
	// 	}

	// 	// 额外设置拐角的墙壁，以确保完全覆盖
	// 	SetCornerWalls(position);
	// }



	// void SetCornerWalls(Vector2Int position)
	// {
	// 	Vector2Int[] cornerDirections = {
	// 	new Vector2Int(1, 1),   // 右上
	// 	new Vector2Int(1, -1),  // 右下
	// 	new Vector2Int(-1, -1), // 左下
	// 	new Vector2Int(-1, 1)   // 左上
	// };

	// 	foreach (var dir in cornerDirections)
	// 	{
	// 		Vector3Int cornerPos = new Vector3Int(position.x + dir.x, position.y + dir.y, 0);
	// 		if (cornerPos.x >= 0 && cornerPos.x < width && cornerPos.y >= 0 && cornerPos.y < height)
	// 		{
	// 			if (map[cornerPos.x, cornerPos.y] != GridType.FLOOR) // 如果不是地板，则设置为墙壁
	// 			{
	// 				map[cornerPos.x, cornerPos.y] = GridType.WALL; // 墙壁标记为1
	// 				wallTilemap.SetTile(cornerPos, wallTile);
	// 			}
	// 		}
	// 	}
	// }

	[System.Serializable]
	public class Room
	{
		public Vector2Int bottomLeft; // 房间左下角坐标
		public Vector2Int topRight;   // 房间右上角坐标
		string seed;

		public Room()
		{
		}

		public Room(Vector2Int _bottomLeft, Vector2Int _topRight, string _seed)
		{
			bottomLeft = _bottomLeft;
			topRight = _topRight;
			seed = _seed;
		}

		public Room(Vector2Int _bottomLeft, Vector2Int _topRight)
		{
			bottomLeft = _bottomLeft;
			topRight = _topRight;
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

