using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [SerializeField] private float landingDelay = 0.4f;
    [SerializeField] private float postAnimDelay = 0.8f;

    private bool triggered = false;

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

    private IEnumerator VictorySequence(VictoryUI victoryUI)
    {
        yield return new WaitForSeconds(landingDelay);

        yield return new WaitForSeconds(postAnimDelay);

        victoryUI.ShowVictory();
    }
}
