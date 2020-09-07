using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionRaycastCheck : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
           
        }
    }
}
