using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    [HideInInspector]
    public bool isOut;
    [HideInInspector]
    public bool isAttack;
    float speed = 4;
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

    }
    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
           

            sr.enabled = true;
            var step = speed * Time.deltaTime;
            if (!isOut)//on the way out
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0.5f, 0, 0), step);
                if (transform.localPosition == new Vector3(0.5f,0,0))
                {
                    isOut = true;
                }
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0.0f, 0, 0), step);
                if (transform.localPosition == new Vector3(0.0f, 0, 0))
                {
                    isOut = false;
                    isAttack = false;
                }
            }

        }
        else
        {
            sr.enabled = false;
        }
    }

    public void Melee()
    {
        isAttack = true;
    }
}
