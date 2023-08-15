 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField]
    private TMP_Text errorMessage;
    // Start is called before the first frame update
    public void Awake()
    {
        ResetDescription();

    }

    public void ResetDescription()
    {
        this.errorMessage.text = "";
    }
    public void SetDescription(string itemDescription)
    {
  
        this.errorMessage.text = itemDescription;
    }
}
