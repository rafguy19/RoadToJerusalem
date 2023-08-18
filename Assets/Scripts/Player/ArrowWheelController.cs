using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowWheelController : MonoBehaviour
{
    public Animator ar;
    private bool arrowwheelelected = false;
    public static int arrowID;
    public int selectedArrow;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            arrowwheelelected = !arrowwheelelected;
        }

        if (arrowwheelelected)
        {
            ar.SetBool("OpenArrowWheel", true);
        }
        else
        {
            ar.SetBool("OpenArrowWheel", false);
        }

        switch (arrowID)
        {
            case 0: // No arrow selected
                break;
            case 1: // arrow selected
                selectedArrow = 1;
                Debug.Log("Arrow");
                break;
            case 2: // fire arrow selected
                selectedArrow = 2;
                Debug.Log("Fire Arrow");
                break;
            case 3: // holy arrow selected
                selectedArrow = 3;
                Debug.Log("Holy Arrow");
                break;
            case 4: // unholy arrow selected
                selectedArrow = 4;
                Debug.Log("Unholy Arrow");
                break;
            default:
                break;
        }
    }
}
