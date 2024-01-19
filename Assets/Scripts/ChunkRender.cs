using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkRender : MonoBehaviour
{
    public const int chunkWidth = 16;
    public const int chunkHeight = 128;

    public ChunkData chunkData;
    public GameWorld parentWorld;

    private Mesh chunkMesh;

    private List<Vector3> verticies = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        chunkMesh = new Mesh();

        RegenerateMesh();

        GetComponent<MeshFilter>().mesh = chunkMesh;
    }

    private void RegenerateMesh()
    {
        verticies.Clear();
        uvs.Clear();
        triangles.Clear();

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

        chunkMesh.triangles = Array.Empty<int>();
        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    public void SpawnBlock(Vector3Int blockPosition)
    {
        chunkData.blocks[blockPosition.x, blockPosition.y, blockPosition.z] = BlockType.Grass;
        RegenerateMesh();
    }

    public void DestroyBlock(Vector3Int blockPosition)
    {
        chunkData.blocks[blockPosition.x, blockPosition.y, blockPosition.z] = BlockType.Air;
        RegenerateMesh();
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
    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < chunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < chunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < chunkWidth)
        {
            return chunkData.blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            if (blockPosition.y < 0 || blockPosition.y > chunkHeight) return BlockType.Air;

            Vector2Int adjacentChunkPosition = chunkData.chunkPosition;

            if (blockPosition.x < 0)
            {
                adjacentChunkPosition.x--;
                blockPosition.x += chunkWidth;
            }
            else if (blockPosition.x >= chunkWidth)
            {
                adjacentChunkPosition.x++;
                blockPosition.x -= chunkWidth;
            }
            else if (blockPosition.z < 0)
            {
                adjacentChunkPosition.y--;
                blockPosition.z += chunkWidth;
            }
            else if (blockPosition.z >= chunkWidth)
            {
                adjacentChunkPosition.y++;
                blockPosition.z -= chunkWidth;
            }

            if (parentWorld.chunkDatas.TryGetValue(adjacentChunkPosition, out ChunkData adjacentChunk))
            {
                return adjacentChunk.blocks[blockPosition.x, blockPosition.y, blockPosition.z];
            }
        }
        return BlockType.Air;
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
        uvs.Add(new Vector2(64f/256, 240f/256));
        uvs.Add(new Vector2(64f/256, 1));
        uvs.Add(new Vector2(80f/256, 240f / 256));
        uvs.Add(new Vector2(80f/256, 1));

        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
        triangles.Add(verticies.Count - 3);
    }
}
