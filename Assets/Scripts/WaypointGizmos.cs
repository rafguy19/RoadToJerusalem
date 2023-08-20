using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WaypointGizmos : GameGizmos
{
    [SerializeField]
    private float radius = 1.0f;

    public override void Draw(GizmosRenderer r)
    {
        Gizmos.DrawWireSphere(r.gameObject.transform.position, radius);
    }
}
