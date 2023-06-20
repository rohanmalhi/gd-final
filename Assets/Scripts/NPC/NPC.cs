using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position - new Vector3(25, 0, 25);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + new Vector3(0, player.position.y, 0);
        transform.LookAt(player);
        transform.Translate(Vector3.forward * Time.deltaTime * 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            
        }
    }
}
