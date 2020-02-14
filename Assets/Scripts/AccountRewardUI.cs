using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AccountRewardUI : MonoBehaviour
{
    public CreatureSO creature;
    public Image icon;
    public TextMeshProUGUI creatureName;
    public RectTransform rect;


    public void SetReward(CreatureSO c) {

        rect = GetComponent<RectTransform>();
        creature = c;
        icon.sprite = creature.creaturePlayerIcon;
        creatureName.text = creature.creatureName;
    }
}
