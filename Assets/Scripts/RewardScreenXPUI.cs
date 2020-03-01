using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreenXPUI : MonoBehaviour
{
    public TextMeshProUGUI creatureRemainingXP;
    public TextMeshProUGUI creatureLevel;
    public Slider xpSlider;
    public Image creatureIcon;
    int levelUpCount;

    public void SetUI(PlayerCreatureStats playerCreatureStat, int xpEarned)
    {
        creatureIcon.sprite = playerCreatureStat.creatureSO.creaturePlayerIcon;
        creatureRemainingXP.text = "XP Remaining: " + (XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level + 1] - playerCreatureStat.creatureStats.Xp).ToString();
        creatureLevel.text = "LVL:" + playerCreatureStat.creatureStats.level.ToString();
        xpSlider.minValue = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level];
        xpSlider.maxValue = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level + 1];
        xpSlider.value = playerCreatureStat.creatureStats.Xp;
    }

    public IEnumerator UpdateSlider(PlayerCreatureStats playerCreatureStat, int xpEarned)
    {
        int originalXp = playerCreatureStat.creatureStats.Xp;
        levelUpCount = 0;
        float i = 0;
        int originalValue = originalXp;
        int targetValue = originalXp + xpEarned;
        float duration = 0.5f;

        yield return new WaitForSeconds(0.5f);
        while (i < xpEarned)
        {
          
            if (Input.GetMouseButtonDown(0))
            {
                i = xpEarned;
                playerCreatureStat.creatureStats.Xp = originalXp + xpEarned;
                int originalLevel = XPMatrix.ReturnLevel(originalXp);
                playerCreatureStat.creatureStats.level = XPMatrix.ReturnLevel(playerCreatureStat.creatureStats.Xp);
                levelUpCount = playerCreatureStat.creatureStats.level - originalLevel;

                creatureLevel.text = "LVL:" + playerCreatureStat.creatureStats.level.ToString();
                if (playerCreatureStat.creatureStats.level + 1 > XPMatrix.xpLevelList.Count) {
                    xpSlider.maxValue = playerCreatureStat.creatureStats.level;
                }
                else xpSlider.maxValue = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level + 1];
                xpSlider.minValue = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level];
                xpSlider.value = playerCreatureStat.creatureStats.Xp;
                creatureRemainingXP.text = "XP Remaining: " + (xpSlider.maxValue - playerCreatureStat.creatureStats.Xp).ToString();
            }
            else
            {
                i += 10 * Time.deltaTime;
                xpSlider.value++;
                creatureRemainingXP.text = "XP Remaining: " + (XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level + 1] - xpSlider.value).ToString();

                if (xpSlider.value == xpSlider.maxValue)
                {
                    playerCreatureStat.creatureStats.level++;
                    levelUpCount++;
                    creatureLevel.text = "LVL:" + playerCreatureStat.creatureStats.level.ToString();
                    if (playerCreatureStat.creatureStats.level + 1 > XPMatrix.xpLevelList.Count)
                    {
                        xpSlider.maxValue = playerCreatureStat.creatureStats.level;
                    }
                    else xpSlider.maxValue = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level + 1];
                    xpSlider.minValue = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level];
                    creatureRemainingXP.text = "XP Remaining: " + (xpSlider.maxValue - playerCreatureStat.creatureStats.Xp).ToString();
                    playerCreatureStat.creatureStats.Xp = XPMatrix.xpLevelList[playerCreatureStat.creatureStats.level];
                }
            }
            yield return new WaitForFixedUpdate();
        }
        for (int r = 0; r < levelUpCount; r++)
        {
            playerCreatureStat.AddLevel();
        }
        yield return new WaitForSeconds(0.2f);
        playerCreatureStat.creatureStats.Xp = originalXp + xpEarned;
        CoreUI.Instance.RewardsScreen.XpFinished = true;
    }
}
