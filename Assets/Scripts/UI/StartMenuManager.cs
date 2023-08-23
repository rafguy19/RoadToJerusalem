using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject page1;
    [SerializeField]
    private GameObject page2;
    [SerializeField]
    private Sprite Bow;
    [SerializeField]
    private Sprite WoodenSword;
    [SerializeField]
    private GameObject spriteGameobject;
    private Image sprite;
    public int weaponselector = 0;
    [SerializeField]
    private TextMeshProUGUI nameOfWeapon;
    [SerializeField]
    private TextMeshProUGUI descOfWeapon;
    // Start is called before the first frame update
    void Start()
    {
        sprite = spriteGameobject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }

    public void NextWeapon()
    {
        if(weaponselector == 1)//Change to bow
        {
            sprite.sprite = WoodenSword;
            weaponselector = 0;
            nameOfWeapon.text = "Wooden Sword";
            descOfWeapon.text = "A close combat weapon. It has decent attack speed with a short distance. Be wary as it has low durability. A true crusader's weapon";
        }
        else//Change to wooden sword
        {
            sprite.sprite = Bow;
            weaponselector = 1;
            nameOfWeapon.text = "Wooden Bow";
            descOfWeapon.text = "A ranged weapon. The damage and speed of arrows will vary on how much charge you add. It is limited based on the arrows you have.";
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
