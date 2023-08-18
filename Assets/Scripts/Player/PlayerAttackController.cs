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



    //For normal melee
    [SerializeField]
    private PlayerMeleeController playerMeleeController;

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
