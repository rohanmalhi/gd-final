using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject deadMenu;
    [SerializeField] private GameObject ui;

    private int playerHealth = 4;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth > 1)
            {
                playerHealth -= 1;
                scoreText.text = "Health: " + playerHealth;
            }
            else
            {
                crosshair.gameObject.SetActive(false);
                ui.gameObject.SetActive(false);
                deadMenu.gameObject.SetActive(true);
            }
        }  else if (collision.gameObject.CompareTag("Weapon"))
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}
