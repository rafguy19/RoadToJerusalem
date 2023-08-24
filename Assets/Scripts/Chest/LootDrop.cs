using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{

    [field:SerializeField]
    public Item InventoryItem { get; private set; }



    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    public List<Item> FoodLoot = new List<Item>();

    public List<Item> WeaponLoot = new List<Item>();
    public enum LootType
    {
        FOOD,
        WEAPON,
    }

    public LootType lootType;
    [SerializeField]
    private AudioSource audioSource;

    //animation duration
    [SerializeField]
    private float duration = 0.3f;

    int numberOfItems = 0;
    private void Start()
    {
        if(lootType == LootType.FOOD)
        {
            foreach (Item items in FoodLoot)
            {
                numberOfItems++;
            }
            //randomise the loot
            int randomLoot = UnityEngine.Random.Range(0, numberOfItems);
            InventoryItem = FoodLoot[randomLoot];
            //randomise the quanitity
            Quantity = UnityEngine.Random.Range(1, 5);
        }
       else if(lootType == LootType.WEAPON)
        {
            foreach (Item items in WeaponLoot)
            {
                numberOfItems++;
            }
            //randomise the loot
            int randomLoot = UnityEngine.Random.Range(0, numberOfItems);
            InventoryItem = WeaponLoot[randomLoot];
        }
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        
    }
    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        if(audioSource != null)
        {
            audioSource.Play();
        }

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            //animation -> Scale from current scale to scale = 0
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
