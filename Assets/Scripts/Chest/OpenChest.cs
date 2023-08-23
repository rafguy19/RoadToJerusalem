using System.Collections;
using UnityEngine;
public class OpenChest : MonoBehaviour
{
   
   
    private Animator ar;

    public GameObject lootDrop;

    bool chestOpened;
    // Start is called before the first frame update
    void Start()
    {
        ar = GetComponent<Animator>();
        chestOpened = false;
    }
   
    private void OnTriggerEnter2D(Collider2D collided)
    {
        if(collided.tag=="Player" && chestOpened == false)
        {

            StartCoroutine(ChestOpenCoroutine());
            //Destroy(gameObject, 1);
        }
       
    }
    IEnumerator ChestOpenCoroutine()
    {
        chestOpened = true;
        ar.Play("ChestOpen");
        ar.Play("NatureChestOpen");

        
        yield return new WaitForSeconds(1);

        Instantiate(lootDrop, this.transform);
    }
}
