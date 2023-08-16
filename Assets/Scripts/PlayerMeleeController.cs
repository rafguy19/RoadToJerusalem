using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    public bool isOut;
    public bool isAttack;
    float speed = 4;
    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            var step = speed * Time.deltaTime;
            if (!isOut)//on the way out
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(1, 0, 0), step);
                if (transform.localPosition == new Vector3(1,0,0))
                {
                    isOut = true;
                }
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0.5f, 0, 0), step);
                if (transform.localPosition == new Vector3(0.5f, 0, 0))
                {
                    isOut = false;
                    isAttack = false;
                }
            }

        }
    }

    public void Melee()
    {
        isAttack = true;
    }
}
