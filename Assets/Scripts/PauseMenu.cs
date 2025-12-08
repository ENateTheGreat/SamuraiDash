/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The pause menu controller, handling button functions and menu state
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //==========
    // UI Object
    //==========
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;

    //============
    // Menu string
    //============
    [Header("Scenes")]
    [SerializeField] private string mainMenu = "MainMenu";

    //=================
    // Pause menu state
    //=================
    private bool isPaused = false;

    //=================
    // Player Reference
    //=================
    [SerializeField] private PlayerController playerController;

    // Set the initial state 
    void Start()
    {  
            pauseMenuUI.SetActive(false);  
    }

    void Update()
    {
        if (playerController.isDead) return; // Prevent pause during death anim

        // Toggle pause on Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Pause Function
    public void Pause()
    {
        if (pauseMenuUI == null) return;
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f; // Pause game -- freeze time
        pauseMenuUI.SetActive(true);
        
    }

    // Resume Function
    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f; // Resume game
        pauseMenuUI.SetActive(false);
    }

    // Restart Button Function
    public void OnRestartButton()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload scene
    }

    // Main Menu Button Function
    public void OnMainMenuButton()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene(mainMenu); // Back to main menu
    }


    // Resume Button Function
    public void OnResumeButton()
    {
        Resume();
    }
}
