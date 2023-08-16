using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
   
    private bool RainActive;
    private bool FogActive;
    public GameObject RainDrops;
    public GameObject RainSplash;
    public GameObject Fog;
    private float x;

    void Start()
    {
         x = Random.Range(0, 4);
        RainSplash.SetActive(false);
        RainDrops.SetActive(false);
        Fog.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

        
       //numbers from 0 to 3 if number is 0 it rains, if number is 1 fog activates, if number is 2 both rain and fog  activates, if number is 3 no weather state.
        switch(x)
        {
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
                RainDrops.SetActive(false);
                RainSplash.SetActive(false);
                Fog.SetActive(false);
                break;



        }



    }
}
