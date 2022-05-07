using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    public CharacterMovement2D characterMovement;
    public Animator animator;
    public int maxHealth = 100;

    public int maxMana = 100;
    public int manaRegen = 5;

    int currentMana;
    int currentHealth;
    public HealthBar healthBar;

    public HealthBar manaBar;
    

    void Start()
    {
        InvokeRepeating("RegenerateMana", 0.0f, 10f / manaRegen);
        manaBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        currentHealth = maxHealth;
        currentMana = maxMana;
        healthBar.setMaxValue(maxHealth);
        manaBar.setMaxValue(maxMana);
        healthBar.gameObject.SetActive(true);
        manaBar.gameObject.SetActive(true);
    }

    public void LoseMana(int mana)
    {
        currentMana -= mana;
        manaBar.setCurrentHealth(currentMana);
    }

    public int getMana()
    {
        return currentMana;
    }

    public void TakeDamage(int damage)
    {          
        if(characterMovement.isDashing == false && characterMovement.isBashing == false)
        {
            if(characterMovement.canTakeDamage == true)
            {
                currentHealth -= damage;
                characterMovement.knockbackTime = characterMovement.knockbackLength;
                characterMovement.StopCoroutine("bashCounter");
                characterMovement.cancelBashing();              
                animator.SetTrigger("Hurt");
                if(currentHealth <=0)
                {
                     Die();
                     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                healthBar.setCurrentHealth(currentHealth);
                characterMovement.canTakeDamage = false;      
                                       
            }
        }            
    }
    void Die()
    {
        animator.SetBool("Death", true);
        GetComponent<Collider2D>().enabled = false;
        this.gameObject.SetActive(false);
    }

    void RegenerateMana() 
    {
        if(currentMana < maxMana)
        {
            currentMana += manaRegen;
        }
        manaBar.setCurrentHealth(currentMana);

    }
}
