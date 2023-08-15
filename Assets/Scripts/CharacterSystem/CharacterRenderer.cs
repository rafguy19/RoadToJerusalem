using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    public static readonly string[] action = { "AFK", "Run"};

    Animator animator;
    int lastDirection;
    private void Awake()
    {
        //cache the animator component
        animator = GetComponent<Animator>();
    }
}
