using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private Ui_Inventory_Page inventoryUI;

    [SerializeField]
    private Inventory inventoryData;

    public List<InventoryItem> initialItems = new List<InventoryItem>();

    //[SerializeField]
    //private AudioClip dropClip;

    //[SerializeField]
    //private AudioSource audioSource;

    //[SerializeField]
    //private CharacterController player;

    //[SerializeField]
    //private Animation errorAni;


    //[SerializeField]
    //private ErrorMessage errorMessage;
    private enum possibleErrors
    {
        none,
        full_health,
        weapon_broken
    }
    private int errorState = (int)possibleErrors.none;
    private void Start()
    {
        PrepareUI();
        PrepareInventoryData();
        inventoryUI.Hide();
    }

    private void PrepareInventoryData()
    {
        inventoryData.Init();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
            {
                continue;
            }
            inventoryData.AddItem(item);
        }
        
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key,item.Value.item.ItemImage, item.Value.quantity);
        }
    }

    private void PrepareUI()
    {
        inventoryUI.InitInventoryUi(inventoryData.Size);
        this.inventoryUI.OnDescriptionRequested += HandelDescriptionRequested;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }


    private void HandleItemActionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.IsEmpty)
        {
            return;
        }
        IItemAction itemAction = inventoryItem.item as IItemAction;
        if (itemAction != null)
        {


            inventoryUI.ShowItemAction(itemIndex);
            //runs perfrom action() when clicked

            inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));


        }
        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        if (destroyableItem != null)
        {

            inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
        }



    }

    private void DropItem(int itemIndex, int quantity)
    {
        inventoryData.RemoveItem(itemIndex, quantity);
        inventoryUI.ResetSelection();
        //audioSource.PlayOneShot(dropClip);
    } 

    public void PerformAction(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            return;
        }
        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        IItemAction itemAction = inventoryItem.item as IItemAction;

        //if player is full health and healing is performed, dont run
        //if(player.healthSystem.GetHealth() == 100 && itemAction.ActionName == "Use")
        //{
        //    errorState = (int)possibleErrors.full_health;
        //    errorMessage.SetDescription("Health is full");
        //}

        //if durability is = 0, unable to equip item
        for (int i = 0; i < inventoryItem.itemState.Count; i++)
        {
            if (inventoryItem.itemState[i].value <= 0)
            {
                errorState = (int)possibleErrors.weapon_broken;
                //errorMessage.SetDescription("Weapon is broken");
            }
        }

        //if(errorState != (int)possibleErrors.none)
        //{
        //    errorAni.Rewind();
        //    errorAni.Play();
        //}
        //else
        //{
            
        //}
        //destroy 1 item
        if (destroyableItem != null)
        {

            inventoryData.RemoveItem(itemIndex, 1);
        }
        //use the item
        if (itemAction != null)
        {
            //do action according to item picked
            itemAction.PerformAction(gameObject, inventoryItem.itemState);

            //audioSource.PlayOneShot(itemAction.actionSFX);

        }
        //reset posible error to none
        errorState = (int)possibleErrors.none;
        inventoryUI.ResetSelection();
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.IsEmpty)
        {
            return;
        }
        inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        inventoryData.SwapItems(itemIndex_1, itemIndex_2);
    }

    private void HandelDescriptionRequested(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }
 
        Item item = inventoryItem.item;

        string description = PrepareDiscription(inventoryItem);
        inventoryUI.UpdateDescription(itemIndex,item.ItemImage, item.name, description);
    }
    private string PrepareDiscription(InventoryItem inventoryItem)
    {
        //more efficinelty create strings
        StringBuilder sb = new StringBuilder();
        //make the base description
        sb.Append(inventoryItem.item.Description);
        //create new line
        sb.AppendLine();
        for (int i = 0; i < inventoryItem.itemState.Count; i++)
        {
            //lists out, Description | Current durability value | Max durability value
            sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" + $":{inventoryItem.itemState[i].value}/"+ $"{inventoryItem.item.DefaultParameterList[i].value}");
        }
        //print the list
        return sb.ToString();
    }
    //[SerializeField]
    //private GameEvent gameEvent;
    public void Update()
    {

        //open inventory UI when assigned button is held,disable inventory when game has won
        if (Input.GetKeyDown(KeyCode.Tab) /*&& gameEvent.Won != true*/)
            {
                //only open inventory is it is not enabled(shown)
                if(inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }

            }
        }
}
