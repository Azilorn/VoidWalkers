using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardContentItemDetailsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI description;

    public void SetUI(string t, string d) {
        type.text = t;
        description.text = d;
    }
}
