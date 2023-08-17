using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private Image currentEquip;

    [SerializeField]
    private TMP_Text weaponName;
    [SerializeField]
    private TMP_Text weaponStats;
    private void Update()
    {
        updateCurrentWeapon();
    }

    void updateCurrentWeapon()
    {
        if(weapon == null)
        {
            currentEquip.enabled = false;
            weaponName.enabled= false;
        }
        else
        {
            currentEquip.enabled = true;
            weaponName.enabled = true;
            currentEquip.sprite = weapon.ItemImage;
            foreach(ItemParameter parameter in weapon.DefaultParameterList)
            {
                weaponStats.SetText(parameter.itemParameter.name + ":"+ parameter.value);
            }
            weaponName.SetText(weapon.name);
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