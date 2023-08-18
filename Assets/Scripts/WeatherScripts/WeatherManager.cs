using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public GameObject RainDrops;
    public GameObject RainSplash;
    public GameObject Fog;
    public GameObject Night;
    private int x;

    void Start()
    {
         x = Random.Range(0, 5);
        RainSplash.SetActive(false);
        RainDrops.SetActive(false);
        Fog.SetActive(false);
        Night.SetActive(false);
       
    }
    // Update is called once per frame
    void Update()
    {
        //create a function to check if weather switch case has run once already

        WeatherGenerationUpdate();
        if(Input.GetKeyDown("p"))
        {
            x++;
            if(x>=5)
            {
                x = 0;
            }
        }
    }
    void WeatherGenerationUpdate()
    {
        //numbers from 0 to 4 if number is 0 it rains, if number is 1 fog activates, if number is 2 both rain and fog  activates, if number is 3 Night time is active
        //if number is 4 no state active
        switch (x)
        {
            //random weather generation for each stage 
            case 0:
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            break;
            case 1:
            Fog.SetActive(true);
            break;
            case 2:
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            Fog.SetActive(true);
            break;
            case 3:
            Night.SetActive(true);
            break;
            case 4:
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(false);
            Night.SetActive(false);
            break;

        }
    }
}
