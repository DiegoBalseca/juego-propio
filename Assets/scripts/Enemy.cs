using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;
    private float currentHealth;
    public int direction = 1;
    public float enemySpeed = 2;
    private Rigidbody2D rigidbody;
    private Animator animator;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("death");
        Destroy(gameObject, 0.5f); 
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    void OnCollisionEnter2D(Collision2D collision)  
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.Death();
        }

        if (collision.gameObject.CompareTag("Obstaculo") || collision.gameObject.layer == 6)
        {
            direction *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Obstaculo") || collider.gameObject.layer == 6)
        {
            direction *= -1;
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(enemySpeed * direction, rigidbody.velocity.y);
    }
}

 

