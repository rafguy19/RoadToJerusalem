using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;

    [field: SerializeField]
    public int Size { get; private set; }

    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
    public void Init()
    {
        inventoryItems = new List<InventoryItem>();
        //sets all inventory to be empty at the start
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }
    public int AddItem(Item item, int quantity, List<ItemParameter> itemState = null)
    {
        if(item.IsStackable == false)
        {
            for (int i = 0; i <= inventoryItems.Count; i++)
            {
                //if inventory is full, stops this loop
                while(quantity > 0 && IsInventoryFull()==false)
                {
                    //since if its not stackable, its quantity will always be 1
                    quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                   
                }
                InformAboutChange();
                return quantity;
            }
        }
        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFirstFreeSlot(Item item, int quantity, List<ItemParameter> itemState = null)
    {
        InventoryItem newItem = new InventoryItem
        { item = item,
            quantity = quantity,
            itemState = new List<ItemParameter>(itemState == null ? item.DefaultParameterList : itemState)
        };
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }

    //use Where() to find the items in a collection without using the foreach loop
    //if false is returned, the inventory is full
    private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;

    private int AddStackableItem(Item item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            //if its empty, it will be a different item, thus we won't stack
            if (inventoryItems[i].IsEmpty)
            {
                continue;
            }

            if (inventoryItems[i].item.ID==item.ID)
            {
                //use maxium stack - current quantity
                int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;
                //if current quantity is more than size left on current stack
                if(quantity> amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                    quantity -= amountPossibleToTake;
                    //loop the for loop to continue filling in the inv space
                }
                //if there is enough space to hold current quantity in the stack
                else
                {
                    //once 
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                    //there is no more item left needed to fill, break loop
                }
            }
        }
        //store remainding item quantity and find a new inv slot
        while(quantity>0 && IsInventoryFull() == false)
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }
        //leaves remainding item behind as inv is now full
        return quantity;
    }

    //if unable to pass index item into dictionary, tell code that this item is empty
    //prevents updating of every single inv slot as some slots might be empty
    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItems[i];
        }
        return returnValue;
    }
    public int GetInvSize()
    {
        return Size;
    }
    public InventoryItem GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    //call it this way to prevent passing and spliting the data inside inv controller
    internal void AddItem(InventoryItem item)
    {
        AddItem(item.item, item.quantity);
    }

    public void SwapItems(int itemIndex_1, int itemIndex_2)
    {
        InventoryItem item1 = inventoryItems[itemIndex_1];
        //swapping of data
        inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
        inventoryItems[itemIndex_2] = item1;
        InformAboutChange();
    }

    private void InformAboutChange()
    {
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if(inventoryItems.Count > itemIndex)
        {
            if (inventoryItems[itemIndex].IsEmpty)
            {
                return;
            }
            int remainder = inventoryItems[itemIndex].quantity - amount;
            //set the particular item to be an empty slot if the quanitity reaches 0
            if(remainder <= 0)
            {
    
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            }
            //change the item's quantity to be [current] - [amount to remove]
            else
            {
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(remainder);
            }
            //updates to UI
            InformAboutChange();
        }
    }

}   
[Serializable]
//it is much ez to add items this way as its variables can't be edited by other scripts compared to using a class
//thus we use a struct as it only passes values
public struct InventoryItem
{
    public int quantity;
    public Item item;
    public List<ItemParameter> itemState;

    //returns when the inventory is empty
    public bool IsEmpty => item == null;


    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity,
            itemState = new List<ItemParameter>(this.itemState)
        };
    }
    //use to allow null objects, Struct requires all varaibles to have value
    public static InventoryItem GetEmptyItem()=> new InventoryItem
    { 
        item = null, quantity=0,
        itemState = new List<ItemParameter>()
    };

}