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
                float height = Mathf.PerlinNoise((x/2f + xOffset) * .2f, (z/2f + zOffset) * .2f) * 10 + 10 * 3;

                for (int y = 0; y < height; y++)
                {
                    result[x, y, z] = BlockType.Grass;
                }
            }
        }
        return result;
    }
}
