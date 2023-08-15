using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosRenderer : MonoBehaviour
{
    public GameGizmos gizmo;

    private void OnDrawGizmos()
    {
        gizmo.Draw(this);
    }
}
