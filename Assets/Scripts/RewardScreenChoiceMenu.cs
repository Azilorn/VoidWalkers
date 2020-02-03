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
        CoreGameInformation.currentRunDetails.GoldMade += gold;
        CreatureTable creatureTable = CreatureTable.Instance;
        for (int i = 0; i < 4; i++) {
            
            int rnd = Random.Range(0, 100);
            rewardContentUIs[i].NullObjects();
           
            if (rnd > 0 && rnd <= 60)
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
                rewardContentUIs[i].RewardName.text = itm.itemName;
                rewardContentUIs[i].RewardDescription.text = itm.bio;
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].Itm = itm;
            }
            //Relic
            else if (rnd > 60 && rnd <= 80) 
            {
                RelicSO relic = InventoryController.Instance.relics[Random.Range(0, InventoryController.Instance.relics.Count)];

                if (InventoryController.Instance.ownedRelics.ContainsKey(InventoryController.Instance.ReturnRelic(relic))) {
                    i--;
                    continue;
                }

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
                rewardContentUIs[i].RewardName.text = relic.relicName;
                rewardContentUIs[i].RewardDescription.text = relic.relicDescription;
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].Relic = relic;
            }
            //ability
            else if (rnd > 80 && rnd <= 100)
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
                rewardContentUIs[i].RewardName.text = a.abilityName;
                rewardContentUIs[i].RewardDescription.text = a.abilityBio;
                rewardContentUIs[i].gameObject.SetActive(true);
                rewardContentUIs[i].Ability = a;
            }

        }
    }
}
