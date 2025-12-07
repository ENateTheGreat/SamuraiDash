/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The HUD controller makes use of the GameManager to display
 *              the stats that are being tracked, with the ability to toggle
 *              time viewing modes (presently not implemented in the UI, sorry)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    //===============
    // UI GameObjects
    //===============
    [SerializeField] private TextMeshProUGUI deathsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private bool showTotalTime = true;

    // Updating the UI every frame
    void Update()
    {
        if (GameManager.Instance == null) return; // No GameManager present

        // Update Deaths
        deathsText.text = "Deaths: " + GameManager.Instance.totalDeaths.ToString();

        // Change time display
        float seconds = showTotalTime 
            ? GameManager.Instance.totalLevelPlayTime // Total time played in level
            : GameManager.Instance.currentLevelTime; // Current level time only

        // Helper vars
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);

        // Update Time
        timeText.text = $"Time: {minutes:00}:{secs:00}";
    }
}
