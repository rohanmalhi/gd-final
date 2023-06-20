using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Ground Check")] 
    public LayerMask ground;
    [SerializeField] private float playerHeight = 0.03f;
    private bool _grounded;
    private readonly float _groundDistance = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Launcher") != true)
        {
            DestroyObject(gameObject);
        }
        
    }
}
