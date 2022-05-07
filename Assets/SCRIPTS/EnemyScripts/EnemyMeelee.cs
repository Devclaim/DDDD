using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeelee : MonoBehaviour
{
    public Enemy enemy;
    public Animator animator;
    public float speed;
    public Rigidbody2D _rigidbody;
    public DetectPlayer detectPlayer;
    public DetectPlayer detectPlayerInner;
    public Transform Player;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int attackDamage;
    private float nextAttackTime;
    public float attackSpeed = 2f;
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {

        if(enemy.isDead== false)
        {
        if(detectPlayer.PlayerDetected && detectPlayerInner.PlayerDetected== false)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        if(detectPlayer.PlayerDetected)
        {
            if(Player.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0,180,0); 
            }
            else
            {
                transform.rotation = Quaternion.Euler(0,0,0); 
            }
        }
        if(detectPlayerInner.PlayerDetected)
        {
            if(Time.time >= nextAttackTime)
            Attack();
            nextAttackTime = Time.time + 1f / attackSpeed;
            
        }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D player in hitPlayer)
            {
                player.GetComponent<CharacterStats>().TakeDamage(attackDamage);
                
            if(player.transform.position.x < enemy.transform.position.x)
            {
                player.GetComponent<CharacterMovement2D>().knockbackRight = true;
            }
            else
            {
                player.GetComponent<CharacterMovement2D>().knockbackRight = false;
            } 
            }
    }
}
