using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickUpSystem : MonoBehaviour
{
    private Inventory inventoryData;

    private void Start()
    {
        inventoryData = GetComponent<InventoryController>().inventoryData;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //set to_collide to the item drop
        ItemDrop item = collision.GetComponent<ItemDrop>();
        if (item != null)
        {
            int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            //if the item can be picked up
            if (remainder == 0)
            {
                //plays the animation
                item.DestroyItem();
            }
            //in case we tried to pick up item we can't store
            else
            {
                item.Quantity = remainder;
            }
        }

        // Check if the LootDrop is not null and "F" key is pressed
        LootDrop loot = collision.GetComponent<LootDrop>();
        if (loot != null)
        {
            int remainder = inventoryData.AddItem(loot.InventoryItem, loot.Quantity);
            // If the item can be picked up
            if (remainder == 0)
            {
                // Plays the animation
                loot.DestroyItem();
            }
            // In case we tried to pick up an item we can't store
            else
            {
                loot.Quantity = remainder;
            }

        }
    }
}
