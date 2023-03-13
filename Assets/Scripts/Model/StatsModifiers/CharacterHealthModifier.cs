using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterHealthModifier : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {

        PCManagement pc = character.GetComponent<PCManagement>();           //In order to access our character's script.

        if (pc.mainStats.currentHP != 0)                                 //Just to check if our current HP is not empty.
        {
            Debug.Log(pc.mainStats.currentHP + (int)val);
            pc.IncreaseHP((int)val);                                        //We increase the current HP by the val assigned to the item in the inspector.
            //pc.IncreaseXP((int)val);
        }
    }
}
