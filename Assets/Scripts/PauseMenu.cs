using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Scenes")]
    [SerializeField] private string mainMenu = "MainMenu";

    private bool isPaused = false;
    [SerializeField] private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {  
            pauseMenuUI.SetActive(false);  
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isDead) return;

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

    public void Pause()
    {
        Debug
            .Log("Game Paused");
        if (isPaused) return;

        isPaused = true;

        Time.timeScale = 0f; // Pause game
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
    }

    public void Resume()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = 1f; // Resume game
        pauseMenuUI.SetActive(false);
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene(mainMenu);
    }

    public void OnResumeButton()
    {
        Resume();
    }
}
