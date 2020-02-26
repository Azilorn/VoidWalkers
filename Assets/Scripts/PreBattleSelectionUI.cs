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

    public List<GameObject> battleSelectionUI = new List<GameObject>();
    public GameObject tavernSelctionUI;
    public GameObject rewardSelctionUI;
    public GameObject shopSelctionUI;
    public GameObject bossSelctionUI;
    public GameObject eventSelectionUI;

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

        for (int i = 0; i < battleSelectionUI.Count; i++)
        {
            battleSelectionUI[i].SetActive(false);
        }
        for (int i = 0; i < previewUI.options.Count; i++)
        {
            previewUI.options[i].gameObject.SetActive(false);
        }
        tavernSelctionUI.SetActive(false);
        rewardSelctionUI.SetActive(false);
        shopSelctionUI.SetActive(false);
        bossSelctionUI.SetActive(false);
        eventSelectionUI.SetActive(false);
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
        else if (selectionInt == 2005)
        {
            previewUI.options[i].sprite = previewUI.optionIcons[5];
        }
        else {

            switch (PreBattleSelectionController.Instance.enemies[selectionInt].partyType)
            {
                case PartyType.Battle:
                    previewUI.options[i].sprite = previewUI.optionIcons[0];
                    break;
                case PartyType.Elite:
                    previewUI.options[i].sprite = previewUI.optionIcons[6];
                    break;
                case PartyType.Boss:
                    previewUI.options[i].sprite = previewUI.optionIcons[4];
                    break;
                case PartyType.Player:
                    break;
            }

          
        }
    }
}
