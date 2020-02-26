using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScrollPos : MonoBehaviour
{
    [SerializeField]float scrollValue = 1f;

    private void Start()
    {
        gameObject.GetComponent<Scrollbar>().value = 1f;
    }
    private void OnEnable()
    {
        gameObject.GetComponent<Scrollbar>().value = 1f;
    }
}
