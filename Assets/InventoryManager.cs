using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public Inventory inventoryData; // Reference to your Inventory Data ScriptableObject

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GameObject persists between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
