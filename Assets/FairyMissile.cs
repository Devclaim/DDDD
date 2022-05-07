using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FairyMissile : MonoBehaviour
{
    public MinionAI minionAI;
    public Transform target;
    public float speed;
    public float rotateSpeed = 800f;

    public int damage = 5;
    Vector2 direction;

    private Rigidbody2D _rigidbody;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        minionAI = FindObjectOfType<MinionAI>();
        if(minionAI.detectedTarget.gameObject.transform != null)
        {
            target = minionAI.detectedTarget.gameObject.transform;
        }
        else
        {
           target = null;
        }        
        Destroy(gameObject, 10);
    }

    void Update()
    {
        if(target != null)
        {
            direction = (Vector2)target.position - _rigidbody.position;
        }
        else
        {
            direction = Vector2.left;
        }

        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _rigidbody.angularVelocity = -rotateAmount * rotateSpeed;
        _rigidbody.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D hitWho)
    {
        
        Enemy enemy = hitWho.gameObject.GetComponent<Enemy>();
        if(hitWho != null && hitWho.gameObject.tag == "Enemy")
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        
    }
}
