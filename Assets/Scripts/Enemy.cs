using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator anim;

    
    public int maxHealth = 100;

    public int currentHealth;

    public int armor;

    public HealthBarBehavior healthBar;

    public int xpReward;

    [SerializeField]
    public PCManagement pc;




    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);


    }



    // Update is called once per frame
    void Update()
    {
        
    }







    public void TakeDamage(int damage)
    {

        //Function to calculate damage
        int reducedDMG = damage - armor;                            //The damage the enemy takes is based on how much resistance they have.
                                                                    //Note that this is the final damage that has been calculated based on critical chances, weapon damage, strength.
                                                                    //This means that this is the FINAL damage modification before current health value is changed.

        currentHealth = currentHealth - reducedDMG;

        healthBar.SetHealth(currentHealth, maxHealth);

        //Function to calculate damage reduction based on armor of the target.

        Debug.Log(gameObject.name + " took: " + reducedDMG + " damage "+ currentHealth+ " remaining");             //For debugging purposes.

        
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0)                                                 //Reconsider adding a hurt animation before the death animation for the final blow.
        {
            healthBar.SetHealth(0, maxHealth);
            healthBar.DisableHealth();
            Die();                                                              //Play death animation.
        }
    }

    void Die()
    {
        
        Debug.Log(gameObject.name+" has died!");

        anim.SetBool("Death", true);

        pc.IncreaseXP(xpReward);

        GetComponent<Collider2D>().enabled = false;                             //We disable the collider of the target that is dead so that the player can pass through.
        foreach( Transform child in transform)                                  //We disable all the components of the enemy such as the visual cue for the dialogue, so that they don't appear when the enemy is dead.
        {
            child.gameObject.SetActive(false);
        }
        this.enabled = false;
    }

}
