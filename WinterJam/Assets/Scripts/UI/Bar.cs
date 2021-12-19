using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Bar : MonoBehaviour
{
    private RawImage image;

    public int Percentage;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float p = Percentage / 100.0f;
        float x = (p - 1);
        float w = p;
        image.uvRect = new Rect(x, image.uvRect.y, w, image.uvRect.height);
    }
}
