using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private EquipableItem weapon;

    private Inventory inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;

    [SerializeField]
    private Image currentEquip;

    [SerializeField]
    private TMP_Text weaponName;
    [SerializeField]
    private TMP_Text weaponStats;
    [SerializeField]
    private SpriteRenderer weaponSprite;

    private void Start()
    {
        inventoryData = GetComponent<InventoryController>().inventoryData;
    }
    private void Update()
    {
        updateCurrentWeapon();
    }

    public EquipableItem getWeapon()
    {
        return weapon;
    }
    void updateCurrentWeapon()
    {
        if(weapon == null)
        {
            currentEquip.enabled = false;
            weaponName.enabled= false;
            weaponStats.enabled= false;
        }
        else
        {
            currentEquip.enabled = true;
            weaponName.enabled = true;
            weaponStats.enabled = true;
            currentEquip.sprite = weapon.ItemImage;
            string allParameterInfo = "";
            foreach (ItemParameter parameter in weapon.DefaultParameterList)
            {
                string parameterInfo = parameter.itemParameter.name + ": " + parameter.value;
                allParameterInfo += parameterInfo + " ";  // Append each parameter info

            }
            weaponStats.SetText(allParameterInfo);
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
        weaponSprite.sprite = weapon.ItemImage;
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