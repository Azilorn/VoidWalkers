using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AccountLevelRewardScreen : MonoBehaviour
{
    public List<AccountRewardUI> accountRewards = new List<AccountRewardUI>();
    public GameObject exitButton;

    private void OnEnable()
    {
        exitButton.SetActive(false);
        StartCoroutine(SetUnlocks());
    }

    //Sets Unlocked Creatures and poisitons UI elements on Account level up screen
    public IEnumerator SetUnlocks() {

        for (int i = 0; i < accountRewards.Count; i++)
            accountRewards[i].gameObject.SetActive(false);

        int startingLevel = 0;
        startingLevel = CoreGameInformation.currentLVL;
        startingLevel -= CoreGameInformation.totalLevelUps;

        if (startingLevel == CoreGameInformation.currentLVL)
        {
            exitButton.SetActive(true);
        }
        else
        {
            int currentReward = 0;
            for (int i = startingLevel; i < CoreGameInformation.currentLVL; i++)
            {
                switch (i + 1)
                {

                    case 2:
                        Debug.Log("level 2");
                        CreatureTable.Instance.UnlockedCreature[33] = true;
                        AddToCurrentRewardPool(currentReward);
                        accountRewards[currentReward].SetReward(CreatureTable.Instance.Creatures[33]);
                        accountRewards[currentReward].gameObject.SetActive(true);
                        SetRandomPos(accountRewards[currentReward].rect);
                        currentReward++;
                        yield return new WaitForSeconds(1f);
                        CreatureTable.Instance.UnlockedCreature[38] = true;
                        AddToCurrentRewardPool(currentReward);
                        accountRewards[currentReward].SetReward(CreatureTable.Instance.Creatures[38]);
                        accountRewards[currentReward].gameObject.SetActive(true);
                        SetRandomPos(accountRewards[currentReward].rect);
                        currentReward++;
                        yield return new WaitForSeconds(1f);
                        CreatureTable.Instance.UnlockedCreature[42] = true;
                        AddToCurrentRewardPool(currentReward);
                        accountRewards[currentReward].SetReward(CreatureTable.Instance.Creatures[42]);
                        accountRewards[currentReward].gameObject.SetActive(true);
                        SetRandomPos(accountRewards[currentReward].rect);
                        currentReward++;
                        yield return new WaitForSeconds(1f);
                        break;
                    case 3:
                        Debug.Log("level 3");
                        CreatureTable.Instance.UnlockedCreature[20] = true;
                        AddToCurrentRewardPool(currentReward);
                        accountRewards[currentReward].SetReward(CreatureTable.Instance.Creatures[20]);
                        accountRewards[currentReward].gameObject.SetActive(true);
                        SetRandomPos(accountRewards[currentReward].rect);
                        currentReward++;
                        yield return new WaitForSeconds(1f);
                        CreatureTable.Instance.UnlockedCreature[43] = true;
                        AddToCurrentRewardPool(currentReward);
                        accountRewards[currentReward].SetReward(CreatureTable.Instance.Creatures[43]);
                        accountRewards[currentReward].gameObject.SetActive(true);
                        SetRandomPos(accountRewards[currentReward].rect);
                        currentReward++;
                        yield return new WaitForSeconds(1f);
                        CreatureTable.Instance.UnlockedCreature[44] = true;
                        AddToCurrentRewardPool(currentReward);
                        accountRewards[currentReward].SetReward(CreatureTable.Instance.Creatures[44]);
                        accountRewards[currentReward].gameObject.SetActive(true);
                        SetRandomPos(accountRewards[currentReward].rect);
                        currentReward++;
                        yield return new WaitForSeconds(1f);
                        break;
                    case 4:
                        Debug.Log("level 4");
                        yield return new WaitForSeconds(1f);

                        break;
                    case 5:
                        Debug.Log("level 5");
                        yield return new WaitForSeconds(1f);

                        break;
                    case 6:
                        Debug.Log("level 6");
                        yield return new WaitForSeconds(1f);

                        break;
                    case 7:
                        Debug.Log("level 7");
                        yield return new WaitForSeconds(1f);

                        break;
                    case 8:
                        Debug.Log("level 8");
                        yield return new WaitForSeconds(1f);

                        break;
                    case 9:
                        Debug.Log("level 9");
                        yield return new WaitForSeconds(1f);

                        break;
                    case 10:
                        Debug.Log("level 10");
                        yield return new WaitForSeconds(1f);

                        break;
                    default:

                        break;
                }

            }
        }
       exitButton.SetActive(true);
    }

    private void AddToCurrentRewardPool(int currentReward)
    {
        if (currentReward >= accountRewards.Count)
        {

            GameObject go = Instantiate(accountRewards[0].gameObject, accountRewards[0].transform.parent);
            AccountRewardUI rewardUI = go.GetComponent<AccountRewardUI>();
            accountRewards.Add(rewardUI);
        }
    }

    public void SetRandomPos(RectTransform rect) {

        rect.localPosition = new Vector3(Random.Range(-100, 100), Random.Range(-60, 60), 0);
        rect.eulerAngles = new Vector3(0, 0, Random.Range(0, 5));
    }
}
