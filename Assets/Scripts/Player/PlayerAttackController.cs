using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    //For attack controller
    bool isbow;

    //For Normal bow attack
    public GameObject bullet;
    [SerializeField]
    float BowPower;
    [SerializeField]
    float MaxBowCharge;
    [SerializeField]
    float bowCharge;
    bool canFire = true;
    bool canHit = true;
    public float arrowSpeed;
    [SerializeField]
    Slider bowPowerSlider;


    [SerializeField]
    public Inventory inventoryData;

    //For normal melee
    [SerializeField]
    private PlayerMeleeController playerMeleeController;

    //to check selected arrow
    private ArrowWheelController arrowWheelController;

    private void Start()
    {
        arrowWheelController = GameObject.FindGameObjectWithTag("ArrowWheel").GetComponent<ArrowWheelController>();
    }
    public void AttackSelector(int type)
    {
        switch (type)
        {
            case 1: //Normal Bow
                isbow = true;
                break;
            case 2: //Normal melee
                meleeAttack();
                isbow = false;
                break;
            default:
                break;
        }
    }

    private void bowAttack()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            int arrowAmt = 0;
            //check if selected arrow exists
            for (int x = 0; x < inventoryData.GetInvSize(); x++)
            {
                if (inventoryData.GetItemAt(x).item == null)
                {
                    continue;
                }

                else
                {
                    switch (arrowWheelController.selectedArrow)
                    {
                        case 1://normal arrow
                            if(inventoryData.GetItemAt(x).item.name == "NormalArrow")
                            {
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                            }
                            break;
                        case 2: //fire arrow
                            if (inventoryData.GetItemAt(x).item.name == "FireArrow")
                            {
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                            }
                            break;
                        case 3: //holy arrow
                            if (inventoryData.GetItemAt(x).item.name == "HolyArrow")
                            {
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                            }
                            break;
                        case 4: //unholy arrow
                            if (inventoryData.GetItemAt(x).item.name == "UnholyArrow")
                            {
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                            }
                            break;
                    }

                }
            }
            Debug.Log(arrowAmt);
        }
        if (Input.GetMouseButton(0) && canFire)
        {

            ChargeBow();
        }

        else if (Input.GetMouseButtonUp(0) && canFire && bowCharge > 2)
        {
            FireBow();
        }
        else // Not firing bow
        {
            if (bowCharge > 0f)
            {
                bowCharge -= 5 * Time.deltaTime;
            }
            else
            {
                bowCharge = 0f;
                canFire = true;
            }
            bowPowerSlider.value = bowCharge / 20;
        }
    }

    void ChargeBow()
    {
        bowCharge += 6.6f * Time.deltaTime;

        bowPowerSlider.value = bowCharge / 20;

        if (bowCharge > MaxBowCharge)
        {
            bowPowerSlider.value = 1;
            bowCharge = MaxBowCharge;
        }
    }

    void FireBow()
    {
        arrowSpeed = bowCharge * 1.5f;

        Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        canFire = false;
    }

    void meleeAttack()
    {

        if (Input.GetMouseButtonDown(0) && canHit)
        {
            playerMeleeController.Melee();
        }
    }

    private void Update()
    {
        if (isbow)
        {
            bowAttack();
        }
    }
}
