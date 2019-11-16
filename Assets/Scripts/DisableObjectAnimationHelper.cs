using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectAnimationHelper : MonoBehaviour
{
    // Start is called before the first frame update
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
