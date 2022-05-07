using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool attackedinAir=false;
    public float upAttackJumpForce;
    public Rigidbody2D _rigidbody;
    public Animator animator;
    public CharacterMovement2D characterMovement2D;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 15;
    public float attackSpeed = 2f;
    float nextAttackTime = 0f;
    public int maxHealth = 100;
    int currentHealth;
    public int maxMana = 100;
    int currentMana;
    public int manaRegen = 5;


    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
    
    void Update()
    {
        //Retired upAttack
        /*if(characterMovement2D.isDashing == false)
        {

        if(Input.GetAxis("Vertical") > 0)
        {
        if(Input.GetButtonDown("Attack"))
        {
            
            if(attackedinAir ==false && characterMovement2D.isGrounded == false )
            {
                
                UpAttack();
                attackedinAir = true;
            }
            
        } 
        }
        else */if(Time.time >= nextAttackTime)
        {
            if(Input.GetButtonDown("Attack"))
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
        }

        
        //}
        if(characterMovement2D.isGrounded)
        {
            attackedinAir = false;
        }
    }

    void UpAttack()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, upAttackJumpForce);
    }

    void Attack()
    {
        
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if(currentHealth <=0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("Death", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void Regenerate() 
    {
        if(currentMana < maxMana)
        {
            currentMana += manaRegen;
        }

    }
}
