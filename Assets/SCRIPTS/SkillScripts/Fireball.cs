using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    
    
    public Rigidbody2D _rigidbody;
    public float speed = 10f;
    public int damage = 40;

    public GameObject impactEffect;
    
    void Update()
    {
        Destroy(gameObject, 5f);
    }
    void Start()
    {
        _rigidbody.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D hitWho)
    {
        CharacterMovement2D playerDodging = hitWho.gameObject.GetComponent<CharacterMovement2D>();
        CharacterStats player = hitWho.gameObject.GetComponent<CharacterStats>();
        Enemy enemy = hitWho.gameObject.GetComponent<Enemy>();
        if(hitWho.collider != null && hitWho.gameObject.tag == "Enemy")
        {
            enemy.TakeDamage(damage);
        }
        if(hitWho.collider != null && hitWho.gameObject.tag ==  "Player")
        {
            if(playerDodging.isDashing || playerDodging.isBashing)
            {
                Physics2D.IgnoreCollision(_rigidbody.GetComponent<Collider2D>(), hitWho.collider);
            }
            else
            {
                player.TakeDamage(damage);
                Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
              
        }
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);   
        }
             
    }

}
