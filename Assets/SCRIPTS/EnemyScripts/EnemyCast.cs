using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCast : MonoBehaviour
{
    public Enemy enemy;
    public float speed;
    public Transform Player;
    public DetectPlayer detectPlayer;
    public DetectPlayer detectPlayerInner;
    public Fireball fireball;
    private float castTimer;
    public GameObject fireballPrefab;
    public Transform castingPoint;
    private float timeStamp;
    public float cooldown;
    public float castTimeFireballEnemy;


    void Update()
    {   
        if(enemy.isDead== false)
        {
        if(detectPlayerInner.PlayerDetected)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
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
        if(detectPlayer.PlayerDetected && timeStamp <= Time.time && detectPlayerInner.PlayerDetected == false)
        {
            
            castTimer += Time.deltaTime;
            if(castTimer >= castTimeFireballEnemy)
            {
                timeStamp = Time.time + cooldown;
                Instantiate(fireballPrefab, castingPoint.position, castingPoint.rotation);
                
            }      
        } 
        }
    }
}
