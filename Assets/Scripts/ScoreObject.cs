using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class ScoreObject : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private TMP_Text scoreText;

    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position - new Vector3(-15, 35, -15);
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Weapon")
        {
            score++;
            scoreText.text = "Score: " + score;
            transform.position = player.position - new Vector3(-15, 0, -15);
        }
    }
}
