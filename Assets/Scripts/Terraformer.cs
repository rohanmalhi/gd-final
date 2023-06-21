using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terraformer : MonoBehaviour
{
    public LayerMask canTerraform;
    public MeshGenerator meshGenerator;
    public Builder Builder;
    public AudioSource sound;
    public ParticleSystem particleSystem;
    
    public Transform orientation;
    void Update()
    {
        // Debug.DrawRay(orientation.position, orientation.forward, Color.blue);
        if (Physics.Raycast(orientation.transform.position, orientation.transform.forward, 
                out RaycastHit hit, 200, canTerraform))
        {
            Terraform(hit);
        }
    }

    private void Terraform(RaycastHit hit)
    {
        if (Builder.BuildMode) return;
        int weight;
        if (Input.GetMouseButton(1) && !hit.collider.gameObject.tag.Equals("Player"))
        {
            particleSystem.transform.position = hit.point;
            particleSystem.Play();
            if (Vector3.Distance(hit.point, orientation.position) < 2)
            {
                return;
            }

            weight = 1;
            UpdateTerraform(hit, weight);
        }
    
        else if (Input.GetMouseButton(0) && !hit.collider.gameObject.tag.Equals("Player"))
        {
            particleSystem.transform.position = hit.point;
            particleSystem.Play();
            weight = -1;
            UpdateTerraform(hit, weight);
        }
    }

    private void UpdateTerraform(RaycastHit hit, int weight)
    {
        meshGenerator.UpdateTerraformChunks(hit.point, weight);
        /*
        Collider[] results = new Collider[10];

        int size = Physics.OverlapSphereNonAlloc(hit.point, 15, results);

        for (int i = 0; i < size; i++)
        {
            if (!results[i].tag.Equals("Player") && !results[i].tag.Equals("Build"))
            {
                meshGenerator.UpdateChunkMesh(results[i].GetComponent<Chunk>(), hit.point, weight);
            }
        }
        */
    }
}
