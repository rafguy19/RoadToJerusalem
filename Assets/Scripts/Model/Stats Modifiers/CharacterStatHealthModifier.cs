using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifier : CharacterStatModifier
{
    //health bar system
    public override void AffectCharacter(GameObject character, float val)
    {
        CharacterController player = character.GetComponent<CharacterController>();

        if (player != null)
        {
            player.healthSystem.Heal((int)val);
                
        }    
    }


}
 