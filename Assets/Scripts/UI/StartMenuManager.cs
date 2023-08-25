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
    private GameObject page3;
    [SerializeField]
    private GameObject page4;
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
    [SerializeField]
    private GameObject basiccontrols;
    [SerializeField]
    private GameObject systems;
    [SerializeField]
    private TextMeshProUGUI controlHeader;
    // Start is called before the first frame update
    void Start()
    {
        sprite = spriteGameobject.GetComponent<Image>();
    }

    public void controlPage()
    {
        if (basiccontrols.activeSelf) // go to systems page
        {
            controlHeader.text = "Systems";
            basiccontrols.SetActive(false);
            systems.SetActive(true);
        }
        else // go to controls page
        {
            controlHeader.text = "Basic Controls";
            basiccontrols.SetActive(true);
            systems.SetActive(false);
        }
    }

    public void LoadPage2()
    {
        HideAllPages();
        page2.SetActive(true);
    }

    public void LoadPage1()
    {
        HideAllPages();
        page1.SetActive(true);
    }

    public void LoadPage3()
    {
        HideAllPages();
        page3.SetActive(true);
    }

    public void LoadPage4()
    {
        HideAllPages();
        page4.SetActive(true);
    }

    private void HideAllPages()
    {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
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
        GameManager.instance.starterWeaponID = weaponselector;
    }

    public void Quit()
    {
        Application.Quit();
    }

}
