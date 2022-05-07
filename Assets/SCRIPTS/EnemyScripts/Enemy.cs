using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D _rigidbody;
    public GameObject player;
    public Collider2D enemyCollider;
    public bool isDead= false;
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public HealthBar healthBar;
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
        healthBar.gameObject.SetActive(false);
        currentHealth = maxHealth;
        healthBar.setMaxValue(maxHealth);
    }

    public void TakeDamage(int damage)
    {          
        healthBar.gameObject.SetActive(true);
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if(currentHealth <=0)
        {
            Die();
            
        }
        healthBar.setCurrentHealth(currentHealth);              
    }

    void Die()
    {
        animator.SetBool("Death", true);
        healthBar.gameObject.SetActive(false);
        isDead = true;
        //Physics2D.IgnoreCollision(enemyCollider,player.GetComponent<Collider2D>());
        player.GetComponent<CharacterMovement2D>().bashedInAir = false;
        player.GetComponent<CharacterMovement2D>().extraJumps = 1;
        Destroy(gameObject);             
    }

    void OnCollisionEnter2D()
    {    
        _rigidbody.velocity = Vector2.zero;
    }
}
