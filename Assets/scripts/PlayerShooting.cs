using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint; 
    public float bulletForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E));
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletForce;
    }
}
