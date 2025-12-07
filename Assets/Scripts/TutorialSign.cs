using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSign : MonoBehaviour
{
    public CanvasGroup bubble;
    public Animator bubbleAnim;
    public float fadeDuration = 0.4f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bubble.gameObject.SetActive(true);
            bubbleAnim.SetTrigger("fade in");
            bubbleAnim.ResetTrigger("fade out");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bubbleAnim.SetTrigger("fade out");
            bubbleAnim.ResetTrigger("fade in");

        }
    }
}
