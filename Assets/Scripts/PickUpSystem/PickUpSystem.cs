using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private Inventory inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //set to_collide to the item drop
        ItemDrop item = collision.GetComponent<ItemDrop>();
        if(item != null)
        {
            int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            //if the item can be picked up
            if(remainder == 0)
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
    }
}
