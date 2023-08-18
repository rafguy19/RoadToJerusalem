using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{

    //used to turn on and off the weather individually.
    private bool Night;
    private bool Rain;
    private bool Fog;
    public GameObject RainDrops;
    public GameObject RainSplash;
    public GameObject FogActive;
    public GameObject DayNight;

   
    // Update is called once per frame
    void Update()
    {
      
        //check if the weather state is active
        //do it so that they can be individually activated using as less input as possible
        //input to turn on and off day night
    }
}
