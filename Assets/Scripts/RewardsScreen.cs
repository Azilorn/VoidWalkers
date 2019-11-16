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

        victoryGameObject.transform.localScale = Vector3.zero;
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
        yield return StartCoroutine(BattleUI.OpenMenu(rewardGameObject, 0, 0.25f));        
    }
    public void CloseRewardMenu() {

        PreBattleSelectionController.Instance.SetPostBattleUIDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
        BattleUI.Instance.BattleTransitionManager.gameObject.SetActive(true);
        BattleUI.Instance.BattleTransitionManager.transitions[1].gameObject.SetActive(true);
        BattleUI.Instance.CloseMenuViaBattleUI(BattleUI.Instance.BattleCanvasTransform.gameObject, 0, 0.35f);
        BattleUI.Instance.CloseMenuViaBattleUI(BattleUI.Instance.RewardsScreen.rewardGameObject, 0, 0.35f);
        BattleUI.Instance.CloseMenuViaBattleUI(BattleUI.Instance.RewardsScreen.gameObject, 0, 0.35f);

    }
}
