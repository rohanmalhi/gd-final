using System;
using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRender;
    public int seed;

    private float[,] GenerateNoiseMap()
    {
        // Create and configure FastNoise object
        FastNoiseLite noise = new FastNoiseLite(seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFrequency(0.025f);
        noise.SetFractalType(FastNoiseLite.FractalType.DomainWarpIndependent);
        // noise.SetDomainWarpAmp(200f);
        // noise.SetFractalOctaves(3);
        // noise.SetFractalLacunarity(2f);
        // noise.SetFractalGain(0.5f);

        // Gather noise data
        float[,] noiseData = new float[30, 30];
        float minVal = float.MaxValue;
        float maxVal = float.MinValue;
        
        for (int y = 0; y < 30; y++)
        {
            for (int x = 0; x < 30; x++)
            {
                noiseData[y, x] = (noise.GetNoise(x, y) + 1) / 2;
                if (noiseData[y, x] < minVal)
                {
                    minVal = noiseData[y, x];
                }
                else if (noiseData[y, x] > maxVal)
                {
                    maxVal = noiseData[y, x];
                }
            }
        }
        print(minVal);
        print(maxVal);

        return noiseData;
    }
    
    public float GaussianFunction10(float x, float y)
    {
        return 1.1f - (0.3f + 0.8f * (1f - Mathf.Exp(-((x * x + y * y) / 10000.0f))));
    }
    
    public void DrawNoiseMap(float[,] noiseMap) {
        int width = noiseMap.GetLength (0);
        int height = noiseMap.GetLength (1);

        Texture2D texture = new Texture2D (width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap [x, y]);
            }
        }
        texture.SetPixels (colourMap);
        texture.Apply ();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3 (width, 1, height);
    }

    public void OnValidate()
    {
        DrawNoiseMap(GenerateNoiseMap());
    }
}