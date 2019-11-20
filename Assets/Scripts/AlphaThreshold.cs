using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaThreshold : MonoBehaviour
{
    private Image image;
    public float threshold = 0.5f;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = threshold;
    }
}
