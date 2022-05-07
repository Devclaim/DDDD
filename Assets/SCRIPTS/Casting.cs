using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : MonoBehaviour
{
    public float groundSlamSpeed= 39;
    public CharacterMovement2D characterControls;
    public float playerMovementSpeed;
    public float playerJumpForce;
    public  bool isCasting;
    private float castTimer;
    private float castProgress;
    public float castCooldown = 2;
    public float cooldownTime = 0;
    public Transform castingPoint;
    public GameObject fireballPrefab;
    public GameObject groundSlamPrefab;
    public GameObject minionPrefab;

    public int fireballManaCost;

    public int groundSlamManaCost;

    public int minionManaCost;

    public float fireballCD;
    public float minionCD;
    public float groundSlamCD;

    private Rigidbody2D _rigidbody;

    CharacterStats characterStats;

    private string activeSkill;

    public  bool inAirSlamming = false;

    void Start()

    {
        calculateCooldowns();
        calculateManaCosts();
        characterStats = FindObjectOfType<CharacterStats>();
        _rigidbody = this.GetComponent<Rigidbody2D>();
        playerMovementSpeed = characterControls.MovementSpeed;
        playerJumpForce = characterControls.JumpForce;
        activeSkill = "FairyMinion";
        activeSkillManager();
           
    }
    void Update()
    { 
         
        if(Input.GetButtonDown("ChangeSkill"))
        {
            if(activeSkill == "FairyMinion")
            {
                activeSkill = "Fireball";
                print(activeSkill);
            }
            else if(activeSkill == "Fireball")
            {
                activeSkill = "GroundSlam";
                print(activeSkill);
            }
            else if(activeSkill == "GroundSlam")
            {
                activeSkill = "FairyMinion";
                print(activeSkill);
            }
            activeSkillManager();
        } 
        if(characterControls.isDashing == false && characterControls.isBashing == false)
        {
        if(Input.GetButtonDown("Fire1")  && Time.time > cooldownTime && !isCasting)
        {
            if(activeSkill == "GroundSlam" && characterStats.getMana() >= groundSlamManaCost)
            {
                inAirSlamming = true;
                characterStats.LoseMana(groundSlamManaCost);                
            }
            else if(activeSkill == "Fireball" && characterStats.getMana() >= fireballManaCost)
            {
                castFireball();
                characterStats.LoseMana(fireballManaCost); 
            }
            else if(activeSkill == "FairyMinion" && characterStats.getMana() >= minionManaCost)
            {
                castFireball();
                characterStats.LoseMana(minionManaCost); 
            }
            else
            {
                print("YOU HAVE NO MANA!");
            }                      
        }
        else if(Input.GetButtonDown("Fire1")  && Time.time < cooldownTime)
        {
            print("BLA BLA I NEED SOME TIME COOLDOWN BLA BLA");
        }
        }
        if(inAirSlamming)
        {
            castGroundSlam();
        }
    }

    IEnumerator ChannelSpell()
    {
        isCasting = true;

        while (castProgress < castTimer)
        {
            yield return null;
            castProgress += Time.deltaTime;
            //progressBar.CurrentValue = castProgress / castTime; (TODO: progress bar)
        }
        if(activeSkill == "Fireball")
        {
            Fireball();
        }
        else if(activeSkill == "GroundSlam")
        {
            GroundSlam();
        }
        else if(activeSkill == "FairyMinion")
        {
            Minion();
        }        
        cooldownTime = Time.time + castCooldown;
        ResetCast();        
    }

    void castFireball()
    {
        characterControls.MovementSpeed = 1;
        characterControls.JumpForce = 3;
        StartCoroutine("ChannelSpell");
    }
    void Fireball()
    {
        Instantiate(fireballPrefab, castingPoint.position, castingPoint.rotation);
    }



    void castGroundSlam()
    {
        if(!characterControls.isGrounded)
        {
            _rigidbody.velocity = new Vector2(0, -1 * groundSlamSpeed);
            if(characterControls.ignoreThisColliderSlam != null)
            {
                Physics2D.IgnoreCollision(characterControls.playerCollider, characterControls.ignoreThisColliderSlam);
            }           
        }
        if(characterControls.isGrounded)
        {
            StartCoroutine("ChannelSpell");
            inAirSlamming = false;
            if(characterControls.ignoreThisColliderSlam != null)
            {                  
                Physics2D.IgnoreCollision(characterControls.playerCollider, characterControls.ignoreThisColliderSlam, false);
            }
            characterControls.ignoreThisColliderSlam = null;
        }       
    }
    void GroundSlam()
    {        
        if(characterControls.isGrounded)
        {
            Instantiate(groundSlamPrefab,castingPoint.position, castingPoint.rotation);
        }       
    }

    void Minion()
    {
        Instantiate(minionPrefab,castingPoint.position, Quaternion.Euler(0,0,0));
    }

    public void InterruptCast()
    {
        StopCoroutine("ChannelSpell");
        ResetCast();
    }

    void calculateManaCosts()
    {
        minionManaCost= 30;
        groundSlamManaCost= 20;
        fireballManaCost = 40;
    }
    void calculateCooldowns()
    {
        minionCD = 15;
        groundSlamCD = 3;
        fireballCD = 5;
    }

    void activeSkillManager()
    {
        if(activeSkill == "GroundSlam")
        {
            castTimer = 0.01f;
            castCooldown = groundSlamCD;
        }
        else if(activeSkill == "Fireball")
        {
            castTimer = 0.6f;
            castCooldown = fireballCD;
        }
        else if(activeSkill == "FairyMinion")
        {
            castTimer = 0.3f;
            castCooldown = minionCD;
        } 
    }

    void ResetCast()
    {
        isCasting = false;
        castProgress = 0f;
        characterControls.MovementSpeed = playerMovementSpeed;
        characterControls.JumpForce = playerJumpForce;
        //progressBar.gameObject.SetActive(false); (TODO: progress bar)
    }

}
