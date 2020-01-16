using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldRewardMenuUI : MonoBehaviour
{
    public static Item itm;
    public static Ability ability;
    public static CreatureSO creatureSO;
    public static RelicSO relic;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI rewardName;
    [SerializeField] private Sprite aIcon;
    private void OnEnable()
    {
        SetReward();
    }
    public void SetReward()
    {
        if (itm != null || ability != null || creatureSO != null || relic != null)
            return;

        int rnd = Random.Range(0, 100);
        RewardType rewardType = RewardType.Ability;

        if (rnd < 60)
        {
            rewardType = RewardType.Item;
            GetRewardAnimation.rewardType = rewardType;
            itm = InventoryController.Instance.gameItems[Random.Range(0, InventoryController.Instance.gameItems.Count)];
            icon.sprite = itm.itemIcon;
            rewardName.text = itm.itemName;
        }
        else if (rnd >= 60 && rnd < 80)
        {
            rewardType = RewardType.Ability;
            GetRewardAnimation.rewardType = rewardType;
            GameObject go = Resources.Load("AbilityTable") as GameObject;
            AbilityTable at = go.GetComponent<AbilityTable>();
            ability = at.Abilities[Random.Range(0, at.Abilities.Count)];
            icon.sprite = aIcon;
            rewardName.text = ability.abilityName;
        }
        else if (rnd >= 80 && rnd < 100)
        {
            rewardType = RewardType.Relic;
            GetRewardAnimation.rewardType = rewardType;
            relic = InventoryController.Instance.relics[Random.Range(0, InventoryController.Instance.relics.Count)];
            icon.sprite = relic.icon;
            rewardName.text = relic.relicName;
        }
    }
}
