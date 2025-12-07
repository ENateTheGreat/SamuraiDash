using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectile;
    public float fireRate = 1.5f;
    public float projectileSpeed = 15f;

    private float fireTimer;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    // Update is called once per frame
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
