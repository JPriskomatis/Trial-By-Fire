using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public PCManagement pcManagement;

    public Animator anim;

    public Transform attackPoint;       //This is the point where our character's attack upon collision with inflict damage.

    public float attackRange;           //This is the radius of our attack point.

    public LayerMask enemyLayers;       //This is the detection that the target is an "enemy" in order for the character to inflict damage to them.

    private int attackDamage;            //This is how much damage our character will do.

    public float attackSpeed = 2f;      //This is how many times the character can attack per second.

    float nextAttackTime = 0f;          //This is the time when we can attack next.

    public int weaponDamage;

    private bool criticalStrike;

    public GameManagement game;


    [SerializeField]
    private InventoryPage inventory;

    // Update is called once per frame
    void Update()
    {
        if( inventory.isPlaying)
        {
            game.Pause();
        }
        else
        {
            game.Resume();
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        if (Time.time >= nextAttackTime)                            //This keeps track of our CURRENT time, where if its bigger than our next attack time then we can attack.
        {
            if (Input.GetMouseButtonDown(0))                        //We call the updated function every frame to detect if the player presses click to attack.
            {
                Attack();                                           //We call the attack function to initiate the attack.
                nextAttackTime = Time.time + 1f / attackSpeed;      //Our current time plus 1 devided by our attack time to see how much longer till our next attack.
            }
        }
        
    }

    void Attack()
    {
        anim.SetTrigger("Attack");                                                                                  //We start with the animation of the attack.

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);       //We detect every enemie we attacked in order to damage them.


        if (IsCrit(pcManagement.criticalStrike))                                                                    //We check whether the attack is a critical strike.
        {
            Debug.Log("Critical Strike!");
            attackDamage = CriticalDamage(Damage(weaponDamage, pcManagement.mainStats.strength));
        }
        else
        {
            Debug.Log("No Critical Strike!");
            attackDamage = Damage(weaponDamage, pcManagement.mainStats.strength);
        }


                                                                                                                    //weaponDamage is the static damage the weapon is dealing, we combine that we the strength of the character to 
                                                                                                                    //find how much the overal damage is.

        foreach (Collider2D enemy in hitEnemies)                                                                    //We loop through everything we collide with with the attack.
        {
            
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);                                                   //We access each object's(enemy we collided with the attack) Enemy script to call the "TakeDamage function in order to
                                                                                                                    //reduce their hp equal to the player's attack damage.
        }


    }



    /**
     * The following code will be producing the damage of the attack.
     * To do so we need to first calculate the base damage of a normal attack.
     * That would mean the damage the specific weapon will be doing in addition to the strength attribute of the character.
     * Next we will make a funcion to check whether the attack is a critical strike or not. A critical strike attack
     * is a powerful attack that deals additional damage. To see whether or not an attack is a critical strike one,
     * we roll a random number from 0-100, if that number is equal or less than the critical strike chances attribute of the character,
     * then the attack we will additional damage.
     * 
     * 
     */


    //Base damage.
    int Damage(int wpDamage, int strength)
    {
        int damage = wpDamage + strength;

        return damage;
    }

    //Now we need a function to calculate whether the strike was a critical strike or no.
    bool IsCrit(int critChances)
    {
        int random = Random.Range(0, 100);              //A critical srtike works based on percantage chances, if the character rolls a lower random number than their critical strike chances, then the
                                                        // strike will be a critical strike.
        Debug.Log(random);
        

        if (random <= critChances)
        {
            return true;
        }
        else
            return false;
    }
    
    //CriticalDamage if the attack is a critical strike.
    int CriticalDamage(int damage)
    {
        int critDamage = Mathf.FloorToInt((damage*1.5f));           //Since I want my damage numbers to be Integers (as they make easier calculations with the Health values), I need to round
                                                                    //the float number into an integer.
                                                                    //Note that this rounds DOWN.

        Debug.Log("Critical Damage is: "+critDamage);

        return critDamage;
    }


}