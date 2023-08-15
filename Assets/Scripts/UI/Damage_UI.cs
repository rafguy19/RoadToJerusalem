using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Damage_UI : MonoBehaviour
{

    public TMP_Text textComponent;
    [SerializeField]
    private CharacterController player;

    // Start is called before the first frame update
    private void Awake()
    {
        UpdateCurrentDamage(player.damage);
    }
    public void UpdateCurrentDamage(float currentDamage)
    {
        // Concatenate the text and variables
        string newText = "Current Damage: " + currentDamage;

        // Assign the new text to the TMP component
        textComponent.text = newText;
    }
}
