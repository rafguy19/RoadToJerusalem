using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifier : CharacterStatModifier
{
    //health bar system
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerHealth player = character.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.Heal((int)val);
                
        }    
    }


}
 