using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowSelectorWheelButton : MonoBehaviour
{

    public int ID;
    private Animator ar;
    public string itemName;
    public TextMeshProUGUI itemText;
    private bool selected = false;
    public Sprite icon;
    // Start is called before the first frame update
    void Start()
    {
        ar = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        ArrowWheelController.arrowID = ID;
    }

    public void Deselected()
    {
        selected = false;
        ArrowWheelController.arrowID = 0;
    }

    public void HoverEnter()
    {
        ar.SetBool("Hover", true);
        itemText.text = itemName;
    }
    public void HoverExit()
    {
        ar.SetBool("Hover", false);
        itemText.text = "";
    }
}
