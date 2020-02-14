using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldRewardMenuUI : MonoBehaviour
{
    public static List<Item> itm = new List<Item>();
    public static List<Ability> ability = new List<Ability>(); 
    public static List<RelicSO> relic = new List<RelicSO>();

    public static Dictionary<string, int> Rewards = new Dictionary<string, int>();
    public List<Image> icons = new List<Image>();
    public List<Image> shadows = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> rewardName = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> rewardDescription = new List<TextMeshProUGUI>();
    [SerializeField] private Sprite aIcon;

    public List<TextMeshProUGUI> RewardName { get => rewardName; set => rewardName = value; }
    public List<TextMeshProUGUI> RewardDescription { get => rewardDescription; set => rewardDescription = value; }

    private void OnEnable()
    {
        SetReward();
    }
    public void ClearLists() {
    
    }
    public void SetReward()
    {
        itm.Clear();
        ability.Clear();
        relic.Clear();

        if (Rewards.Count == 0)
            Rewards = null;

        if (Rewards == null)
        {
            Rewards = new Dictionary<string, int>();

            for (int i = 0; i < 3; i++)
            {
                int rnd = Random.Range(0, 100);

                if (rnd < 75)
                {
                    int iRnd = Random.Range(0, InventoryController.Instance.gameItems.Count);
                   
                    if ((int)InventoryController.Instance.gameItems[iRnd].floorAvailable == PreBattleSelectionController.Instance.GameDetails.Floor)
                    {
                        if (itm.Contains(InventoryController.Instance.gameItems[iRnd]))
                        {
                            i--;
                            continue;
                        }
                        else
                        {
                            itm.Add(InventoryController.Instance.gameItems[iRnd]);
                            icons[i].sprite = itm[itm.Count - 1].itemIcon;
                            shadows[i].sprite = itm[itm.Count - 1].itemIcon;
                            RewardName[i].text = itm[itm.Count - 1].itemName;
                            RewardDescription[i].text = itm[itm.Count - 1].bio;
                            Rewards.Add("itm" + i, InventoryController.Instance.ReturnItem(itm[itm.Count - 1]));
                        }
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                   
                }
                else if (rnd >= 75 && rnd < 95)
                {
                    GameObject go = Resources.Load("AbilityTable") as GameObject;
                    AbilityTable at = go.GetComponent<AbilityTable>();
                    int aRnd = Random.Range(0, at.Abilities.Count);

                    if ((int)at.Abilities[aRnd].floorAvailable == PreBattleSelectionController.Instance.GameDetails.Floor)
                    {
                        if (ability.Contains(at.Abilities[aRnd]))
                        {
                            i--;
                            continue;
                        }
                        else
                        {
                            ability.Add(at.Abilities[aRnd]);
                            icons[i].sprite = aIcon;
                            shadows[i].sprite = aIcon;
                            RewardName[i].text = ability[ability.Count - 1].abilityName;
                            RewardDescription[i].text = ability[ability.Count - 1].abilityBio;
                            Rewards.Add("ability" + i, InventoryController.Instance.ReturnAbility(ability[ability.Count - 1]));
                        }
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                   
                }
                else if (rnd >= 95 && rnd < 100)
                {
                    int rRnd = Random.Range(0, InventoryController.Instance.relics.Count);

                    if ((int)InventoryController.Instance.relics[rRnd].floorAvailable == PreBattleSelectionController.Instance.GameDetails.Floor)
                    {
                        if (InventoryController.Instance.ownedRelics.Count == InventoryController.Instance.relics.Count)
                        {
                            Debug.Log("already have all Relics");
                            i--;
                            continue;
                        }


                        if (relic.Contains(InventoryController.Instance.relics[rRnd]))
                        {
                            i--;
                            continue;
                        }
                        else
                        {
                            relic.Add(InventoryController.Instance.relics[rRnd]);
                            if (InventoryController.Instance.ownedRelics.ContainsKey(InventoryController.Instance.ReturnRelic(relic[relic.Count - 1])) && InventoryController.Instance.ownedRelics[InventoryController.Instance.ReturnRelic(relic[relic.Count - 1])] == true)
                            {
                                i--;
                                continue;
                            }
                            else
                            {
                                icons[i].sprite = relic[relic.Count - 1].icon;
                                shadows[i].sprite = relic[relic.Count - 1].icon;
                                RewardName[i].text = relic[relic.Count - 1].relicName;
                                RewardDescription[i].text = relic[relic.Count - 1].relicDescription;
                                Rewards.Add("relic" + i, InventoryController.Instance.ReturnRelic(relic[relic.Count - 1]));
                            }
                        }
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                  
                }
            }
        }
        else if (Rewards != null) {
            Debug.Log("rewards != null");
            int index = 0;
            foreach (KeyValuePair<string, int> entry in Rewards) {
                if (index == 3)
                    return;
                if (entry.Key.Contains("itm")) {
                    icons[index].sprite = InventoryController.Instance.ReturnItem(entry.Value).itemIcon;
                    shadows[index].sprite = InventoryController.Instance.ReturnItem(entry.Value).itemIcon;
                    RewardName[index].text = InventoryController.Instance.ReturnItem(entry.Value).itemName;
                    RewardDescription[index].text = InventoryController.Instance.ReturnItem(entry.Value).bio;
                    itm.Add(InventoryController.Instance.ReturnItem(entry.Value));
                    index++;
                }
                else if (entry.Key.Contains("ability"))
                {
                    icons[index].sprite = aIcon;
                    shadows[index].sprite = aIcon;
                    RewardName[index].text = InventoryController.Instance.ReturnAbility(entry.Value).abilityName;
                    RewardDescription[index].text = InventoryController.Instance.ReturnAbility(entry.Value).abilityBio;
                    ability.Add(InventoryController.Instance.ReturnAbility(entry.Value));
                    index++;
                }
                else if (entry.Key.Contains("relic"))
                {
                    icons[index].sprite = InventoryController.Instance.ReturnRelic(entry.Value).icon;
                    shadows[index].sprite = InventoryController.Instance.ReturnRelic(entry.Value).icon;
                    RewardName[index].text = InventoryController.Instance.ReturnRelic(entry.Value).relicName;
                    RewardDescription[index].text = InventoryController.Instance.ReturnRelic(entry.Value).relicDescription;
                    relic.Add(InventoryController.Instance.ReturnRelic(entry.Value));
                    index++;
                }
            }
        }
        SaveLoadManager.Save();
    }
}

