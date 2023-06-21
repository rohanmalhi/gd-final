using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEditor;

public class TreeSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform treeHolder;
    private GameObject _instantiatedObject;
    private List<GameObject> _objectsInScene = new List<GameObject>();
    
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius =1;
    public bool draw;
    
    List<Vector2> points;

    void OnValidate() {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }
    

    void OnDrawGizmos() {
        if (!draw) return;
        Gizmos.DrawWireCube(regionSize/2,regionSize);
        if (points != null) {
            foreach (Vector2 point in points) {
                Gizmos.DrawSphere(new Vector3(point.x, 20, point.y), displayRadius);
            }
        }
    }


    void Start()
    {
        foreach (Transform obj in treeHolder)
        {
            if (Application.isPlaying)
            {
                Destroy(obj.gameObject);
            }
            else
            {
                DestroyImmediate(obj.gameObject);
            } 
        }
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        print(points.Count);
        System.Random random = new System.Random();
        for (int i = 0; i < points.Count; i++)
        {
            // Generate a random position within a -100 by 100 bound.
            Vector3 randomPosition = new Vector3(points[i].x - 108, 50, points[i].y - 108);

            // Shoot a raycast towards the random position.
            RaycastHit hit;

            // Check if the raycast hit a surface.
            if (Physics.Raycast(randomPosition, Vector3.down, out hit) 
                && hit.normal.y > 0.97 && hit.point.y > -9.4f && hit.point.y < 5f && Mathf.PerlinNoise(hit.point.x * 0.05f, hit.point.z * 0.05f) > 0.6 )
            {
                // Instantiate the prefab at the hit point.
                _instantiatedObject = Instantiate(prefab, hit.point, Quaternion.identity);
                _instantiatedObject.isStatic = true;
                _instantiatedObject.transform.parent = treeHolder;
                float number = Random.Range(0.2f, 1f);
                _instantiatedObject.transform.localScale = Vector3.one * (float)number;
                _objectsInScene.Add(_instantiatedObject);
            }
        }
    }
}