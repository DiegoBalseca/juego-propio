using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    private Vector2 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (Vector2.Distance(startPosition, transform.position) >= distance)
        {
            direction *= -1;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Obstaculo") || collision.gameObject.CompareTag("Enemy"))
        {
            direction *= -1;
            Flip();
        }
    }
}
