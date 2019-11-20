using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardScreenChoiceMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI goldText;
    public Sprite abilityIcon;
    public List<RewardContentUI> rewardContentUIs = new List<RewardContentUI>();

    public void SetUI() {

        int gold = Random.Range(10, 25 * PreBattleSelectionController.Instance.GameDetails.Floor);
        goldText.text = gold.ToString();
        PreBattleSelectionController.Instance.GameDetails.Gold += gold;
        GameObject go = Resources.Load("CreatureTable") as GameObject;
        CreatureTable creatureTable = go.GetComponent<CreatureTable>();
        for (int i = 0; i < 4; i++) {
            
            int rnd = Random.Range(0, 100);
            rewardContentUIs[i].NullObjects();
            //Creature
            if (rnd <= 25)
            {
                CreatureSO so = creatureTable.Creatures[Random.Range(0, creatureTable.Creatures.Count)];

                bool skip = false;  
                for (int j = 0; j < rewardContentUIs.Count; j++)
                {
                    if (rewardContentUIs[j].Creature == so) {
                        skip = true;
                    }
                }
                if (skip)
                {
                    rewardContentUIs[i].gameObject.SetActive(false);
                    i--;
                    continue;
                }
                rewardContentUIs[i].Icon.sprite = so.creaturePlayerIcon;
                rewardContentUIs[i].RewardType.text = "Creature";
                rewardContentUIs[i].RewardType.color = Color.magenta;
                rewardContentUIs[i].RewardTypeBorder.color = Color.magenta;
                rewardContentUIs[i].RewardTypeNudge.color = Color.magenta;
                rewardContentUIs[i].RewardBorder.color = Color.magenta;
                rewardContentUIs[i].RewardName.text = so.creatureName;
                rewardContentUIs[i].CreatureDetailsUI.SetUI((PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1) / 2, so.primaryElement, so.secondaryElement);
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].CreatureDetailsUI.gameObject.SetActive(true);
                rewardContentUIs[i].ItemDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].AbilityDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].RelicDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].Creature = so;
            }
            //Item
            else if (rnd > 25 && rnd <= 50)
            {
                Item itm = InventoryController.Instance.gameItems[Random.Range(0, InventoryController.Instance.gameItems.Count)];

                bool skip = false;
                for (int j = 0; j < rewardContentUIs.Count; j++)
                {
                    if (rewardContentUIs[j].Itm == itm)
                    {
                        skip = true;
                    }
                }
                if (skip)
                {
                    rewardContentUIs[i].gameObject.SetActive(false);
                    i--;
                    continue;
                }

                rewardContentUIs[i].Icon.sprite = itm.itemIcon;
                rewardContentUIs[i].RewardType.text = "Item";
                rewardContentUIs[i].RewardType.color = Color.cyan;
                rewardContentUIs[i].RewardTypeBorder.color = Color.cyan;
                rewardContentUIs[i].RewardTypeNudge.color = Color.cyan;
                rewardContentUIs[i].RewardBorder.color = Color.cyan;
                rewardContentUIs[i].RewardName.text = itm.itemName;
                rewardContentUIs[i].ItemDetailsUI.SetUI(itm.itemType.ToString(), itm.bio);
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].CreatureDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].ItemDetailsUI.gameObject.SetActive(true);
                rewardContentUIs[i].AbilityDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].RelicDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].Itm = itm;
            }
            //Relic
            else if (rnd > 50 && rnd <= 75) 
            {
                RelicSO relic = InventoryController.Instance.relics[Random.Range(0, InventoryController.Instance.relics.Count)];

                bool skip = false;
                for (int j = 0; j < rewardContentUIs.Count; j++)
                {
                    if (rewardContentUIs[j].Relic == relic)
                    {
                        skip = true;
                    }
                }
                if (skip)
                {
                    rewardContentUIs[i].gameObject.SetActive(false);
                    i--;
                    continue;
                }

                rewardContentUIs[i].Icon.sprite = relic.icon;
                rewardContentUIs[i].RewardType.text = "Relic";
                rewardContentUIs[i].RewardType.color = Color.green;
                rewardContentUIs[i].RewardTypeBorder.color = Color.green;
                rewardContentUIs[i].RewardTypeNudge.color = Color.green;
                rewardContentUIs[i].RewardBorder.color = Color.green;
                rewardContentUIs[i].RewardName.text = relic.relicName;
                rewardContentUIs[i].RelicDetailsUI.SetUI(relic.relicDescription);
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].CreatureDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].ItemDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].AbilityDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].RelicDetailsUI.gameObject.SetActive(true);
                rewardContentUIs[i].Relic = relic;
            }
            //ability
            else if (rnd > 75 && rnd <= 100)
            {
                Ability a = InventoryController.Instance.abilities[Random.Range(0, InventoryController.Instance.abilities.Count)];

                bool skip = false;
                for (int j = 0; j < rewardContentUIs.Count; j++)
                {
                    if (rewardContentUIs[j].Ability == a)
                    {
                        skip = true;
                    }
                }
                if (skip)
                {
                    rewardContentUIs[i].gameObject.SetActive(false);
                    i--;
                    continue;
                }

                rewardContentUIs[i].Icon.sprite = abilityIcon;
                rewardContentUIs[i].RewardType.text = "Ability";
                rewardContentUIs[i].RewardType.color = Color.yellow;
                rewardContentUIs[i].RewardTypeBorder.color = Color.yellow;
                rewardContentUIs[i].RewardTypeNudge.color = Color.yellow;
                rewardContentUIs[i].RewardBorder.color = Color.yellow;
                rewardContentUIs[i].RewardName.text = a.abilityName;
                rewardContentUIs[i].AbilityDetailsUI.SetUI(a);
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].CreatureDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].ItemDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].RelicDetailsUI.gameObject.SetActive(false);
                rewardContentUIs[i].AbilityDetailsUI.gameObject.SetActive(true);
                rewardContentUIs[i].Ability = a;
            }

        }
    }
}
