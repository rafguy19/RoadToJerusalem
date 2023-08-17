using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowWheelController : MonoBehaviour
{
    public Animator ar;
    private bool arrowwheelelected = false;
    public static int arrowID;

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
                Debug.Log("Arrow");
                break;
            case 2: // fire arrow selected
                Debug.Log("Fire Arrow");
                break;
            case 3: // holy arrow selected
                Debug.Log("Holy Arrow");
                break;
            case 4: // unholy arrow selected
                Debug.Log("Unholy Arrow");
                break;
            default:
                break;
        }
    }
}
