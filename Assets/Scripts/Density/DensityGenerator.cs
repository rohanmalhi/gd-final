using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DensityGenerator : MonoBehaviour {

    const int threadGroupSize = 8;
    public ComputeShader densityShader;
    public ComputeShader terraformShader;

    protected List<ComputeBuffer> buffersToRelease;

    void OnValidate() {
        if (FindObjectOfType<MeshGenerator>()) {
            FindObjectOfType<MeshGenerator>().RequestMeshUpdate();
        }
    }

    public virtual ComputeBuffer Generate (ComputeBuffer pointsBuffer, int numPointsPerAxis, Vector3 boundsSize, 
        Vector3 worldBounds, Vector3 centre, Vector3 offset, Vector3 spacing) {
        int numPoints = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        int numThreadsPerAxis = Mathf.CeilToInt (numPointsPerAxis / (float) threadGroupSize);
        // Points buffer is populated inside shader with pos (xyz) + density (w).
        // Set paramaters
        densityShader.SetBuffer (0, "points", pointsBuffer);
        densityShader.SetInt ("numPointsPerAxis", numPointsPerAxis);
        densityShader.SetVector("boundsSize", new Vector4(boundsSize.x, boundsSize.y, boundsSize.z));
        densityShader.SetVector ("centre", new Vector4 (centre.x, centre.y, centre.z));
        densityShader.SetVector ("offset", new Vector4 (offset.x, offset.y, offset.z));
        densityShader.SetVector("spacing", new Vector4(spacing.x, spacing.y, spacing.z));
        densityShader.SetVector("worldSize", worldBounds);
        // densityShader.SetBool("terraform", false);

        // Dispatch shader
        densityShader.Dispatch (0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        if (buffersToRelease != null) {
            foreach (var b in buffersToRelease) {
                b.Release();
            }
        }

        // Return voxel data buffer so it can be used to generate mesh
        return pointsBuffer;
    }
    
    public virtual void Terraform(ComputeBuffer pointsBuffer, int numPointsPerAxis, Vector3 boundsSize, 
        Vector3 worldBounds, Vector3 centre, Vector3 offset, Vector3 spacing, Vector3 terraformPos,int terraformWeight)
    {
        
    }
    
}