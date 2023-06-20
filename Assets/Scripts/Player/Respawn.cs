using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(0, 20, 0);
        }
    }
}
