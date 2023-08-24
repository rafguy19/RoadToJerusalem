using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int starterWeaponID;
    public bool staterReceived;
    public int scene = 1;

    public List<InventoryItem> starterWeapon = new List<InventoryItem>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        staterReceived = false;

    }

}