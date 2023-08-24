using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public GameObject RainDrops;
    public GameObject Torch;
    public GameObject RainSplash;
    public GameObject Fog;
    //public GameObject Night;
    public GameObject Day;
    private int x;

    void Start()
    {
         x = Random.Range(0, 8);
        RainSplash.SetActive(false);
        RainDrops.SetActive(false);
        Fog.SetActive(false);
        //Night.SetActive(false);
        Day.SetActive(true);
        Torch.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //create a function to check if weather switch case has run once already

        WeatherGenerationUpdate();
        if(Input.GetKeyDown("p"))
        {
            x++;
            if(x>8)
            {
                x = 0;
            }
        }
    }
    void WeatherGenerationUpdate()
    {
        //numbers from 0 to 7 to randomise weather and day or night time
        switch (x)
        {
            //random weather generation for each stage 
            case 0:
                //Just rain
           
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            break;
            case 1:
                //just fog
            RainDrops.SetActive(false);
            RainSplash.SetActive(false);
            Fog.SetActive(true);
            break;
            case 2:
                //both rain and fog in the day
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            Fog.SetActive(true);
            break;
            case 3:
                //only night
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(false);
            Day.SetActive(false);
            Torch.SetActive(true);
            //Night.SetActive(true);
            break;
            case 4:
                //rain and night
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            
            //Night.SetActive(true);
            break;
            case 5:
                //fog and night
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(true);
            //Night.SetActive(true);
            Torch.SetActive(true);
            break;
            case 6:
                //everything at night
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            Fog.SetActive(true);
            Torch.SetActive(true);
            //Night.SetActive(true);
            break;
            case 7:
                //nothing at all only day
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(false);
                //Night.SetActive(false);
            Torch.SetActive(false);
            Day.SetActive(true);
            break;
              



        }
    }
}
