using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreBattleSelectionUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private TextMeshProUGUI goldText;

    public TextMeshProUGUI FloorText { get => floorText; set => floorText = value; }
    public TextMeshProUGUI GoldText { get => goldText; set => goldText = value; }

    public List<GameObject> selectionUI = new List<GameObject>();
    public GameObject tavernSelctionUI;
    public GameObject rewardSelctionUI;
    public GameObject shopSelctionUI;
    public GameObject bossSelctionUI;

    public OptionSelectedPreviewUI previewUI;
    public HorizontalSwipe swipe;

    public void SetFloorText(string s) {

        floorText.text = s;
    }
    public void SetGoldText(string s) {
        GoldText.text = s;
    }

    public void ReturnToMainMenu() {
        SceneController.Instance.LoadScene(0);
    }
    public void SetAllUIInactive() {

        for (int i = 0; i < selectionUI.Count; i++)
        {
            selectionUI[i].SetActive(false);
        }
        for (int i = 0; i < previewUI.options.Count; i++)
        {
            previewUI.options[i].gameObject.SetActive(false);
        }
        tavernSelctionUI.SetActive(false);
        rewardSelctionUI.SetActive(false);
        shopSelctionUI.SetActive(false);
        bossSelctionUI.SetActive(false);
    }
    public void SetOptionPreviewSprites(int i, int selectionInt)
    {
        previewUI.options[i].gameObject.SetActive(true);
        if (selectionInt == 2001)
        {
            previewUI.options[i].sprite = previewUI.optionIcons[1];
        }
        else if (selectionInt == 2002)
        {
            previewUI.options[i].sprite = previewUI.optionIcons[2];
        }
        else if (selectionInt == 2003)
        {
            previewUI.options[i].sprite = previewUI.optionIcons[3];
        }
        else if (selectionInt == 2004)
        {
            previewUI.options[i].sprite = previewUI.optionIcons[4];
        }
        else {
            previewUI.options[i].sprite = previewUI.optionIcons[0];
        }
    }
}
