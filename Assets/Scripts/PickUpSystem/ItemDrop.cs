using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [field:SerializeField]
    public Item InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    //animation duration
    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
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
