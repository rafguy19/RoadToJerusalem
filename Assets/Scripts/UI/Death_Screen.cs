using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Screen : MonoBehaviour
{
    [SerializeField]
    private Animation death_enter;
    // Start is called before the first frame update
    void Start()
    {
        death_enter.Play();
    }
}
