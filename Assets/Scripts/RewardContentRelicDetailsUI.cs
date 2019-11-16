using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardContentRelicDetailsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void SetUI(string d) {
        description.text = d;
    }
}
