using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Ui_Inventory_Item item;


    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<Ui_Inventory_Item>();
    }


    void Update()
    {

        //set the canvas's postion = mosue position - canvas position[off set] (updates every frame)
        transform.position = canvas.transform.TransformPoint(Input.mousePosition - canvas.transform.position);
    }
    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }
    public void Toggle(bool val)
    {
        gameObject.SetActive(val); 
    }
}
