using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    //For attack controller
    bool isbow;

    bool iscrossbow;
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

    int arrowLoctionInInv = 0;

    //audio
    public AudioSource outOfArrow;
    public AudioClip outOfAmmoClip;
    bool pullPlay;
    public AudioClip pullBow;
    public AudioClip shootBow;
    private void Start()
    {
        pullPlay = false;
        canFire = true;
        arrowWheelController = GameObject.FindGameObjectWithTag("ArrowWheel").GetComponent<ArrowWheelController>();
    }
    public void AttackSelector(int type)
    {
        switch (type)
        {
            case 1: //Normal Bow
                isbow = true;
                iscrossbow = false;
                break;
            case 2: //Normal melee
                meleeAttack();
                isbow = false;
                iscrossbow = false;
                break;
            case 3: //Crossbow
                isbow = false;
                iscrossbow = true;
                break;
            default:
                break;
        }
    }

    private void bowAttack()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            int arrowAmt = 0;
            bool arrowRemoved = false;
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
                            if (inventoryData.GetItemAt(x).item.name == "NormalArrow")
                            {
                                arrowLoctionInInv = x;
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                                if(arrowRemoved == false)
                                {

                                    arrowRemoved = true;
                                }
                            }
                            break;
                        case 2: //fire arrow
                            if (inventoryData.GetItemAt(x).item.name == "FireArrow")
                            {
                                arrowLoctionInInv = x;
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                                if (arrowRemoved == false)
                                {


                                    arrowRemoved = true;
                                }
                            }
                            break;
                        case 3: //holy arrow
                            if (inventoryData.GetItemAt(x).item.name == "HolyArrow")
                            {
                                arrowLoctionInInv = x;
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                                if (arrowRemoved == false)
                                {


                                    arrowRemoved = true;
                                }
                            }
                            break;
                        case 4: //unholy arrow
                            if (inventoryData.GetItemAt(x).item.name == "UnholyArrow")
                            {
                                arrowLoctionInInv = x;
                                arrowAmt += inventoryData.GetItemAt(x).quantity;
                                if (arrowRemoved == false)
                                {


                                    arrowRemoved = true;
                                }
                            }
                            break;
                    }
                }
            }

            if(arrowAmt > 0)
            {


                ChargeBow();

            }
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    //when out of arrow
                    outOfArrow.PlayOneShot(outOfAmmoClip);
                    Debug.Log("NO arrows");
                }

            }
        }

        else if (Input.GetMouseButtonUp(0) && canFire && bowCharge > MaxBowCharge/2)
        {
            pullPlay = false;
            outOfArrow.PlayOneShot(shootBow);
            CinemachineShake.Instance.ShakeCamera(3, .1f);
            inventoryData.RemoveItem(arrowLoctionInInv, 1);
            Debug.Log(inventoryData.GetItemAt(arrowLoctionInInv).quantity);
            FireBow();
        }
        else // Not firing bow
        {
            pullPlay = false;
            if (bowCharge > 0f)
            {
                bowCharge -= BowPower*4 * Time.deltaTime;
            }
            else
            {
                bowCharge = 0f;
                canFire = true;
            }
            bowPowerSlider.value = bowCharge / MaxBowCharge;
        }
    }

    void ChargeBow()
    {
        if (pullPlay == false)
        {
            Debug.Log("play");
            outOfArrow.PlayOneShot(pullBow);
            pullPlay = true;
        }
        bowCharge += BowPower * Time.deltaTime;

        bowPowerSlider.value = bowCharge / MaxBowCharge;

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

        //remove one selected arrow
        pullPlay = false;

    }

    void meleeAttack()
    {

        if (Input.GetMouseButtonDown(0) && canHit)
        {
            Debug.Log("Attack");

            playerMeleeController.Melee();
        }
    }

    private void Update()
    {
        if (isbow)
        {
            bowAttack();
        }
        if (iscrossbow)
        {
            crossbowAttack();
        }
    }
    void crossbowAttack()
    {
        int arrowAmt = 0;
        bool arrowRemoved = false;
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
                        if (inventoryData.GetItemAt(x).item.name == "NormalArrow")
                        {
                            arrowLoctionInInv = x;
                            arrowAmt += inventoryData.GetItemAt(x).quantity;
                            if (arrowRemoved == false)
                            {

                                arrowRemoved = true;
                            }
                        }
                        break;
                    case 2: //fire arrow
                        if (inventoryData.GetItemAt(x).item.name == "FireArrow")
                        {
                            arrowLoctionInInv = x;
                            arrowAmt += inventoryData.GetItemAt(x).quantity;
                            if (arrowRemoved == false)
                            {


                                arrowRemoved = true;
                            }
                        }
                        break;
                    case 3: //holy arrow
                        if (inventoryData.GetItemAt(x).item.name == "HolyArrow")
                        {
                            arrowLoctionInInv = x;
                            arrowAmt += inventoryData.GetItemAt(x).quantity;
                            if (arrowRemoved == false)
                            {
                                arrowRemoved = true;
                            }
                        }
                        break;
                    case 4: //unholy arrow
                        if (inventoryData.GetItemAt(x).item.name == "UnholyArrow")
                        {
                            arrowLoctionInInv = x;
                            arrowAmt += inventoryData.GetItemAt(x).quantity;
                            if (arrowRemoved == false)
                            {


                                arrowRemoved = true;
                            }
                        }
                        break;
                }
            }
        }

        if (arrowAmt > 0)
        {
            Reload();
            if (bowCharge < MaxBowCharge)
            {
                canFire = false;
            }
            if(Input.GetMouseButtonDown(0) && canFire)
            {
                arrowSpeed = bowCharge * 1.5f;

                Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

                //remove one selected arrow
                pullPlay = false;
                bowCharge = 0;
                outOfArrow.PlayOneShot(shootBow);
                CinemachineShake.Instance.ShakeCamera(3, .1f);
                inventoryData.RemoveItem(arrowLoctionInInv, 1);
                Debug.Log(inventoryData.GetItemAt(arrowLoctionInInv).quantity);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //when out of arrow
                outOfArrow.PlayOneShot(outOfAmmoClip);
                Debug.Log("NO arrows");
            }

        }
    }

    void Reload()
    {
        if (pullPlay == false)
        {
            Debug.Log("play");
            outOfArrow.PlayOneShot(pullBow);
            pullPlay = true;
        }
        bowCharge += BowPower * Time.deltaTime;
        bowPowerSlider.value = bowCharge / MaxBowCharge;

        if (bowCharge >= MaxBowCharge)
        {
            bowPowerSlider.value = 1;
            bowCharge = MaxBowCharge;
            canFire = true;
        }
    }
}
