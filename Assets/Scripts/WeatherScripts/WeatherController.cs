using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{

    //bool for on off weather aka rain and fog, and also switching between day and night
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
        //detecting if rain is active
        //if (RainDrops.activeSelf && RainSplash.activeSelf)
        //{
        //    Rain = true;

        //}
        //if (Rain = true)
        //{
        //    RainDrops.SetActive(true);
        //    RainSplash.SetActive(true);
        //}
        //else if (Rain = false)
        //{
        //    RainDrops.SetActive(false);
        //    RainSplash.SetActive(false);
        //}
        //if(FogActive.activeSelf)
        //{
        //    Fog = true;
        //}
        //else if(Fog=false)
        //{
        //    FogActive.SetActive(false);
        //}
        //if(Night.activeSelf)
        //{
        //    Night = true;
        //}
        //else if(Night=false)
        //{
        //    DayNight.SetActive(false);
        //}
        
        //input to turn on and off day night
    }
}
