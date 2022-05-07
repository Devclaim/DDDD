using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    public float knockback= 3;
    public int  damage= 30;

    void Update()
    {
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D hitWho)
    {
        Enemy enemy = hitWho.gameObject.GetComponent<Enemy>();
        if(hitWho != null && hitWho.gameObject.tag == "Enemy")
        {
            enemy.TakeDamage(damage);
            if(transform.position.x > enemy.GetComponent<Rigidbody2D>().position.x)
           {
               enemy.GetComponent<Rigidbody2D>().velocity = new Vector2 (-knockback, knockback);
           }
           else
           {
               enemy.GetComponent<Rigidbody2D>().velocity = new Vector2 (knockback, knockback);
           }
        }
        
    }
}
