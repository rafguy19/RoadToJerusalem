using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public GameObject RainDrops;
    public GameObject Moon;
    public GameObject RainSplash;
    public GameObject Fog;
    //public GameObject Night;
    public GameObject Day;
    private int x;
    private AudioSource rain;
    //public AudioClip Rain;
    private bool IsRaining;

    void Start()
    {
        rain = GetComponent<AudioSource>();
        x = Random.Range(0, 8);
        RainSplash.SetActive(false);
        RainDrops.SetActive(false);
        Fog.SetActive(false);
        //Night.SetActive(false);
        Day.SetActive(false);
        Moon.SetActive(false);
        IsRaining = false;
    }
    // Update is called once per frame
    void Update()
    {
        //create a function to check if weather switch case has run once already

       
        if (IsRaining == true)
        {
            rain.Play();
            IsRaining = false;
        }
        if (Input.GetKeyDown("p"))
        {
            x++;
            if(x>8)
            {
                x = 0;
            }
        }
        WeatherGenerationUpdate();
    }
    void WeatherGenerationUpdate()
    {
        //numbers from 0 to 7 to randomise weather and day or night time
        switch (x)
        {
            //random weather generation for each stage 
            case 0:
                //Just rain
            Day.SetActive(true);
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
                //rain.Play();
                IsRaining = true;
            break;
            case 1:
                //just fog
            Day.SetActive(true);
            rain.Stop();
            RainDrops.SetActive(false);
            RainSplash.SetActive(false);
            Fog.SetActive(true);
            break;
            case 2:
                //both rain and fog in the day
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            Fog.SetActive(true);
            Day.SetActive(true);
                IsRaining = true;
                break;
            case 3:
                //only night
            rain.Stop();
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(false);
            Day.SetActive(false);
            Moon.SetActive(true);
            
            break;
            case 4:
                //rain and night
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
                IsRaining = true;
                break;
            case 5:
                //fog and night
            rain.Stop();
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(true);
            Moon.SetActive(true);
            break;
            case 6:
                //everything at night
            RainDrops.SetActive(true);
            RainSplash.SetActive(true);
            Fog.SetActive(true);
            Moon.SetActive(true);
                IsRaining = true;
                break;
            case 7:
                //nothing at all only day
            rain.Stop();
            RainSplash.SetActive(false);
            RainDrops.SetActive(false);
            Fog.SetActive(false);
            Moon.SetActive(false);
            Day.SetActive(true);
            break;
              



        }
    }
}
