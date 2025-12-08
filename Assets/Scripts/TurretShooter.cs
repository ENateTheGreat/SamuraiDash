/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The turret shooter script for firing projectiles handling animation and frequency.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour
{
    public Transform firePoint; // Where projectiles spawn
    public GameObject projectile; // Shuriken Prefab
    public float fireRate = 1.5f; // Time between shots
    public float projectileSpeed = 15f; // Speed of projectile

    private float fireTimer; // Timer to track firing
    private Animator anim; // Animator reference

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); // Get animator from child object
    }

    // Start for initial shot
    void Start()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    // Shooting update with timer
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    // Shooting logic
    void Shoot()
    {
        if (anim != null)
        {
            anim.SetTrigger("shoot");
        }

        GameObject projObj = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Shuriken proj = projObj.GetComponent<Shuriken>();

        if (proj != null)
        {
            proj.speed = projectileSpeed;  
        }
    }
}
