using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    private Animator ar;
    private bool hasOpen;
    public List<GameObject> ChestLoot = new List<GameObject>(); 
    // Start is called before the first frame update
    void Start()
    {
        ar = GetComponent<Animator>();
        
    }
    //void Update()
    //{
    //    RNGitemDrop();
    //}
    private void OnTriggerEnter2D(Collider2D collided)
    {
        if(collided.tag=="Player")
        {
            ar.Play("ChestOpen");
            Destroy(gameObject, 6);
        }
    }
    //void RNGitemDrop()
    //{

    //}
}
