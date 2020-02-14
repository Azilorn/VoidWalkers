using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountXpUpdateScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelCount;
    [SerializeField] private TextMeshProUGUI xpCount;
    [SerializeField] private Image xpBar;
    [SerializeField] private GameObject continueButton;

    public TextMeshProUGUI LevelCount { get => levelCount; set => levelCount = value; }
    public TextMeshProUGUI XpCount { get => xpCount; set => xpCount = value; }
    public Image XpBar { get => xpBar; set => xpBar = value; }
    public GameObject ContinueButton { get => continueButton; set => continueButton = value; }

    public void OnEnable()
    {
        ContinueButton.SetActive(false);
        StartCoroutine(AddExperience());
    }

    public IEnumerator AddExperience()
    {

        int addExperience = CoreGameInformation.currentRunDetails.FinalRunScore;
        LevelCount.text = CoreGameInformation.currentLVL.ToString();
        int requiredXp = (CoreGameInformation.currentLVL + 1) * 1000;
        xpCount.text = (requiredXp - CoreGameInformation.currentXPEarned).ToString();

        int startingXP = CoreGameInformation.currentXPEarned;

        yield return new WaitForSeconds(1f);
        int endXp = CoreGameInformation.currentXPEarned + addExperience;
        while (startingXP <= endXp)
        {
            if (startingXP >= requiredXp)
            {
                CoreGameInformation.currentLVL++;
                CoreGameInformation.totalLevelUps++;
                requiredXp = (CoreGameInformation.currentLVL + 1) * 1000;
                LevelCount.text = CoreGameInformation.currentLVL.ToString();
                xpCount.text = (requiredXp - startingXP).ToString();
                xpBar.fillAmount = 0;
            }
            else
            {
                float x, y = 0;
                x = startingXP - (CoreGameInformation.currentLVL * 1000);
                y = requiredXp - (CoreGameInformation.currentLVL * 1000);
                xpCount.text = (requiredXp - startingXP).ToString();
                xpBar.fillAmount = (x / y);
                startingXP = startingXP + 10;
            }

            yield return null;
        }

        CoreGameInformation.currentXPEarned = endXp;
        yield return new WaitForEndOfFrame();
        ContinueButton.SetActive(true);
    }
}
