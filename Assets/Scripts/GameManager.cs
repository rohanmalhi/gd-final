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
    
    [Header("Map Stuff")]
    [SerializeField] GameObject playerCam;
    [SerializeField] private GameObject mapCam;
    
    
    [Header("Pause Menu Buttons")] 
    [SerializeField] GameObject resumeButton;
    [SerializeField] private GameObject pauseSettingsButton;
    [SerializeField] private GameObject pauseQuitButton;
    

    
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        string sceneName = currentScene.name;

        if (sceneName == "SampleScene")
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
        SceneManager.LoadScene(1);
    }

    public void OnSettingsButtonClick()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void OnResumeButtonClick()
    {
        menu = 0;
        gameRunning = true;
        panel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMainQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnLevelQuitButtonClick()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene(1);
        }
    }

    public void OnQuitToMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMapButtonClick()
    {
        playerCam.SetActive(false);
        mapCam.SetActive(true);
        panel.SetActive(false);
    }

    public void OnMapReturnClick()
    {
        playerCam.SetActive(false);
        mapCam.SetActive(true);
        panel.SetActive(false);
    }
}
