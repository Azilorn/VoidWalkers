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

        int rnd = Random.Range(0, 4);
        RewardType rewardType = RewardType.Ability;
        switch (rnd)
        {
            case 0:
                rewardType = RewardType.Item;
                GetRewardAnimation.rewardType = rewardType;
                itm = InventoryController.Instance.gameItems[Random.Range(0, InventoryController.Instance.gameItems.Count)];
                icon.sprite = itm.itemIcon;
                rewardName.text = itm.itemName;
                break;
            case 1:
                rewardType = RewardType.Ability;
                GetRewardAnimation.rewardType = rewardType;
                GameObject go = Resources.Load("AbilityTable") as GameObject;
                AbilityTable at = go.GetComponent<AbilityTable>();
                ability = at.Abilities[Random.Range(0, at.Abilities.Count)];
                icon.sprite = aIcon;
                rewardName.text = ability.abilityName;
                break;
            case 2:
                rewardType = RewardType.Creature;
                GetRewardAnimation.rewardType = rewardType;
                GameObject go1 = Resources.Load("CreatureTable") as GameObject;
                CreatureTable ct = go1.GetComponent<CreatureTable>();
                creatureSO = ct.Creatures[Random.Range(0, ct.Creatures.Count)];
                icon.sprite = creatureSO.creaturePlayerIcon;
                rewardName.text = creatureSO.creatureName;
                break;
            case 3:
                rewardType = RewardType.Relic;
                GetRewardAnimation.rewardType = rewardType;
                relic = InventoryController.Instance.relics[Random.Range(0, InventoryController.Instance.relics.Count)];
                icon.sprite = relic.icon;
                rewardName.text = relic.relicName;
                break;
            default:
                break;
        }
    }
}
