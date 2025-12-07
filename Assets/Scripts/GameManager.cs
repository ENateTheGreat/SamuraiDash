/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The GameManager class is used to maintain general game stats 
 *              in a singular, persisted instance across scenes.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //===================
    // Singleton Instance
    //===================
    public static GameManager Instance;

    //==============
    // General Stats
    //==============
    [Header("Stats")]
    public int totalDeaths = 0;
    public float totalLevelPlayTime = 0f;
    public float currentLevelTime = 0f;

    //=============================================
    // Whether we are currently counting level time
    //=============================================
    private bool countingTime = false;

    //======================
    // Singleton Maintenence
    //======================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() // Cleanup
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    //===============
    // Stats Tracking
    //===============
    private void Update()
    {
        if (!countingTime) return; // i.e. Main menu

        // Time tracking
        float dt = Time.deltaTime;
        currentLevelTime += dt;
        totalLevelPlayTime += dt;
    }

    // Function to check if we need to be tracking time
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level")) // Level_1, Level_2, etc.
        {
            countingTime = true;
            currentLevelTime = 0f;
        }
        else
        {
            countingTime = false;
        }
    }

    // Death Stat adding
    public void RegisterDeath()
    {
        totalDeaths++;
    }
}
