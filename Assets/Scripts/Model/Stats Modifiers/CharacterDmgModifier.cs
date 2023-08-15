using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDmgModifier : CharacterStatModifier
{
    //health bar system
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerController player = character.GetComponent<PlayerController>();

        player.UpdateDamage((int)val);
    }


}
