using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    public List<Collider2D> ColliderToDisable;
    public List<SpriteRenderer> RendererToDisable;

    public void DisableEverything(bool state)
    {
        foreach(var c in ColliderToDisable)
        {
            c.enabled = state;
        }

        foreach (var c in RendererToDisable)
        {
            c.enabled = state;
        }
    }
}
