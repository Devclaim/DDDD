using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinionAI : MonoBehaviour
{
    public GameObject detectedTarget;
    public DetectPlayer detectPlayer;
    public Transform Target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float upTime = 20;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath;


    Seeker seeker;
    Rigidbody2D _rigidbody;


    [Header("Projectile Settings")]
    private float castTimer;
    public GameObject fairyMissilePrefab;
    public Transform castingPoint;
    private float timeStamp;
    public float cooldown;
    public float castTime;


    void Start()
    {
        if(reachedEndOfPath)
        {
            reachedEndOfPath = true;
        }
        reachedEndOfPath = false;
        seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        Target = GameObject.Find("minionSpawn").transform;

        InvokeRepeating("UpdatePath", 0f, 0.3f);
        Destroy(gameObject, upTime);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(_rigidbody.position, Target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        _rigidbody.AddForce(force);

        float distance = Vector2.Distance(_rigidbody.position , path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f,1f);
        }
        else if(force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f,1f);
        }
    }

    void Update()
    {
        
        if(detectPlayer.PlayerDetected && timeStamp <= Time.time)
        {   
            if(detectPlayer.Target != null)
            {
                detectedTarget = detectPlayer.Target; 
            }         
            castTimer += Time.deltaTime;
            if(castTimer >= castTime)
            {
                timeStamp = Time.time + cooldown;
                Instantiate(fairyMissilePrefab, castingPoint.position, castingPoint.rotation);
                
            }  
        }
            
    }
}
