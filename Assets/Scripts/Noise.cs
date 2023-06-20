using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, 
        int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        System.Random pseudoRng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = pseudoRng.Next(-100000, 100000) + offset.x;
            float offsetY = pseudoRng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float[,] noiseMap = new float[mapWidth, mapHeight];
        // check if scale is less than or equal to 0
        scale = scale <= 0 ? 0.001f : scale;
        
        // get the half of map's width and height
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                
                for (int i = 0; i < octaves; i++)
                {            
                    // create non-integer inputs to the perlin noise 
                    float sampleX = (x - halfWidth) / scale * frequency  + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency  + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // normalizes values between 0 and 1, method defined in utils.cs
        noiseMap.Normalize();

        return noiseMap;
    }
}
