using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardScreenChoiceMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject takeAll;
    public TextMeshProUGUI goldText;
    public Sprite abilityIcon;
    public List<RewardContentUI> rewardContentUIs = new List<RewardContentUI>();


    private void OnEnable()
    {
        takeAll.SetActive(true);
    }
    public void SetUI() {

        int gold = Random.Range(25 * PreBattleSelectionController.Instance.GameDetails.Floor, 50 * PreBattleSelectionController.Instance.GameDetails.Floor);
        goldText.text = gold.ToString();
        PreBattleSelectionController.Instance.GameDetails.Gold += gold;
        CoreGameInformation.currentRunDetails.GoldMade += gold;
        CreatureTable creatureTable = CreatureTable.Instance;

        int itemCount = Random.Range(3, 5);

        for (int i = 0; i < itemCount; i++) {
            
            int rnd = Random.Range(0, 100);
            rewardContentUIs[i].NullObjects();
           
            if (rnd >= 0 && rnd <= 60)
            {
                Item itm = InventoryController.Instance.gameItems[Random.Range(0, InventoryController.Instance.gameItems.Count)];

                if ((int)itm.floorAvailable <= PreBattleSelectionController.Instance.GameDetails.Floor)
                {
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
                    else
                    {
                        rewardContentUIs[i].Icon.sprite = itm.itemIcon;
                        rewardContentUIs[i].RewardName.text = itm.itemName;
                        rewardContentUIs[i].RewardDescription.text = itm.bio;
                        rewardContentUIs[i].gameObject.SetActive(true);
                        rewardContentUIs[i].Itm = itm;
                        rewardContentUIs[i].Collected = false;
                    }
                }
                else {
                    i--;
                    continue;
                }

              
            }
            //Relic
            else if (rnd > 60 && rnd <= 80) 
            {
                RelicSO relic = InventoryController.Instance.relics[Random.Range(0, InventoryController.Instance.relics.Count)];

                if ((int)relic.floorAvailable <= PreBattleSelectionController.Instance.GameDetails.Floor)
                {
                    if (InventoryController.Instance.ownedRelics.ContainsKey(InventoryController.Instance.ReturnRelic(relic)))
                    {
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
                    else
                    {
                        rewardContentUIs[i].Icon.sprite = relic.icon;
                        rewardContentUIs[i].RewardName.text = relic.relicName;
                        rewardContentUIs[i].RewardDescription.text = relic.relicDescription;
                        rewardContentUIs[i].gameObject.SetActive(true);
                        rewardContentUIs[i].Relic = relic;
                        rewardContentUIs[i].Collected = false;
                    }
                }
                else
                {
                    i--;
                    continue;
                }               
            }
            //ability
            else if (rnd > 80 && rnd <= 100)
            {
                Ability a = InventoryController.Instance.abilities[Random.Range(0, InventoryController.Instance.abilities.Count)];

                if ((int)a.floorAvailable <= PreBattleSelectionController.Instance.GameDetails.Floor)
                {

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
                    else
                    {
                        rewardContentUIs[i].Icon.sprite = abilityIcon;
                        rewardContentUIs[i].RewardName.text = a.abilityName;
                        rewardContentUIs[i].RewardDescription.text = a.abilityBio;
                        rewardContentUIs[i].gameObject.SetActive(true);
                        rewardContentUIs[i].Ability = a;
                        rewardContentUIs[i].Collected = false;
                    }
                }
                else
                {
                    i--;
                    continue;
                }

            }
            rewardContentUIs[i].gameObject.SetActive(true);
        }

        for (int i = itemCount; i < 5; i++) {
            rewardContentUIs[i].gameObject.SetActive(false);
        }
    }

    public void TakeAllItems() {

        foreach (RewardContentUI item in rewardContentUIs)
        {
            item.AddObjectToPlayer();
        }
    }
}
