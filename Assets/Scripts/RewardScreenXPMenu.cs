﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreenXPMenu : MonoBehaviour
{
    public RewardScreenXPUI[] rewardScreenXPUIs = new RewardScreenXPUI[6];

    public void SetUI(int xpEarned) {

        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party.Length; i++)
        {
            if (BattleController.Instance.MasterPlayerParty.party[i].creatureSO == null)
            {
                rewardScreenXPUIs[i].gameObject.SetActive(false);
            }
            else {
                if (BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP > 0)
                {
                    rewardScreenXPUIs[i].SetUI(BattleController.Instance.MasterPlayerParty.party[i], xpEarned);
                    rewardScreenXPUIs[i].gameObject.SetActive(true);
                    StartCoroutine(rewardScreenXPUIs[i].UpdateSlider(BattleController.Instance.MasterPlayerParty.party[i], xpEarned));
                }
                else {
                    rewardScreenXPUIs[i].SetUI(BattleController.Instance.MasterPlayerParty.party[i], xpEarned);
                    rewardScreenXPUIs[i].gameObject.SetActive(true);
                }
            }
        }

    }
    public void SetUISingle(int xpEarned, int cIndex) {

        for (int i = 0; i < rewardScreenXPUIs.Length; i++) {
            if (i == cIndex)
                continue;
            else rewardScreenXPUIs[i].gameObject.SetActive(false);
        }
        if (BattleController.Instance.MasterPlayerParty.party[cIndex].creatureSO == null)
        {
            return;
        }
        if (BattleController.Instance.MasterPlayerParty.party[cIndex].creatureStats.HP > 0)
        {
            rewardScreenXPUIs[cIndex].SetUI(BattleController.Instance.MasterPlayerParty.party[cIndex], xpEarned);
            rewardScreenXPUIs[cIndex].gameObject.SetActive(true);
            StartCoroutine(rewardScreenXPUIs[cIndex].UpdateSlider(BattleController.Instance.MasterPlayerParty.party[cIndex], xpEarned));
        }
    }
}
