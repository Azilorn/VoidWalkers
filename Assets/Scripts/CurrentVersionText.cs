using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentVersionText : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        GetComponent<TextMeshProUGUI>().text = "Version: " + Application.version;
    }
}
