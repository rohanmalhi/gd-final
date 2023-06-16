using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    private bool gameRunning;

    private int menu; // 0 = None, 1 = Title, 2 = Settings, 3 = Load, 4 = Pause
    

    [SerializeField] private GameObject panel;
    [SerializeField] private AudioSource buttonSound;
    
    [Header("Map Stuff")]
    [SerializeField] GameObject playerCam;
    [SerializeField] private GameObject mapCam;
    [SerializeField] private GameObject crosshair;
    
    
    [Header("Pause Menu Buttons")] 
    [SerializeField] GameObject resumeButton;
    [SerializeField] private GameObject pauseSettingsButton;
    [SerializeField] private GameObject pauseQuitButton;
    

    
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        string sceneName = currentScene.name;

        if (sceneName == "Terrain")
        {
            gameRunning = true;
            menu = 0;
        }
        else if (sceneName == "MainMenu")
        {
            menu = 1;
            gameRunning = false;
        }
        else
        {
            gameRunning = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Pause"))
        {
            menu = 4;
            gameRunning = false;
            panel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (gameRunning == false)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void OnNewGameButtonClick()
    {
        buttonSound.Play();
        SceneManager.LoadScene(1);
    }

    public void OnSettingsButtonClick()
    {
        buttonSound.Play();
        SceneManager.LoadScene("SettingsMenu");
    }

    public void OnResumeButtonClick()
    {
        buttonSound.Play();
        menu = 0;
        gameRunning = true;
        panel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMainQuitButtonClick()
    {
        Application.Quit();
        buttonSound.Play();
    }

    public void OnLevelQuitButtonClick()
    {
        buttonSound.Play();
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene(1);
        }
    }

    public void OnQuitToMenuClick()
    {
        buttonSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMapButtonClick()
    {
        buttonSound.Play();
        playerCam.SetActive(false);
        mapCam.SetActive(true);
        panel.SetActive(false);
        crosshair.SetActive(false);
    }

    public void OnMapReturnClick()
    {
        buttonSound.Play();
        playerCam.SetActive(false);
        mapCam.SetActive(true);
        panel.SetActive(false);
        crosshair.SetActive(true);
    }
}
