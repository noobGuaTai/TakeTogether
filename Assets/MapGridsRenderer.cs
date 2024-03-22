using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        
    }

    public void GenMap(int totX, int totY)
    {
        GenGrid();
        this.totX = totX;  this.totY = totY;
        instanceData = new InstanceData[totX * totY];
        bounds = new Bounds(transform.position, new Vector3(100000, 100000, 100000));
        materialBuffer = new ComputeBuffer(totX * totY, InstanceData.Size());

        for (int i = 0; i < totX; i++) { 
            for(int j = 0; j < totY; j++)
            {
                var pos = new Vector2(i, j) * 20;
                var type = 0;
                var index = GetIndex(i, j);
                instanceData[index].pos = pos;
                instanceData[index].type = type;
            }
        }
        materialBuffer.SetData(instanceData);
        material.SetBuffer("instanceData", materialBuffer);

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
        if (grid != null)
        {
            Graphics.DrawMeshInstancedIndirect(grid, 0, material, bounds, argsBuffer, 0, 
                null, UnityEngine.Rendering.ShadowCastingMode.Off, false, gameObject.layer);
        }
    }
}
