using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private float inputHorizontal;
    private float jumpForce = 4.5f;
    private float playerSpeed = 5f;
    public GroundsSensor groundSensor;
    private Animator animator;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCoolDown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform hitBoxPosition;
    [SerializeField] private float baseChargedAttackDamage = 15f;
    [SerializeField] private float maxChargedAttackDamage = 40f;
    private float chargedAttackDamage;

    private bool canShoot = true;
    private bool isDead = false; // <--- NUEVO

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundSensor = GetComponentInChildren<GroundsSensor>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing || isDead)
        {
            return;
        }

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

        if (Input.GetButtonDown("Fire2"))
        {
            AttackCharge();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            AttackCharge();
        }

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            NormalAttack();
        }

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

    void Death()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("¡muerte!");
        animator.SetTrigger("IsDeath");

        rigidbody2D.velocity = Vector2.zero;
        this.enabled = false;
    }

    public void OnDeathAnimationEnd()
    {
        Debug.Log("Animación de muerte terminada. Destruyendo jugador...");
        Destroy(gameObject);
        // O usa gameObject.SetActive(false); si prefieres solo ocultarlo
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
        if (chargedAttackDamage < baseChargedAttackDamage)
        {
            chargedAttackDamage = baseChargedAttackDamage;
        }
        else if (chargedAttackDamage > maxChargedAttackDamage)
        {
            chargedAttackDamage = maxChargedAttackDamage;
        }

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
        if (other.CompareTag("Trampa"))
        {
            Debug.Log("¡Tocó una trampa!");
            Death();
        }
    }
}
