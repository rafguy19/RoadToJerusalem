using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private PlayerScriptableObject stats;
    private float moveSpeed;

    private Vector2 moveDirection;

    // For arrow
    public GameObject bullet;
    [SerializeField]
    float BowPower;
    [SerializeField]
    float MaxBowCharge;
    [SerializeField]
    float bowCharge;
    bool canFire = true;
    public float arrowSpeed;
    [SerializeField]
    Slider bowPowerSlider;

    // Start is called before the first frame update
    void Start()
    {
        stats.Reset();
        moveSpeed = stats.speed;
        rb = gameObject.GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        if (Input.GetMouseButton(0) && canFire)
        {
            ChargeBow();
        }
        else if(Input.GetMouseButtonUp(0) && canFire)
        {
            FireBow();
        }
        else // Not firing bow
        {
            if(bowCharge > 0f)
            {
                bowCharge -= Time.deltaTime;
            }
            else
            {
                bowCharge = 0f;
                canFire = true;
            }
            bowPowerSlider.value = bowCharge * 0.2f;
        }
    }

    private void FixedUpdate()
    {
        applyMovement();
    }

    void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    void applyMovement()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void ChargeBow()
    {
        bowCharge += 3 * Time.deltaTime;

        bowPowerSlider.value = bowCharge * 0.2f;

        if(bowCharge > MaxBowCharge)
        {
            bowPowerSlider.value = 1;
            bowCharge = MaxBowCharge;
        }
    }

    void FireBow()
    {
        arrowSpeed = bowCharge;

        Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        canFire = false;
    }
}
