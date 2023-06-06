using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDensity : DensityGenerator {

    [Header ("Noise")]
    public int seed;
    public int numOctaves = 4;
    public float lacunarity = 2;
    public float persistence = .5f;
    public float noiseScale = 1;
    public float noiseWeight = 1;
    public bool closeEdges;
    public float floorOffset = 1;
    public float weightMultiplier = 1;
    public float radius;

    public float hardFloorHeight;
    public float hardFloorWeight;

    public Vector4 shaderParams;

    public override ComputeBuffer Generate (ComputeBuffer pointsBuffer, int numPointsPerAxis, Vector3 boundsSize, 
        Vector3 worldBounds, Vector3 centre, Vector3 offset, Vector3 spacing) {
        densityShader.SetBool("terraform", false);
        buffersToRelease = new List<ComputeBuffer> ();

        // Noise parameters
        var prng = new System.Random (seed);
        var offsets = new Vector3[numOctaves];
        float offsetRange = 1000;
        for (int i = 0; i < numOctaves; i++) {
            offsets[i] = new Vector3 ((float) prng.NextDouble () * 2 - 1, (float) prng.NextDouble () * 2 - 1, (float) prng.NextDouble () * 2 - 1) * offsetRange;
        }

        var offsetsBuffer = new ComputeBuffer (offsets.Length, sizeof (float) * 3);
        offsetsBuffer.SetData (offsets);
        buffersToRelease.Add (offsetsBuffer);

        densityShader.SetVector ("centre", new Vector4 (centre.x, centre.y, centre.z));
        densityShader.SetInt ("octaves", Mathf.Max (1, numOctaves));
        densityShader.SetFloat ("lacunarity", lacunarity);
        densityShader.SetFloat ("persistence", persistence);
        densityShader.SetFloat ("noiseScale", noiseScale);
        densityShader.SetFloat ("noiseWeight", noiseWeight);
        densityShader.SetBool ("closeEdges", closeEdges);
        densityShader.SetBuffer (0, "offsets", offsetsBuffer);
        densityShader.SetFloat ("floorOffset", floorOffset);
        densityShader.SetFloat ("weightMultiplier", weightMultiplier);
        densityShader.SetFloat ("hardFloor", hardFloorHeight);
        densityShader.SetFloat ("hardFloorWeight", hardFloorWeight);
        densityShader.SetFloat ("radius", radius);

        densityShader.SetVector ("params", shaderParams);

        return base.Generate (pointsBuffer, numPointsPerAxis, boundsSize, worldBounds, centre, offset, spacing);
    }
    
    public override void Terraform (ComputeBuffer pointsBuffer, int numPointsPerAxis, Vector3 boundsSize, 
        Vector3 worldBounds, Vector3 centre, Vector3 offset, Vector3 spacing, Vector3 terraformPos, int terraformWeight) {
        Vector3 tempPos = terraformPos + boundsSize/2 - centre;
        
        int terraX = Mathf.RoundToInt(tempPos.x / spacing.x);
        int terraY = Mathf.RoundToInt((tempPos.y / spacing.y));
        int terraZ = Mathf.RoundToInt(tempPos.z / spacing.z);
        
        
        terraformShader.SetInts("terraformPos", terraX, terraY, terraZ);
        terraformShader.SetBuffer (0, "points", pointsBuffer);
        terraformShader.SetInt ("numPointsPerAxis", numPointsPerAxis);
        terraformShader.SetInt("terraformWeight", terraformWeight);
        
        int numThreadsPerAxis = Mathf.CeilToInt (numPointsPerAxis / (float) 8);
        // Dispatch shader
        terraformShader.Dispatch (0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        if (buffersToRelease != null) {
            foreach (var b in buffersToRelease) {
                b.Release();
            }
        }
    }
    
    public static uint FNVHash(string str)
    {
        const uint fnvPrime = 0x811C9DC5;
        uint hash = 0;
        uint i;

        for (i = 0; i < str.Length; i++)
        {
            hash *= fnvPrime;
            hash ^= (byte)str[(int)i];
        }

        return hash;
    }
}