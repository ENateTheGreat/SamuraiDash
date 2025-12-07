/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: Main Menu script that handles UI navigation and scene loading.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //==========
    // UI Panels
    //==========
    [Header("UI Panels")]
    [SerializeField] private CanvasGroup mainMenuGroup;
    [SerializeField] private CanvasGroup levelSelectGroup;
    [SerializeField] private CanvasGroup optionsGroup;

    //============
    // UI Settings
    //============
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    //=========
    // UI State
    //=========
    private bool isSwitching = false;

    //=====================
    // Group Initialization
    //=====================
    private void Start()
    {
        // Main Menu Group
        mainMenuGroup.alpha = 1f;
        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        // Level Select Group
        levelSelectGroup.alpha = 0f;
        levelSelectGroup.interactable = false;
        levelSelectGroup.blocksRaycasts = false;

        // Options Group (Not implemented yet, presently disabled)
        optionsGroup.alpha = 0f;
        optionsGroup.interactable = false;
        optionsGroup.blocksRaycasts = false;
    }

    //======================
    // Button Event Handlers
    //======================

    // Exit game functionality, with editor support
    public void OnQuitButton()
    {

        Debug.Log("Quit button pressed.");
        Application.Quit();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Switch to Level Select panel
    public void OnPlayButton()
    {
        Debug.Log("Play button pressed.");
        if (!isSwitching)
        {
            StartCoroutine(FadePanels(mainMenuGroup, levelSelectGroup));
        }
    }

    // Return to main menu panel (presently only selectable from Level Select)
    // Can be updated to dynamically return from other panels if needed
    public void OnLevelBackToMainMenu()
    {
        Debug.Log("Level Select Back button pressed.");
        if (!isSwitching)
        {
            StartCoroutine(FadePanels(levelSelectGroup, mainMenuGroup));
        }
    }

    // Load specific levels from Level Select panel
    public void OnTutorialButton() // Changed name from tutorial to level 1 later...
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnLevel2Button()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void OnLevel3Button()
    {
        SceneManager.LoadScene("Level_3");
    }

    //===================
    // Timed Fading Logic
    //===================
    private IEnumerator FadePanels(CanvasGroup from, CanvasGroup to)
    {
        isSwitching = true; // Set state

        to.gameObject.SetActive(true); // Activate target panel
        float t = 0f; // init timer

        // Turn off all interactions during fade
        from.interactable = false;
        from.blocksRaycasts = false;
        to.interactable = false;
        to.blocksRaycasts = false;

        // init starting alphas
        float startFrom = from.alpha;
        float startTo = to.alpha;

        // Fade loop
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            
            // Smooth fading
            from.alpha = Mathf.Lerp(startFrom, 0f, lerp);
            to.alpha = Mathf.Lerp(startTo, 1f, lerp);

            yield return null;
        }

        // Final values
        from.alpha = 0f;
        to.alpha = 1f;

        // Disable original panel
        from.gameObject.SetActive(false);

        // Enable interactions on target panel
        to.interactable = true;
        to.blocksRaycasts = true;
        isSwitching = false; // Reset state
    }
}
