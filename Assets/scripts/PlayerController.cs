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

[Serializefield ] private LayerMask enemyLayer;
[Serializefield ] private float dashForce = 20; 
[Serializefield ] private float dashDuration = 0.5f;
[Serializefield ] private float dashCoolDown = 1;
private bool cadDash = true;
private bool isDashing = false;

[Serializefield] private float attackDamage = 10;
[Serializefield] private float attackRadius = 1;
[Serializefield] private Transform hitBoxPosition;
[Serializefield] private float basechargedAttackDamage = 15; 
[Serializefield] private float maxcharged AttackDamage = 40;
private float chargedAttackDamage;


    
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundSensor = GetComponentInChildren<GroundsSensor>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {

          if(isDashing)
        {
            return;
        }
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if(inputHorizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("IsRunning", true);
        }

        else if(inputHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("IsRunning", true);
        }

        else 
        {
            animator.SetBool("IsRunning", false);
        }

        if(Input.GetButtonDown("Jump") && groundSensor.isGrounded == true)
        {
            if(groundSensor.isGrounded || groundSensor.canDoubleJump)
            {
                Jump();
            } 
          
       
        }

         if (input.GetKeyDown(KeyCode.LeftShift)) //nuevo de hoy
        {
            StartCoroutine(Dash());
        }

        if (input.(GetButtonDown)("Fire2"))
        {
            AttackCharge();
        }

        if(Input.GetBUttonUp("Fire2"))
        {
            AttackCharge();
        }

        if(Input.GetButtonDown("Fire1") && canShoot)
        {

        }



            

        animator.SetBool("IsJumping", !groundSensor.isGrounded);
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(playerSpeed * inputHorizontal, rigidbody2D.velocity.y);

    }

    void Jump()
    
    {

         if(!groundSensor.isGrounded)
         {
            groundSensor.canDoubleJump = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
         }


        rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    IEnumuertor Dash() //nuevo de hoy
    {

        float gravity = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        isDashing = true;
        canDash = false;
        rigidbody2D.AddForce(trasnform.right * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds (dashDuration);
        rigidbody2D.gravity = gravity;
        isDashing = false;
        isDashing = false;
        yield return new WaitForSeconds (dashCoolDown);
        canDash = true;
    }


    public void TakeDamage (float damage;)
    {
        currentHealth-= (int)damage;
        healthBar.value = currentHealth;
        
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    void NormalAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitBoxPosition.position, attackRadius);
        
        foreach(Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(attackDamage);
        }

        
    }

    void AttackDamage()
    {
        chargedAttackDamage += Time.deltaTime;
    }

    void AttackCharge()
     {

        if(chargedAttackDamage < maxchargedAttackDamage)
         chargedAttackDamage 
         Collider2D[] enemies = Physics2D.OverlapCircleAll(hitBoxPosition.position, attackRadius);
        
        foreach(Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(attackDamage);
        }

        chargedAttackDamage = basechargedAttackDamage;
     }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitBoxPosition.position, attackRadius);
    }


  
}
