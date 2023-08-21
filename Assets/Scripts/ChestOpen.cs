using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public GameObject WeaponChest;
    public GameObject FoodChest;
    private Animator ar;

    
    // Start is called before the first frame update
    void Start()
    {
        ar = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
