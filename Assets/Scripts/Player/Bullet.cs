using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float force;
    private PlayerAttackController player;
    public float lifetime;
    private ArrowWheelController arrowWheelController;
    [SerializeField]
    private Sprite arrow;
    [SerializeField]
    private Sprite firearrow;
    [SerializeField]
    private Sprite holyarrow;
    [SerializeField]
    private Sprite unholyarrow;


    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        arrowWheelController = GameObject.FindGameObjectWithTag("ArrowWheel").GetComponent<ArrowWheelController>();
        ArrowRenderer();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackController>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * player.arrowSpeed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot+135);
        Destroy(gameObject, lifetime);
    }

    public void ArrowRenderer()
    {
        switch (arrowWheelController.selectedArrow)
        {
            case 1:
                sr.sprite = arrow;
                break;
            case 2:
                sr.sprite = firearrow;
                break;
            case 3:
                sr.sprite = holyarrow;
                break;
            case 4:
                sr.sprite = unholyarrow;
                break;
            default:
                break;
        }
    }
}
