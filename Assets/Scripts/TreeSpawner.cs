using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class TreeSpawner : MonoBehaviour
{
    public GameObject prefab;
    private GameObject _instantiatedObject;
    private List<GameObject> _objectsInScene = new List<GameObject>();
    
    void OnValidate()
    {
        foreach (var obj in _objectsInScene)
        {
            try
            {
                DestroyImmediate(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Destroy(obj);
            }
        }
        for (int i = 0; i < 100; i++)
        {
            // Generate a random position within a -100 by 100 bound.
            Vector3 randomPosition = new Vector3(Random.Range(-90, 90), 50, Random.Range(-90, 90));

            // Shoot a raycast towards the random position.
            RaycastHit hit;

            // Check if the raycast hit a surface.
            if (Physics.Raycast(randomPosition, Vector3.down, out hit))
            {
                // Instantiate the prefab at the hit point.
                _instantiatedObject = Instantiate(prefab, hit.point + Vector3.one*3, Quaternion.identity);
                _instantiatedObject.isStatic = true;
                _objectsInScene.Add(_instantiatedObject);
            }
        }
    }
}