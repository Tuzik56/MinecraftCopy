using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public Dictionary<Vector2Int, ChunkData> chunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChunkRender ChunkPrefab;

    private Camera mainCamera;
    private Transform cameraTransform;

    void Start()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;

        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                int xPosition = x * ChunkRender.chunkWidth;
                int zPosition = z * ChunkRender.chunkWidth;

                var chunkData = new ChunkData();
                chunkData.chunkPosition = new Vector2Int(x, z);
                chunkData.blocks = TerrainGenerator.GenerateTerrain(xPosition, zPosition);

                chunkDatas.Add(new Vector2Int(x, z), chunkData);

                var chunk = Instantiate(ChunkPrefab, new Vector3Int(xPosition, 0, zPosition), Quaternion.identity, transform);
                chunk.chunkData = chunkData;
                chunk.parentWorld = this;

                chunkData.render = chunk;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (Physics.Raycast(ray, out var hitInfo))
            {
                Vector3 blockCenter = Input.GetMouseButtonDown(0) ? hitInfo.point - hitInfo.normal / 2 : hitInfo.point + hitInfo.normal / 2;
                Vector3Int blockWorldPosition = Vector3Int.FloorToInt(blockCenter);
                Vector2Int chunkPosition = GetChunkContainingBlock(blockWorldPosition);

                if (chunkDatas.TryGetValue(chunkPosition, out ChunkData chunkData))
                {
                    Vector3Int chunkOrigin = new Vector3Int(chunkPosition.x, 0, chunkPosition.y) * ChunkRender.chunkWidth;

                    if (Input.GetMouseButtonDown(0))
                    {
                        chunkData.render.DestroyBlock(blockWorldPosition - chunkOrigin);
                    }
                    else
                    {
                        chunkData.render.SpawnBlock(blockWorldPosition - chunkOrigin);
                    }
                }
            }
        }
    }

    public Vector2Int GetChunkContainingBlock(Vector3Int blockWorldPosition)
    {
        return new Vector2Int(blockWorldPosition.x / ChunkRender.chunkWidth, blockWorldPosition.z / ChunkRender.chunkWidth);
    }
}
