﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardContentUI : MonoBehaviour
{

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI rewardName;
    [SerializeField] private TextMeshProUGUI rewardType;
    [SerializeField] private Image rewardBorder;
    [SerializeField] private Image rewardTypeBorder;
    [SerializeField] private Image rewardTypeNudge;
    [SerializeField] private RewardContentCreatureDetailsUI creatureDetailsUI;
    [SerializeField] private RewardContentItemDetailsUI itemDetailsUI;
    [SerializeField] private RewardContentRelicDetailsUI relicDetailsUI;
    [SerializeField] private RewardContentAbilityDetailsUI abilityDetailsUI;

    private CreatureSO creature;
    private Item itm;
    private RelicSO relic;
    private Ability ability;

    public RewardContentCreatureDetailsUI CreatureDetailsUI { get => creatureDetailsUI; set => creatureDetailsUI = value; }
    public RewardContentItemDetailsUI ItemDetailsUI { get => itemDetailsUI; set => itemDetailsUI = value; }
    public RewardContentRelicDetailsUI RelicDetailsUI { get => relicDetailsUI; set => relicDetailsUI = value; }
    public RewardContentAbilityDetailsUI AbilityDetailsUI { get => abilityDetailsUI; set => abilityDetailsUI = value; }
    public Image Icon { get => icon; set => icon = value; }
    public TextMeshProUGUI RewardName { get => rewardName; set => rewardName = value; }
    public TextMeshProUGUI RewardType { get => rewardType; set => rewardType = value; }
    public CreatureSO Creature { get => creature; set => creature = value; }
    public Item Itm { get => itm; set => itm = value; }
    public RelicSO Relic { get => relic; set => relic = value; }
    public Ability Ability { get => ability; set => ability = value; }
    public Image RewardTypeBorder { get => rewardTypeBorder; set => rewardTypeBorder = value; }
    public Image RewardTypeNudge { get => rewardTypeNudge; set => rewardTypeNudge = value; }
    public Image RewardBorder { get => rewardBorder; set => rewardBorder = value; }

    public void NullObjects()
    {
        Creature = null;
        Itm = null;
        Relic = null;
        Ability = null;
    }

    public void AddObjectToPlayer()
    {
        Debug.Log("Creature: " + Creature + " Item: " + Itm  + " Relic: " + Relic  + " Ability: " + Ability );
        if (Creature != null)
        {
           //TODO Create Creature Backlog mechanic
        }
        if (Itm != null)
        {
            InventoryController.Instance.AddItem(Itm);
        }
        if (Relic != null)
        {
            InventoryController.Instance.AddRelic(relic);
        }
        if (Ability != null)
        {
            InventoryController.Instance.AddAbility(ability);

        }
    }
}
