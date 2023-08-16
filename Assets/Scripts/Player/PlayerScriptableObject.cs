using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerScriptableObject : ScriptableObject
{
    public float maxHp;
    public float currentHp;
    public float speed;
    public float atkSpeed;
    public int damage;

    public void Reset()
    {
        currentHp = maxHp;
    }
}
