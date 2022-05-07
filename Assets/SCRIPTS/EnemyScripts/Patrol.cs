using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Enemy enemy;
     public float speed;
    private bool movingRight = true;
    public Transform groundDetector;

    void Update()
    {
        if(enemy.isDead== false)
        {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundCast = Physics2D.Raycast(groundDetector.position, Vector2.down, 2f);
        if(groundCast.collider == false)
        {
            if(movingRight == true)
            {
                transform.eulerAngles = new Vector3(0,-180,0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3 (0,0,0);
                movingRight = true;

            }
        }
        }
    }
}
