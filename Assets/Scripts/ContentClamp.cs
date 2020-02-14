using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentClamp : MonoBehaviour
{
   [SerializeField] float bottom, top;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector3(rect.offsetMax.x, top);
        rect.offsetMin = new Vector3(rect.offsetMin.x, top);
    }

    // Update is called once per frame
    void Update()
    {
        if (rect.offsetMax.y > top)
        {
            rect.offsetMax = new Vector3(rect.offsetMax.x, top);
        }
        if (rect.offsetMin.y < bottom)
        {
            rect.offsetMin = new Vector3(rect.offsetMin.x, top);
        }
    }
}
