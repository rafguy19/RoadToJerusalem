using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
   [System.Serializable]
   public class DropWeapons
   {
        public string name;
        public ScriptableObject item;
        public int ItemRarity;
   }
    public List<DropWeapons> ChestLoot = new List<DropWeapons>();
    private Animator ar;
    private bool hasOpen;
    // Start is called before the first frame update
    void Start()
    {
        ar = GetComponent<Animator>();
    }
   
    private void OnTriggerEnter2D(Collider2D collided)
    {
        if(collided.tag=="Player")
        {
            ar.Play("ChestOpen");
            Destroy(gameObject, 6);
        }
    }
    void CalcItemDrop()
    {
        int DropWhichItem = Random.Range(0, 4);
    }
}
