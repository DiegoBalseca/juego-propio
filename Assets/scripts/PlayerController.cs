using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private float inputHorizontal;
    private float jumpForce = 4.5f;
    private float playerSpeed = 5f;
    private Animator animator;

    [Header("Sensores y Control")]
    public GroundsSensor groundSensor;
    private bool isDead = false;
    private bool isDashing = false;
    private bool canDash = true;

    [Header("Dash")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCoolDown = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Ataques")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform hitBoxPosition;
    [SerializeField] private float baseChargedAttackDamage = 15f;
    [SerializeField] private float maxChargedAttackDamage = 40f;
    private float chargedAttackDamage;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundSensor = GetComponentInChildren<GroundsSensor>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDashing || isDead) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (inputHorizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("IsRunning", true);
        }
        else if (inputHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetButtonDown("Jump") && (groundSensor.isGrounded || groundSensor.canDoubleJump))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Fire1")) NormalAttack();
        if (Input.GetButtonDown("Fire2")) AttackCharge();
        if (Input.GetButtonUp("Fire2")) AttackCharge();

        
        if (Input.GetKeyDown(KeyCode.E)) Shoot();

        animator.SetBool("IsJumping", !groundSensor.isGrounded);
    }

    void FixedUpdate()
    {
        if (!isDead && !isDashing)
        {
            rigidbody2D.velocity = new Vector2(playerSpeed * inputHorizontal, rigidbody2D.velocity.y);
        }
    }

    void Jump()
    {
        if (!groundSensor.isGrounded)
        {
            groundSensor.canDoubleJump = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        }

        rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    IEnumerator Dash()
    {
        float originalGravity = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        isDashing = true;
        canDash = false;

        rigidbody2D.AddForce(transform.right * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);

        rigidbody2D.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    public void TakeDamage(float damage)
    {
        int currentHealth = 100;
        UnityEngine.UI.Slider healthBar = null;

        currentHealth -= (int)damage;
        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("IsDeath");
        audioSource.PlayOneShot(deathSound);

        MusicaNivel musica = FindObjectOfType<MusicaNivel>();
        if (musica != null) musica.StopMusic();

        rigidbody2D.velocity = Vector2.zero;
        this.enabled = false;
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    void NormalAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitBoxPosition.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    void AttackCharge()
    {
        chargedAttackDamage += Time.deltaTime;
        chargedAttackDamage = Mathf.Clamp(chargedAttackDamage, baseChargedAttackDamage, maxChargedAttackDamage);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitBoxPosition.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(chargedAttackDamage);
            }
        }

        chargedAttackDamage = baseChargedAttackDamage;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.rotation.y == 0 ? 1f : -1f;
                rb.velocity = new Vector2(direction * bulletSpeed, 0f);
            }

            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (hitBoxPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hitBoxPosition.position, attackRadius);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trampa") || other.CompareTag("Muerte"))
        {
            Death();
        }
    }
}


