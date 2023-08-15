using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ItemParameters : ScriptableObject
{
    [field: SerializeField]

    public string ParameterName { get; private set; }
}
