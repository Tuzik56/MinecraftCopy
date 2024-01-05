using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public Dictionary<Vector2Int, ChunkData> chunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChunkRender ChunkPrefab;

    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                int xPosition = x * ChunkRender.chunkWidth;
                int zPosition = z * ChunkRender.chunkWidth;

                var chunkData = new ChunkData();
                chunkData.chunkPosition = new Vector2Int(x, z);
                chunkData.blocks = TerrainGenerator.GenerateTerrain(x, z);

                chunkDatas.Add(new Vector2Int(x, z), chunkData);

                var chunk = Instantiate(ChunkPrefab, new Vector3Int(xPosition, 0, zPosition), Quaternion.identity, transform);
                chunk.chunkData = chunkData;
                chunk.parentWorld = this;
            }
        }
    }

    void Update()
    {
        
    }
}
