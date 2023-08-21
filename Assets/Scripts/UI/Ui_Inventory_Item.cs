using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

                                               
public class Ui_Inventory_Item : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IEndDragHandler, IDropHandler, IDragHandler
{
    // Start is called before the first frame update
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text quantityTxt;
    [SerializeField]
    private GameObject quantityBG;

    [SerializeField]
    //since selector itself is a parent, we cant just disable it (it will hide everything inside), thus we only
    //hide the Image, not the selector game object itself, therefore we need to have a seperate part just for selector
    private Image selectorImage;

    //making events for certain actions players do to the items in their inventory

    //when item ic licked, when "drop" option is clicked, when item is being dragged and when rmb is clicked on the item
    public event Action<Ui_Inventory_Item>OnItemClicked,OnItemDropped, OnItemBeginDrag, OnItemEndDrag, OnRightMouseButtonClick;

    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }
    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }


    //set data required for the items
    public void SetData(Sprite sprite, int quantity,bool isStackable)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        if (isStackable)
        {
            this.quantityBG.SetActive(true);
          
        }
        else
        {
            this.quantityBG.SetActive(false);
        }
       
        empty = false;
    }
    public void Select()
    {
        selectorImage.enabled = true;
    }
    //disable selector sprite when item is not selected
    public void Deselect()
    {
        selectorImage.enabled = false;
    }

    //run this when the mouse clicked on the item
 

    public void OnPointerClick(PointerEventData pointerData)
    {
        
        //this runs if it is a right click
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseButtonClick?.Invoke(this);
        }
        //this runs if it is a left click
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
        {
            return;
        }
        //? is used to check if the current dragging item is not null
        //if not null, exceute Invoke(this)
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropped?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
