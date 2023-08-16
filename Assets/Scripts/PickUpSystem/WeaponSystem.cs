using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private EquipableItem weapon;

    [SerializeField]
    private Inventory inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;

    [SerializeField]
    private Image currentEquiped;

    private void Update()
    {
        updateCurrentEquippedItem();
    }
    void updateCurrentEquippedItem()
    {
        if (weapon != null)
        {
            currentEquiped.enabled= true;
            currentEquiped.sprite = weapon.ItemImage;
        }
        else
        {
            currentEquiped.enabled = false;
        }
    }
    public void SetWeapon(EquipableItem weaponItemSO, List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }


    private void ModifyParameters()
    {
        foreach (var parameter in parametersToModify)
        {
            if(itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                //if this is -ve, we subtract frm it, else add
                float newValue = itemCurrentState[index].value + parameter.value;

                if(newValue <= 0)
                {
                    newValue = 0;
                }
                //updating old value to new one
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
                
            };
        }
    }
}