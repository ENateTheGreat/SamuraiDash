/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: Shurilken projectile script for movement and cleanup.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 10f; // Speed
    public float lifetime = 10f; // Max lifetime before self-destruct

    private Rigidbody2D srb;

    // Set rb reference
    private void Awake()
    {
        srb = GetComponent<Rigidbody2D>();
    }

    void Start() // Launch shuriken
    {
        srb.velocity = transform.right * -speed;

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision) // Destroy on collision
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) // Destroy on trigger collision
    {
        Destroy(gameObject);
    }
}
