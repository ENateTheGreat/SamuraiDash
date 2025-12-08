/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The victory controller script for handling player victory sequence.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [SerializeField] private float landingDelay = 0.4f; // Allow time for player to hit ground if not grounded
    [SerializeField] private float postAnimDelay = 0.8f; // Allow time for the victory animation to complete

    private bool triggered = false; // Victory state

    // Victory logic for when player enters victory zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.EnterVictoryState();
        }

        VictoryUI victoryUI = FindObjectOfType<VictoryUI>();

        if (victoryUI != null)
        {
            StartCoroutine(VictorySequence(victoryUI));
        }
        else
        {
            Debug.LogWarning("VictoryUI not found in the scene.");
        }
    }

    // Timed victory sequence
    private IEnumerator VictorySequence(VictoryUI victoryUI)
    {
        yield return new WaitForSeconds(landingDelay);

        yield return new WaitForSeconds(postAnimDelay);

        victoryUI.ShowVictory();
    }
}
