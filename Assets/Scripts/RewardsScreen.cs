using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardsScreen : MonoBehaviour
{
    [SerializeField] private GameObject victoryGameObject;
    [SerializeField] private GameObject experienceGameObject;
    [SerializeField] private GameObject rewardGameObject;
    [SerializeField] private bool xpFinished;
    [SerializeField] private bool rewardSelected;

    public GameObject VictoryGameObject { get => victoryGameObject; set => victoryGameObject = value; }
    public GameObject ExperienceGameObject { get => experienceGameObject; set => experienceGameObject = value; }
    public bool XpFinished { get => xpFinished; set => xpFinished = value; }
    public bool RewardSelected { get => rewardSelected; set => rewardSelected = value; }
    public GameObject RewardGameObject { get => rewardGameObject; set => rewardGameObject = value; }

    public IEnumerator StartVictoryScreen() {

        while (RelicUIIcon.Instance.gameObject.activeInHierarchy)
            yield return null;
        victoryGameObject.transform.localScale = Vector3.zero;
        MenuTransitionsController.Instance.StartTransition(0, false);
        yield return new WaitForSecondsRealtime(0.3f);
        BattleUI.Instance.RewardsScreen.transform.localScale = Vector3.one;
        BattleUI.Instance.RewardsScreen.gameObject.SetActive(true);
        StartCoroutine(BattleUI.OpenMenu(victoryGameObject, 0, 0.45f));

        bool clicked = false;

        while (clicked == false)
        {

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(BattleUI.CloseMenu(victoryGameObject, 0, 0.45f));
                StartCoroutine(StartExperienceScreen());
                clicked = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator StartExperienceScreen() {

        xpFinished = false;
        experienceGameObject.SetActive(true);
        experienceGameObject.transform.localScale = Vector3.zero;
        //TODO Change Xp to Trainer XP;
        experienceGameObject.GetComponent<RewardScreenXPMenu>().SetUI(20);
        MenuTransitionsController.Instance.StartTransition(0, false);
        yield return new WaitForSecondsRealtime(0.3f);
        yield return StartCoroutine(BattleUI.OpenMenu(experienceGameObject, 0, 0.25f));
        bool clicked = false;

        while (!clicked)
        {
            if (Input.GetMouseButton(0) && XpFinished)
            {
                StartCoroutine(BattleUI.CloseMenu(experienceGameObject, 0, 0.45f));
                StartCoroutine(StartRewardsScreen());
                clicked = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator StartRewardsScreen()
    {
        rewardSelected = false;
        rewardGameObject.SetActive(true);
        rewardGameObject.transform.localScale = Vector3.zero;
        //TODO Change Xp to Trainer XP;
        rewardGameObject.GetComponent<RewardScreenChoiceMenu>().SetUI();
        MenuTransitionsController.Instance.StartTransition(0, false);
        yield return new WaitForSecondsRealtime(0.3f);
        yield return StartCoroutine(BattleUI.OpenMenu(rewardGameObject, 0, 0.25f));        
    }
    public void CloseRewardMenu()
    {
       StartCoroutine(CloseRewardMenuCoroutine());
    }

    private IEnumerator CloseRewardMenuCoroutine()
    {
        PreBattleSelectionController.Instance.SetPostFloorOptionDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
        MenuTransitionsController.Instance.StartTransition(0, false);
        AudioManager.Instance.PlayMusicWithMultiplePartsFromAudioManager(UIAudio.Instance.WorldfloorBGM[0].AudioList);
        yield return new WaitForSecondsRealtime(0.3f);
        BattleUI.Instance.BattleCanvasTransform.gameObject.SetActive(false);
        BattleUI.Instance.RewardsScreen.rewardGameObject.SetActive(false);
        BattleUI.Instance.RewardsScreen.gameObject.SetActive(false);
        WorldMenuUI.Instance.ToggleMenuBars(true);
    }
}
