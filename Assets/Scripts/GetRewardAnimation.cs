using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType {Item, Creature, Ability, Relic }
public class GetRewardAnimation : MonoBehaviour
{
    public static List<RewardType> rewardType = new List<RewardType>();
    public static List<int> rewardInt = new List<int>();
    [SerializeField] private List<Animator> anim = new List<Animator>();
    [SerializeField] private Animator rewardAnim;

    public void GetItem()
    {
        rewardType.Clear();
        rewardInt.Clear();
        foreach (KeyValuePair<string,int> entry in WorldRewardMenuUI.Rewards)
        {
            if (entry.Key.Contains("itm")) {
                rewardType.Add(RewardType.Item);
                rewardInt.Add(entry.Value);
            }
           else if (entry.Key.Contains("ability")) {

                rewardType.Add(RewardType.Ability);
                rewardInt.Add(entry.Value);
            }
           else if (entry.Key.Contains("relic")) {

                rewardType.Add(RewardType.Relic);
                rewardInt.Add(entry.Value);
            }
        }
        for (int i = 0; i < rewardType.Count; i++)
        {
            switch (rewardType[i])
            {
                
                case RewardType.Item:
                    anim[i].SetTrigger("recieveitem");
                        InventoryController.Instance.AddItem(InventoryController.Instance.gameItems[rewardInt[i]]);
                    break;
                case RewardType.Creature:
                    anim[i].SetTrigger("recievecreature");
                    break;
                case RewardType.Ability:
                    anim[i].SetTrigger("recieveitem");
                        InventoryController.Instance.AddAbility(InventoryController.Instance.abilities[rewardInt[i]]);
                    break;
                case RewardType.Relic:
                        InventoryController.Instance.AddRelic(InventoryController.Instance.relics[rewardInt[i]]);
                    anim[i].SetTrigger("recieverelic");
                    break;
            }
        }
        rewardAnim.SetTrigger("closemenu");
        WorldRewardMenuUI.itm.Clear();
        WorldRewardMenuUI.ability.Clear();
        WorldRewardMenuUI.relic.Clear();
        PreBattleSelectionController.Instance.SetPostFloorOptionDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
    }
}
