using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridsRenderer : MonoBehaviour
{
   
    public struct VertexData {
        MapGenerator.GridType gridType;
    }

    private Mesh grid;
    public int gridLength = 16;
    public VertexData[] instanceData;
    public RenderParams renderParams;
    public int totX;
    public int totY;

    // Start is called before the first frame update

    int GetIndex(int x, int y) {
        return y * totX + x;
    }

    void GenGrid() {
        grid = new Mesh();
        grid.vertices = new Vector3[]{
            new Vector3( 0, 0, 0),
            new Vector3( gridLength, 0, 0),
            new Vector3( gridLength, gridLength, 0),
            new Vector3( 0, gridLength, 0), };
        grid.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    }
    void Start()
    {
        GenGrid();
       
    }

    void GenMap(int totX, int totY)
    {
        this.totX = totX;  this.totY = totY;
        instanceData = new VertexData[totX * totY];
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.RenderMeshInstanced(renderParams, grid, 0, instanceData);
    }
}
