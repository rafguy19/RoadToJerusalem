using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Arrow : Item, IDestroyableItem//, IItemAction
{
    public enum ArrowType
    {
        NORMAL,
    }


    [field: SerializeField]
    public ArrowType arrowType { get; private set; }

    public string ActionName => "Equip";


    public ArrowType getArrowType()
    {
        return arrowType;
    }

    //public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    //{
    //    WeaponSystem weaponSystem= character.GetComponent<WeaponSystem>();
    //    if(weaponSystem != null)
    //    {
    //        //if Equipable item is null, set the DefaultParameterList. Else past the itemState of current weapon
    //        weaponSystem.SetWeapon(this, itemState == null ?
    //                            DefaultParameterList : itemState);

    //        foreach (ModifierData data in modifierData)
    //        {
    //            data.statModifier.AffectCharacter(character, data.value);
    //        }

    //        return true;
    //    }

    //    return false;
    //}
}
