using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    private bool turnFaceLeft = false;
    private bool turnFaceRight = false;
    public bool facingRight = false;
    public float movement;
    public Casting casting;
    public CharacterStats characterStats;
    public Collider2D playerCollider;
    public Collider2D ignoreThisCollider;
    public Collider2D ignoreThisColliderBash;
    public Collider2D ignoreThisColliderSlam;
    public bool isDashing = false;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int dashDirection;
    public Animator animator;
    public float MovementSpeed;
    public float JumpForce;
    public float secondJumpForce;

    public bool isgrounded = true;
    public bool isJumping;
    public float jumpTimeCounter;
    public float jumpTime;
    public bool isGrounded;
    public Transform feetPos;
    public Vector2 checkGroundSize;
    public LayerMask isJumpable;
    private float moveInput;

    public bool dashedInAir =false;


    public float groundedRemember;
    public float groundedRememberTimer;

    public float jumpRemember;
    public float jumpRememberTimer;



    public  Rigidbody2D _rigidbody;

    public int extraJumps = 1;
    public float knockback;
    public float knockbackLength;
    public float knockbackTime;
    public bool knockbackRight;
    public bool canTakeDamage = true;
    public float canTakeDamageTimer;
    public float canTakeDamageTime;

    
    [Header("Bash Settings")]

    
    public float bashChargeSlow;
    public float maxBashTime= 2f;
    public float bashSpeed;
    public bool canBash = true;
    public bool isBashing = false;
    public  float bashingTime;
    private  float bashingTimeReset;
    public bool bashedInAir = false;
    public int  bashDamage;
    Vector3 bashDirection;
    private bool enemyIsDead = false;
    private float bashChargeMovementSpeed;

    [Header("Bash Arrow Settings")]

    public GameObject arrow;

    [Header("Walljump Settings")]

    public bool isTouchingFront;
    public Transform checkWall;
    public Vector2 checkWallSize;
    public bool wallSliding;
    public float wallSlidingSpeed;

    public bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    


    [Header("Controller")]

    //controller support stuff
    public  float rStickX;
    public  float rStickY;
    public  bool useController= false;
    void Start()
    {
        Cursor.visible = true;
        _rigidbody = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        bashingTimeReset = bashingTime;
        arrow.SetActive(false);
        bashChargeMovementSpeed = MovementSpeed;
    }


    void FixedUpdate()
    {
        playerDirection();

    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(feetPos.position, checkGroundSize,0, isJumpable);
        movement = Input.GetAxisRaw("Horizontal");

        if(knockbackTime <= 0)
        {                
            if(!isBashing && !wallJumping && !isDashing && !casting.inAirSlamming)
            {
                transform.position += new Vector3(movement,0,0) * Time.deltaTime * MovementSpeed;
                animator.SetFloat("Speed", Mathf.Abs(movement));
            }           
        }
        else 
        {
           if(knockbackRight)
           {
               _rigidbody.velocity = new Vector2 (-knockback, knockback);
               movement =0;
           }
           if(!knockbackRight)
           {
               _rigidbody.velocity = new Vector2 (knockback, knockback);
               movement=0;
           }
           
           knockbackTime -= Time.deltaTime;
           if(knockbackTime <= 0)
           {
               _rigidbody.velocity = Vector2.zero;
           }
        }
        
        //------------------------ROTATE CHARACTER------------------------------------
        if(!wallSliding){
        if(!Mathf.Approximately(0, movement))
        {
            transform.rotation = movement > 0 ? Quaternion.Euler(0,180,0) : Quaternion.identity;
            _rigidbody.velocity = new Vector2(moveInput * MovementSpeed, _rigidbody.velocity.y);
        }
        }
        //------------------------ROTATE CHARACTER------------------------------------

        //------------------------DASHING------------------------------------             
        if(dashedInAir == false)
        {
            if(dashDirection == 0)
            {
                if(Input.GetButtonDown("Dash") && knockbackTime <=0 && !casting.inAirSlamming)
                {
                    casting.InterruptCast();
                    if(movement < 0 && !wallSliding)
                    {
                        dashDirection = 1;
                        isDashing = true;
                    }
                    else if (movement > 0 && !wallSliding)
                    {
                        dashDirection = 2;
                        isDashing = true;
                    }
                    else if (movement == 0 && !wallSliding)
                    {
                        dashDirection = 3;
                        isDashing = true;
                    }
                    else if (wallSliding)
                    {
                        dashDirection = 4;
                        isDashing = true;
                    }
                }
            }
            else
            {
                if(dashTime <= 0)
                {   
                    if(turnFaceRight)
                    {
                        facingRight = true;
                        turnFaceRight = false;
                    }
                    if(turnFaceLeft)
                    {
                        facingRight = false;
                        turnFaceLeft = false;
                    }
                    
                    dashDirection = 0;
                    dashTime = startDashTime;
                    _rigidbody.velocity = Vector2.zero;
                    isDashing= false;
                               
                    if(isGrounded == false)
                    {
                        dashedInAir = true;
                    }
                }
                else
                {
                    dashTime -= Time.deltaTime;
                    if(dashDirection == 1)
                    {
                        _rigidbody.velocity = Vector2.left * dashSpeed;
                    }
                    else if(dashDirection == 2)
                    {
                        _rigidbody.velocity = Vector2.right * dashSpeed;
                        
                    }
                    else if (dashDirection == 3)
                    {
                        if(facingRight)
                        {
                            _rigidbody.velocity = Vector2.right * dashSpeed;
                        }
                        else if (!facingRight)
                        {
                            _rigidbody.velocity = Vector2.left * dashSpeed;
                        }
                    }
                    else if(dashDirection == 4)
                    {
                        if(facingRight)
                        {
                            _rigidbody.velocity = Vector2.left * dashSpeed;
                            _rigidbody.transform.eulerAngles = new Vector3(0, 0, 0);
                            turnFaceLeft = true;
                        }
                        else if (!facingRight)
                        {
                            _rigidbody.velocity = Vector2.right * dashSpeed;
                            _rigidbody.transform.eulerAngles = new Vector3(0, 180, 0);
                            turnFaceRight = true;
                        }
                    }
                    if(ignoreThisCollider != null)
                    {
                        Physics2D.IgnoreCollision(playerCollider, ignoreThisCollider);
                    }
                }
            }   
        }
    
        if(isDashing == false)
        {
            if(ignoreThisCollider != null)
                {                  
                Physics2D.IgnoreCollision(playerCollider, ignoreThisCollider, false);
                }
            ignoreThisCollider = null;
        }
        if(isBashing == false)
        {
            if(ignoreThisColliderBash != null)
                {                  
                Physics2D.IgnoreCollision(playerCollider, ignoreThisColliderBash, false);
                }
            ignoreThisColliderBash = null;
        }
        if(isGrounded)
        {
            bashedInAir = false;
            dashedInAir = false;
            extraJumps = 1;
        }     
        //-----------------------DASHING------------------------------------


        //-----------------------DAMAGE TIMER------------------------------------
        if(canTakeDamageTimer <= 0)
        {
            canTakeDamage = true;
            canTakeDamageTimer = canTakeDamageTime;
        }
        else if(canTakeDamage == false)
        {
            canTakeDamageTimer -= Time.deltaTime;
        }
        //-----------------------DAMAGE TIMER------------------------------------



        //------------------------JUMPING---------------------------------------
        jumpRemember -= Time.deltaTime;
        if(Input.GetButtonDown("Jump") && !wallJumping && isDashing)
        {
            jumpRemember = jumpRememberTimer;
        }

        groundedRemember -= Time.deltaTime;
        if(isGrounded)
        {
            groundedRemember = groundedRememberTimer;
        }
        if(isDashing== false && isBashing == false)
        {
        if(!wallJumping)
        {
            if((Input.GetButtonDown("Jump") || jumpRemember > 0) && extraJumps > 0 && !isGrounded && !casting.inAirSlamming)
            {           
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
                jumpTimeCounter = jumpTime;
                isJumping = true;
                extraJumps--;
            }
            else if(Input.GetButtonDown("Jump")  && groundedRemember > 0 && !casting.inAirSlamming) 
            {
                jumpTimeCounter = jumpTime;
                isJumping = true;
            }

            if(Input.GetButton("Jump") && isJumping == true)
            {
                if(jumpTimeCounter > 0)
                {               
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
                    jumpTimeCounter -= Time.deltaTime;
                } 
                else
                {
                    isJumping = false;
                }             
            }
            if(Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }
        }
        }
        //------------------------JUMPING------------------------------------


        //------------------------ANIMATION------------------------------------ 
        if(isJumping == true)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        if(Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if(isgrounded == true)
        {
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
        }
        //------------------------ANIMATION------------------------------------


            
        //------------------------BASHING------------------------------------
        if(Input.GetButtonDown("Bash") && bashedInAir == false && knockbackTime <=0 && !casting.inAirSlamming)
        { 
            _rigidbody.drag = bashChargeSlow;
            MovementSpeed= 1;
            StartCoroutine("bashCounter");
            canBash = true;
            arrow.SetActive(true);
        }
        else 
        if(Input.GetButtonUp("Bash")&& bashedInAir == false && canBash)
        {
            casting.InterruptCast();
            Bash();
            StopCoroutine("bashCounter");
            arrow.SetActive(false);                                  
        }
        if(isBashing)
        {
            if(bashingTime > 0)
            {               
                bashedInAir = true; 
                bashingTime -= Time.deltaTime;
                _rigidbody.velocity = bashDirection * bashSpeed;
                if(ignoreThisColliderBash != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, ignoreThisColliderBash);               
                }
            }
            else
            {   
                if(enemyIsDead)
                {
                    bashedInAir = false;
                    enemyIsDead = false;
                    StopCoroutine("bashCounter");
                }           
                bashingTime = bashingTimeReset;               
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);  
                isBashing = false;
                _rigidbody.velocity = Vector2.zero; 
            }
                                      
        }            
        //------------------------BASHING------------------------------------
 
        //------------------------WALLJUMP------------------------------------
        isTouchingFront = Physics2D.OverlapBox(checkWall.position, checkWallSize,0, isJumpable);
        if(isTouchingFront)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }
        if(wallSliding)
        {
            isGrounded = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue));
            bashedInAir = false;
            dashedInAir = false;
            extraJumps = 1;
        }
        if(Input.GetButtonDown("Jump") && wallSliding)
        {
            wallJumping = true;           
            Invoke("SetWallJumpingToFalse", wallJumpTime);
            _rigidbody.velocity = Vector2.zero; 
        }
        
        if(wallJumping)
        {
            if(facingRight)
            {
                _rigidbody.velocity = new Vector2(xWallForce * -1, yWallForce);
            }
            else if (!facingRight)
            {
                _rigidbody.velocity = new Vector2(xWallForce * 1, yWallForce);
            }           
        }
        //------------------------WALLJUMP------------------------------------

        //------------------------CONTROLLER------------------------------------
        rStickX = Input.GetAxis("Horizontal");
        rStickY = Input.GetAxis("Vertical");

        if(rStickX != 0 &&  rStickY != 0)
        {
            useController = true;                 
        }
        if(useController)
        {
            Cursor.lockState = CursorLockMode.Locked;     
            Cursor.visible = false;
        }
        if(Input.GetButtonDown("Attack") && useController)
        {
            useController = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        //------------------------CONTROLLER------------------------------------        
    }   

    public void Bash()
    {
            _rigidbody.drag = 0;
            isBashing = true;
            canBash = false;
            _rigidbody.velocity = Vector2.zero;
            Vector3 rightTrigger = new Vector3(rStickX, rStickY, 0);
            if(useController)
            {
                bashDirection = rightTrigger;
            }else if (!useController){
                bashDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition)- _rigidbody.transform.position);
            }                
            bashDirection.z =0;
            if(bashDirection.x >0 )
                {
                    _rigidbody.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    _rigidbody.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            bashDirection = bashDirection.normalized;
            if(isGrounded)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            if(ignoreThisColliderBash != null)
            {
                Physics2D.IgnoreCollision(playerCollider, ignoreThisColliderBash);
            }
    }
    IEnumerator bashCounter()
    {
        float pauseTime = Time.realtimeSinceStartup + maxBashTime;
        while(Time.realtimeSinceStartup < pauseTime)
        {
            yield return null;
                   
        }
        if(_rigidbody.drag== bashChargeSlow)
        {
            print("times up chump");  
            cancelBashing();
            
            bashedInAir = true;
        }

    }

    public void cancelBashing()
    {        
        MovementSpeed = bashChargeMovementSpeed;
        _rigidbody.drag = 0;
        canBash = false;
        arrow.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D theCollision)
    {  
        _rigidbody.velocity = Vector2.zero;
        if (theCollision.gameObject.tag == "floor")
        {
            isgrounded = true;
            isJumping = false;
        }
        
        if(theCollision.gameObject.tag != "floor")
        {
            ignoreThisCollider = theCollision.collider;
            ignoreThisColliderBash = theCollision.collider;
            ignoreThisColliderSlam = theCollision.collider;
            if(theCollision.transform.position.x > transform.position.x)
            {
                knockbackRight = true;
            }
            else
            {
                knockbackRight = false; 
            } 
        }
         if(theCollision.gameObject.tag == "Enemy")
        {                     
            if(isBashing)
            {
                theCollision.gameObject.GetComponent<Enemy>().TakeDamage(bashDamage);
                if(theCollision.gameObject.GetComponent<Enemy>().isDead)
                {
                    enemyIsDead= true;
                }

            } 
        }
        if(theCollision.gameObject.tag == "Enemy" && isBashing == false && isDashing == false)
        {
            InvokeRepeating("DotDamage", 0f, 0.5f); 
        }  
    
    
        
    }
    void DotDamage()
    {
        characterStats.TakeDamage(15);   
    }
 
 
     void OnCollisionExit2D(Collision2D theCollision)
    {
     if (theCollision.gameObject.tag != "floor")
        {
            isgrounded = false;
        }
        CancelInvoke("DotDamage");
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
        _rigidbody.velocity = Vector2.zero;
    }

    void playerDirection()
    {
        if(movement < 0 && facingRight)
        {
            facingRight = !facingRight;
        }
        else if (movement > 0 && !facingRight)
        {
            facingRight = !facingRight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(feetPos.position, checkGroundSize);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(checkWall.position, checkWallSize);

    }
}
