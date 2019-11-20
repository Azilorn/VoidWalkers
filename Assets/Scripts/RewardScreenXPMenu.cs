using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreenXPMenu : MonoBehaviour
{
    public RewardScreenXPUI[] rewardScreenXPUIs = new RewardScreenXPUI[6];

    public void SetUI(int xpEarned) {

        for (int i = 0; i < BattleController.Instance.TurnController.PlayerParty.party.Length; i++)
        {
            if (BattleController.Instance.TurnController.PlayerParty.party[i].creatureSO == null)
            {
                rewardScreenXPUIs[i].gameObject.SetActive(false);
            }
            else {
                if (BattleController.Instance.TurnController.PlayerParty.party[i].creatureStats.HP > 0)
                {
                    rewardScreenXPUIs[i].SetUI(BattleController.Instance.TurnController.PlayerParty.party[i], xpEarned);
                    rewardScreenXPUIs[i].gameObject.SetActive(true);
                    StartCoroutine(rewardScreenXPUIs[i].UpdateSlider(BattleController.Instance.TurnController.PlayerParty.party[i], xpEarned));
                }
                else {
                    rewardScreenXPUIs[i].SetUI(BattleController.Instance.TurnController.PlayerParty.party[i], xpEarned);
                    rewardScreenXPUIs[i].gameObject.SetActive(true);
                }
            }
        }

    }
}
