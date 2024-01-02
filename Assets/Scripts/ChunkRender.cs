using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRender : MonoBehaviour
{
    [SerializeField] Material material;

    private const int chunkWidth = 10;
    private const int chunkHeight = 10;

    private int[,,] blocks = new int[chunkWidth, chunkHeight, chunkWidth];

    private List<Vector3> verticies = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        Mesh chunkMesh = new Mesh();

        blocks[0, 0, 0] = 1;
        blocks[0, 0, 1] = 1;

        for (int y = 0; y < chunkHeight; y++)
        {
            for (int x = 0; x < chunkWidth; x++)
            {
                for (int z = 0; z < chunkWidth; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }

        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        Vector3Int blockPosition = new Vector3Int(x, y, z);

        if (GetBlockAtPosition(blockPosition) == 0) return;

        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) GenerateRightSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0) GenerateLeftSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0) GenerateFrontSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0) GenerateBackSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) GenerateTopSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0) GenerateBottomSide(blockPosition);

        //GenerateRightSide(blockPosition);
        //GenerateLeftSide(blockPosition);
        //GenerateFrontSide(blockPosition);
        //GenerateBackSide(blockPosition);
        //GenerateTopSide(blockPosition);
        //GenerateBottomSide(blockPosition);
    }

    private int GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < chunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < chunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < chunkWidth)
        {
            return blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            return 0;
        }
    }

    private void GenerateRightSide(Vector3Int blockPosition)
    {
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerciciesSquare();
    }

    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);

        AddLastVerciciesSquare();
    }

    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerciciesSquare();
    }

    private void GenerateBackSide(Vector3Int blockPosition)
    {
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);

        AddLastVerciciesSquare();
    }

    private void GenerateTopSide(Vector3Int blockPosition)
    {
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerciciesSquare();
    }

    private void GenerateBottomSide(Vector3Int blockPosition)
    {
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);

        AddLastVerciciesSquare();
    }

    private void AddLastVerciciesSquare()
    {
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
        triangles.Add(verticies.Count - 3);
    }
}
