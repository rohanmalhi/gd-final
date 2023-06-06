using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Transform orientation;
    public GameObject buildBlock;
    public GameObject ghostBlock;

    public bool BuildMode { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildMode = !BuildMode;
            ghostBlock.SetActive(false);
        }

        if (!BuildMode) return;
        if (Physics.Raycast(orientation.position, orientation.forward, out var hit, 10) 
            && hit.transform.tag.Equals("Build"))
        {
            Vector3 hitNormal = hit.normal;
            Transform hitObject = hit.transform;

            // Instantiate a new cube at the hit point
            ghostBlock.transform.position = hitObject.position + hitNormal;
            ghostBlock.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(1))
            {
                // Instantiate a new cube at the hit point
                GameObject newCube = Instantiate(buildBlock, ghostBlock.transform.position, Quaternion.identity);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            ghostBlock.gameObject.SetActive(false);
            if (Input.GetMouseButtonDown(1))
            {
                Instantiate(buildBlock, orientation.position + orientation.forward * 5, Quaternion.identity);
            }
        }
    }
}
