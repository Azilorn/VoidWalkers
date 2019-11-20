using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardContentCreatureDetailsUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI level;
   [SerializeField] private TextMeshProUGUI primaryType;
   [SerializeField] private TextMeshProUGUI secondaryType;

    public void SetUI(int l, ElementType primary, ElementType secondary) {

        level.text = "LVL:" + l.ToString();
        primaryType.text = primary.ToString();

        if (secondary == ElementType.None)
        {
            secondaryType.transform.parent.gameObject.SetActive(false);
        }
        else {
            secondaryType.text = secondary.ToString();
            secondaryType.transform.parent.gameObject.SetActive(true);
        }
    }
   
}
