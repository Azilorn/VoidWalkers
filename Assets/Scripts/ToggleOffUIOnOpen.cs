using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOffUIOnOpen : MonoBehaviour
{
    public GameObject uiToToggle;
    private void OnEnable()
    {
        uiToToggle.SetActive(false);
    }
}
