using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType {Item, Creature, Ability, Relic }
public class GetRewardAnimation : MonoBehaviour
{
    public static RewardType rewardType;
    [SerializeField] private Animator anim;

    public void GetItem()
    {
        switch (rewardType)
        {
            case RewardType.Item:
                anim.SetTrigger("recieveitem");
                InventoryController.Instance.AddItem(WorldRewardMenuUI.itm);
                break;
            case RewardType.Creature:
                anim.SetTrigger("recievecreature");
                break;
            case RewardType.Ability:
                anim.SetTrigger("recieveitem");
                InventoryController.Instance.AddAbility(WorldRewardMenuUI.ability);
                break;
            case RewardType.Relic:
                InventoryController.Instance.AddRelic(WorldRewardMenuUI.relic);
                anim.SetTrigger("recieverelic");
                break;
        }
        WorldRewardMenuUI.itm = null;
        WorldRewardMenuUI.creatureSO = null;
        WorldRewardMenuUI.ability = null;
        WorldRewardMenuUI.relic = null;
        PreBattleSelectionController.Instance.SetPostFloorOptionDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
    }
    public static void SetRewardType(RewardType t) {

        rewardType = t;
    }
}
