using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup victoryPanelGroup;
    [SerializeField] private Image overlay;
    [SerializeField] private Button nextLevelButton;

    [Header("Fade Settings")]
    [SerializeField] private float overlayTargetAlpha = 0.2f; // 20% dark
    [SerializeField] private float overlayFadeDuration = 0.5f;
    [SerializeField] private float panelFadeDuration = 0.4f;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string nextLevel;

    private bool shown = false;

    private void Awake()
    {
        victoryPanelGroup.alpha = 0f;
        victoryPanelGroup.interactable = false;
        victoryPanelGroup.blocksRaycasts = false;
        overlay.raycastTarget = false;

        Color c = overlay.color;
        c.a = 0f;
        overlay.color = c;
    }

    public void ShowVictory()
    {
        if (shown) return;
        shown = true;
        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        float t = 0f;
        float startAlpha = overlay.color.a;
        Color c = overlay.color;

        while (t < overlayFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / overlayFadeDuration);
            c.a = Mathf.Lerp(startAlpha, overlayTargetAlpha, lerp);
            overlay.color = c;
            yield return null;
        }

        c.a = overlayTargetAlpha;
        overlay.color = c;

        t = 0f;
        startAlpha = victoryPanelGroup.alpha;
        victoryPanelGroup.interactable = false;
        victoryPanelGroup.blocksRaycasts = false;

        while (t < panelFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / panelFadeDuration);
            victoryPanelGroup.alpha = Mathf.Lerp(startAlpha, 1f, lerp);
            yield return null;
        }
        Debug.Log("Victory panel shown.");
        victoryPanelGroup.alpha = 1f;
        victoryPanelGroup.interactable = true;
        victoryPanelGroup.blocksRaycasts = true;
    }

    public void OnNextLevelButton()
    {
        Debug.Log("Loading Next Level Scene.");
        string currentScene = SceneManager.GetActiveScene().name;

        if (MusicManager.Instance != null && MusicManager.Instance.musicSource != null)
        {
            MusicManager.Instance.musicSource.Stop();
        }

        switch (currentScene)
        {
            case "Level_1":
                SceneManager.LoadScene("Level_2");
                break;

            case "Level_2":
                SceneManager.LoadScene("Level_3");
                break;

            case "Level_3":
                nextLevelButton.gameObject.SetActive(false);
                break;

            default:
                Debug.LogWarning("Unknown scene: " + currentScene);
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void OnMainMenuButton()
    {
        Debug.Log("Loading Main Menu Scene: " + mainMenuScene);
        if (!string.IsNullOrEmpty(mainMenuScene))
            SceneManager.LoadScene(mainMenuScene);
    }
}
