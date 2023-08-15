using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Inventory_Page : MonoBehaviour
{
    [SerializeField]
    private Ui_Inventory_Item item_Prefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private UiInventoryDescription itemDescription;

    [SerializeField]
    private MouseFollower mousefollower;

    List<Ui_Inventory_Item> listOfUiItems = new List<Ui_Inventory_Item>();

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

    //now we need 2 indicies to get data of draged item and "to be dropped" item
    public event Action<int, int> OnSwapItems;
    //gets current array index, -1 as it is outside the array, aka nothing is selected
    private int currenlyDraggedItemIndex = -1;

    [SerializeField]
    private ItemActionPannel actionPannel;
    private void Awake()
    {
        Hide();
        mousefollower.Toggle(false);
        itemDescription.ResetDescription();
    }
    //initialising our inventory UI
    public void InitInventoryUi(int inventory_size)
    {
        for (int i = 0; i < inventory_size; i++)
        {
            //makes an Item depending on inventory size
            Ui_Inventory_Item uiItem = Instantiate(item_Prefab, Vector3.zero, Quaternion.identity);

            //set the created item to be parented to our content panel
            uiItem.transform.SetParent(contentPanel);

            //add the created item into the list of items we have currently
            listOfUiItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDropped += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseButtonClick += HandleShowItemActions;
        }
    }


    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuanitity)
    {
        //check if item exist
        if(listOfUiItems.Count > itemIndex)
        {
            listOfUiItems[itemIndex].SetData(itemImage, itemQuanitity);
        }
    }
    private void HandleShowItemActions(Ui_Inventory_Item inventory_item_ui)
    {
        //gets current item's array index
        int index = listOfUiItems.IndexOf(inventory_item_ui);
        //if no item, array index will be -1
        if (index == -1)
        {
            return;
        }
        OnItemActionRequested?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprtie, int quantity)
    {
        mousefollower.Toggle(true);
        mousefollower.SetData(sprtie, quantity);
    }
    private void HandleBeginDrag(Ui_Inventory_Item inventory_item_ui)
    {
        //gets current item's array index
        int index = listOfUiItems.IndexOf(inventory_item_ui);
        //if no item, array index will be -1
        if (index == -1)
        {
            return;
        }
        currenlyDraggedItemIndex = index;
        //to indicate we are selecting on that particular item, avoid confusion when there is multiple item on our inv
        HandleItemSelection(inventory_item_ui);
        OnStartDragging?.Invoke(index);
        
    }
    private void HandleEndDrag(Ui_Inventory_Item inventory_item_ui)
    {

        ResetDraggedItem();
    }

    private void HandleSwap(Ui_Inventory_Item inventory_item_ui)
    {
        //gets current item's array index
        int index = listOfUiItems.IndexOf(inventory_item_ui);

        //if no item, array index will be -1
        if (index == -1)
        {

            return;
        }
        OnSwapItems?.Invoke(currenlyDraggedItemIndex, index);
        HandleItemSelection(inventory_item_ui);
    }

    private void ResetDraggedItem()
    {
        mousefollower.Toggle(false);
        currenlyDraggedItemIndex = -1;
    }

   
    private void HandleItemSelection(Ui_Inventory_Item inventory_item_ui)
    {
        int index = listOfUiItems.IndexOf(inventory_item_ui);
        if(index == -1)
        {
            return;
        }
        OnDescriptionRequested?.Invoke(index);
    }

    //shows and hide inventory UI with an assingned button
    public void Show()
    {
        gameObject.SetActive(true);
        //reset selection when inventory is re-opened
        ResetSelection();

    }

    public void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    public void AddAction(string actionName, Action peformAction)
    {
        actionPannel.AddButton(actionName, peformAction);
    }
    public void ShowItemAction(int itemIndex)
    {
        actionPannel.Toggle(true);
        actionPannel.transform.position = listOfUiItems[itemIndex].transform.position;
    }
    private void DeselectAllItems()
    {
        //to select every single item, loops for x amount of items in out inv
        foreach (Ui_Inventory_Item item in listOfUiItems)
        {
            item.Deselect();
        }
        actionPannel.Toggle(false);
    }

    public void Hide()
    {
        actionPannel.Toggle(false);
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage,name, description);
        //deslect all items first, then select current item
        DeselectAllItems();
        listOfUiItems[itemIndex].Select();
    }

    internal void ResetAllItems()
    {
        foreach (var item in listOfUiItems)
        {
            item.ResetData();
            item.Deselect();
        }
    }
}
