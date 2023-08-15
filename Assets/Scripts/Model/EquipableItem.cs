using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipableItem : Item, IDestroyableItem, IItemAction
{
    
    [SerializeField]
    private List<ModifierData> modifierData = new List<ModifierData>();
    public string ActionName => "Equip";

    [field: SerializeField]
    public AudioClip actionSFX { get; private set;}

    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        WeaponSystem weaponSystem= character.GetComponent<WeaponSystem>();
        if(weaponSystem != null)
        {
            //if Equipable item is null, set the DefaultParameterList. Else past the itemState of current weapon
            weaponSystem.SetWeapon(this, itemState == null ?
                                DefaultParameterList : itemState);

            foreach (ModifierData data in modifierData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }

            return true;
        }

        return false;
    }
}
