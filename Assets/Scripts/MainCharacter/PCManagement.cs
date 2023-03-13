using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * This class will be mostly used to contain stats and attributes of the character
 * so that other scripts will call them from here to make calculations.
 * 
 * 
 * 
 */


public class PCManagement : MonoBehaviour
{




    public int experiencePoints;
    public int level;

    private int xpRequired;

    
    [System.Serializable]                    //Serializable translates a data format into one that unity can understand. Through Serialization we can edit these variables through the Inspection.
    public class MainAttributes                //We create a class for the character's stats so that its easier to adjust them through in the inspection
    {
        public int maxHP;
        public int currentHP;
        public int strength;
        public int magic;
        public int faith;
        public int agility;

    }

    [System.Serializable]
    public class SecondaryAttributes
    {
        public int charm;
        public int luck;
    }

    [System.Serializable]
    public class MiscellaneousAttributes
    {
        public int resistance;                      //How much damage is mitigated from attacks
        public int fame;                            //Reputation in other words
    }

    
    public int criticalStrike;





    public MainAttributes mainStats;               //We can access each attribute of the playerStats class through stats if we desire to do so.
    public SecondaryAttributes secondariesStats;
    public MiscellaneousAttributes miscellaneousStats;

    private void Awake()
    {
        xpRequired = 60;
    }



    public void IncreaseXP(int xp)
    {
        experiencePoints += xp;

        if (experiencePoints > xpRequired)
            LevelUp();
        xpRequired = xpRequired * 2;
    }


    public void IncreaseHP(int boostHP)
    {
        if(mainStats.currentHP < mainStats.maxHP)
        {
            mainStats.currentHP += boostHP;
            if (mainStats.currentHP > mainStats.maxHP)
            {
                mainStats.currentHP = mainStats.maxHP;
            }
        }
    }

    public void LevelUp()
    {
        Debug.Log("You have leveled up!");

    }
}
