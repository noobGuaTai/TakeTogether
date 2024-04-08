using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using static MapGenerator;

public class MapGridsRenderer : MonoBehaviour
{
   
    public struct InstanceData {
        public Vector2 pos;
        public int type;
        public InstanceData(Vector2 pos, int type) {
            this.pos =  pos;
            this.type = type;      
        }
        public static int Size() {
            return 2 * sizeof(float) + sizeof(int);
        }
    }

    private Mesh grid = null;
    public int gridLength = 16;
    public InstanceData[] instanceData;
    public RenderParams renderParams;
    public int totX;
    public int totY;
    public Material material;
    public Matrix4x4[] matrixs;
    public Bounds bounds;
    public ComputeBuffer materialBuffer;
    public ComputeBuffer argsBuffer;
    public uint[] argsBufferArray = new uint[5] { 0, 0, 0, 0, 0 };
    public GameObject player = null;
    public Grid gridComponent = null;
    public MapGenerator mapGenerator = null;
    float2 blPos;
    float2 trPos;
    // Start is called before the first frame update

    int GetIndex(int x, int y) {
        return y * totX + x;
    }

    void GenGrid() {
        if (grid != null)
            return;
        grid = new Mesh();
        grid.vertices = new Vector3[]{
            new Vector3( 0, 0, 0),
            new Vector3( gridLength, 0, 0),
            new Vector3( gridLength, gridLength, 0),
            new Vector3( 0, gridLength, 0), };
        grid.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
    }
    void Start()
    {
        gameObject.SetActive(false);
        gridComponent = transform.Find("/Grid").GetComponent<Grid>();
        // mapGenerator = transform.Find("/MapGenerator").GetComponent<MapGenerator>();
    }

    public void GenMap(GridType[,] map)
    {
        var totX = map.GetLength(0);
        var totY = map.GetLength(1);

        GenGrid();
        this.totX = totX;  this.totY = totY;
        instanceData = new InstanceData[totX * totY];
        bounds = new Bounds(transform.position, new Vector3(100000, 100000, 100000));
        materialBuffer = new ComputeBuffer(totX * totY, InstanceData.Size());
        var totSize = new Vector2(totX, totY) * gridLength;
        blPos = new Vector2(0, 0) * gridLength - 0.5f * totSize;
        trPos = new Vector2(totX, totY) * gridLength - 0.5f * totSize;

        for (int i = 0; i < totX; i++) { 
            for(int j = 0; j < totY; j++)
            {
                var pos = new Vector2(i, j) * gridLength - 0.5f * totSize;
                var type = (int)map[i, j];
                var index = GetIndex(i, j);
                instanceData[index].pos = pos;
                instanceData[index].type = type;
            }
        }
        materialBuffer.SetData(instanceData);
        material.SetBuffer("instanceData", materialBuffer);
        material.SetMatrix("local2Wolrd", transform.localToWorldMatrix);
        material.SetVector("_border", new Vector4(blPos.x, blPos.y, trPos.x, trPos.y));
        Debug.Log("blPos:" + material.GetFloatArray("blPos"));

        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBufferArray[0] = (uint)grid.GetIndexCount(0);
        argsBufferArray[1] = (uint)(this.totX * this.totY);
        argsBufferArray[2] = (uint)grid.GetIndexStart(0);
        argsBufferArray[3] = (uint)grid.GetBaseVertex(0);
        argsBuffer.SetData(argsBufferArray);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.Find("/MapGenerator").GetComponent<MapGenerator>() != null)
        {
            mapGenerator = transform.Find("/MapGenerator").GetComponent<MapGenerator>();
        }

        if (grid != null)
        {
            material.SetMatrix("local2World", transform.localToWorldMatrix);

            
            if (player == null)
            {
                player = transform.Find("/PlayerManager").GetComponent<PlayerManager>().localPlayer;
            }
            if (player != null)
            {
                float3 playerWorldPos = player.transform.position;
                float3 gridSize = mapGenerator.gridSize;
                float3 playerMapPos = playerWorldPos / gridSize * gridLength 
                    + new float3(blPos, 0);
                float2 playerMapUV = (playerMapPos.xy - blPos) / (trPos - blPos);

                material.SetVector(
                    "_playerMapUV", new float4(playerMapUV, 0, 0));
            }
            Graphics.DrawMeshInstancedIndirect(grid, 0, material, bounds, argsBuffer, 0, 
                null, UnityEngine.Rendering.ShadowCastingMode.Off, false, gameObject.layer);
        }
    }
}
