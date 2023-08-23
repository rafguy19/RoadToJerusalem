using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WChestItemDrop : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioSource;

    //animation duration
    [SerializeField]
    private float duration = 0.3f;

    //for chest
    private bool hasOpen;

    public Transform gameObjectPos;

    public GameObject lootDrop;

    private void Start()
    {
        hasOpen = false;
       
   
    }
    void OnTriggerEnter2D(Collider2D collided)
    {
        if (collided.tag == "Player")
        {
            hasOpen = true;
            ItemDrop();
        }
    }
    //do rng using switch case
    void ItemDrop()
    {

        Instantiate(lootDrop, this.transform);
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            //animation -> Scale from current scale to scale = 0
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        //Destroy(gameObject);
        

    }
    
}


