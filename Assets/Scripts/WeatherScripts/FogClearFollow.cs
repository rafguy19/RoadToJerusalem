using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FogClearFollow : MonoBehaviour
{

    public VisualEffect vfxRenderer;
    public VisualEffect vfxSmoker;
    // Update is called once per frame
    void Update()
    {
        if (vfxRenderer != null)
        {
           vfxRenderer.SetVector3("PlayerPosition", transform.localPosition);

        }
        if(vfxSmoker!=null)
        {
            vfxSmoker.SetVector3("PlayerPos", transform.localPosition);
        }
    }
}
