using UnityEngine;

public static class TerrainGenerator
{
    public static BlockType[,,] GenerateTerrain(int xOffset, int zOffset)
    {
        var result = new BlockType[ChunkRender.chunkWidth, ChunkRender.chunkHeight, ChunkRender.chunkWidth];

        for (int x = 0; x < ChunkRender.chunkWidth; x++)
        {
            for (int z = 0;  z < ChunkRender.chunkWidth; z++)
            {
                float height = Mathf.PerlinNoise((x + xOffset) * .2f, (z + zOffset) * .2f) * 5 + 10 * 3;

                for (int y = 0; y < height; y++)
                {
                    result[x, y, z] = BlockType.Grass;
                }
            }
        }
        return result;
    }
}
