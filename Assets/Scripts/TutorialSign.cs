/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: Tutorial sign script for showing and hiding tutorial bubbles.
 * NOTE: Sorry I did not write the text bubble scripts, those came with the prefabs but they
 *       did require some tinkering and assembly to get working properly.
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSign : MonoBehaviour
{
    public CanvasGroup bubble; // The speech bubble
    public Animator bubbleAnim; // Animator for bubble
    public float fadeDuration = 0.4f; // Duration of fade

    // Trigger logic for spawning in bubble on player enter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bubble.gameObject.SetActive(true);
            bubbleAnim.SetTrigger("fade in");
            bubbleAnim.ResetTrigger("fade out");
        }
    }


    // Trigger logic for removing bubble on player exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bubbleAnim.SetTrigger("fade out");
            bubbleAnim.ResetTrigger("fade in");

        }
    }
}
