using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RewardContentAbilityDetailsUI : MonoBehaviour
{
    private Ability ability;
    [SerializeField] private TextMeshProUGUI abilityPower;
    [SerializeField] private TextMeshProUGUI abilityAccuracy;

    public void SetUI(Ability a)
    {
        ability = a;
        abilityPower.text = "PWR: " + ability.abilityStats.power.ToString();
        abilityAccuracy.text = "ACC: " + ability.abilityStats.accuracy.ToString();

    }
}
