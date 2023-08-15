using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private PlayerScriptableObject stats;

    //player stats
    private float moveSpeed;
    private float atkDmg;

    private Vector2 moveDirection;
    private Vector2 direction;

    // For arrow
    public GameObject bullet;
    [SerializeField]
    float BowPower;
    [SerializeField]
    float MaxBowCharge;
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
        faceMouse();

        if (Input.GetMouseButton(0) && canFire)
        {
            ChargeBow();
        }
        else if(Input.GetMouseButtonUp(0) && canFire)
        {
            FireBow();
        }
        else
        {
            if(bowCharge > 0f)
            {
                bowCharge -= 1f * Time.deltaTime;
            }
            else
            {
                bowCharge = 0f;
                canFire = true;
            }
            bowPowerSlider.value = bowCharge;
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

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        transform.up = direction;
    }

    void ChargeBow()
    {
        bowCharge += Time.deltaTime;

        bowPowerSlider.value = bowCharge;

        if(bowCharge > MaxBowCharge)
        {
            bowPowerSlider.value = MaxBowCharge;
        }
    }

    void FireBow()
    {
        if (bowCharge > MaxBowCharge) bowCharge = MaxBowCharge;

        arrowSpeed = bowCharge + BowPower;

        Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        canFire = false;
    }

    //player modifier
    public void UpdateDamage(int newDmgValue)
    {
        atkDmg = newDmgValue;
    }
}
