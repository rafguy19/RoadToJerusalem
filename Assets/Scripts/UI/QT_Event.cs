using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QT_Event : MonoBehaviour
{
    public float fillAmount = 0;
    public float timeThreshhold = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            fillAmount += 0.1f;
        }
        timeThreshhold += Time.deltaTime;

        if(timeThreshhold >= 0.4f)
        {
            timeThreshhold = 0;
            fillAmount -= 0.05f;
        }

        if (fillAmount <= 0)
        {
            fillAmount = 0;
        }

        GetComponent<Image>().fillAmount = fillAmount;
    }
}
