using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    public int sortingOrder = -21;
    public Renderer vfxRenderer;
    public string layer;

    void OnValidate()
    {
        vfxRenderer = GetComponent<Renderer>();
        if (vfxRenderer)
        {
            vfxRenderer.sortingOrder = sortingOrder;
            vfxRenderer.sortingLayerName = layer;
        }
    }
}
