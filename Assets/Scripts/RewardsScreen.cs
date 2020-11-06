using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RewardsScreen : MonoBehaviour
{
    [SerializeField] private GameObject victoryGameObject;
    [SerializeField] private GameObject experienceGameObject;
    [SerializeField] private GameObject rewardGameObject;
    [SerializeField] private GameObject creatureEvolutionScreen;
    [SerializeField] private GameObject addNewCreatureToParty;
    private bool xpFinished;
    private bool rewardSelected;

    [SerializeField] List<CreatureSO> floorOneCreatures = new List<CreatureSO>();
    [SerializeField] List<CreatureSO> floorTwoCreatures = new List<CreatureSO>();
    [SerializeField] List<CreatureSO> floorThreeCreatures = new List<CreatureSO>();

    public GameObject VictoryGameObject { get => victoryGameObject; set => victoryGameObject = value; }
    public GameObject ExperienceGameObject { get => experienceGameObject; set => experienceGameObject = value; }
    public bool XpFinished { get => xpFinished; set => xpFinished = value; }
    public bool RewardSelected { get => rewardSelected; set => rewardSelected = value; }
    public GameObject RewardGameObject { get => rewardGameObject; set => rewardGameObject = value; }
    public GameObject AddNewCreatureToParty { get => addNewCreatureToParty; set => addNewCreatureToParty = value; }

    public IEnumerator StartVictoryScreen() {

        while (RelicUIIcon.Instance.gameObject.activeInHierarchy)
            yield return null;
        victoryGameObject.transform.localScale = Vector3.zero;
        MenuTransitionsController.Instance.StartTransition(0, false);
        yield return new WaitForSecondsRealtime(0.3f);
        CoreUI.Instance.RewardsScreen.transform.localScale = Vector3.one;
        CoreUI.Instance.RewardsScreen.gameObject.SetActive(true);
        StartCoroutine(CoreUI.OpenMenu(victoryGameObject, 0, 0.45f));

        bool clicked = false;

        while (clicked == false)
        {

            if (Input.GetMouseButtonDown(0))
            {
                clicked = true;
                StartCoroutine(CoreUI.CloseMenu(victoryGameObject, 0, 0.45f));
                StartCoroutine(StartExperienceScreen());
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator StartExperienceScreen() {

        xpFinished = false;
        experienceGameObject.SetActive(true);
        experienceGameObject.transform.localScale = Vector3.zero;
        //TODO Change Xp to Trainer XP;
        experienceGameObject.GetComponent<RewardScreenXPMenu>().SetUI((int)((BattleController.Instance.EnemyParty.ReturnAverageLevelAcrossParty() * 10) * ComeBackXPModifier()));
        MenuTransitionsController.Instance.StartTransition(4, false);
        yield return new WaitForSecondsRealtime(0.3f);
        MenuTransitionsController.Instance.StartTransition(4, true);
        yield return StartCoroutine(CoreUI.OpenMenu(experienceGameObject, 0, 0.25f));
        bool clicked = false;

        while (!clicked)
        {
            if (Input.GetMouseButton(0) && XpFinished)
            {
                clicked = true;
                StartCoroutine(CoreUI.CloseMenu(experienceGameObject, 0, 0.45f));
                yield return StartCoroutine(creatureEvolutionScreen.GetComponent<CreatureEvolutionUI>().EvolveCreatureCoroutine(BattleController.Instance.MasterPlayerParty));
                StartCoroutine(StartRewardsScreen());
            }
            yield return new WaitForFixedUpdate();
        }

    }

    private float ComeBackXPModifier()
    {
        float modifier = 1;
        int averageLevelParty = BattleController.Instance.MasterPlayerParty.ReturnAverageLevelAcrossParty();
        int averageLevelEnemy = BattleController.Instance.EnemyParty.ReturnAverageLevelAcrossParty();

        if (averageLevelEnemy > averageLevelParty)
        {
            modifier = 1f + (0.25f * (averageLevelEnemy - averageLevelParty));
           
        }
        else if (averageLevelParty > averageLevelEnemy)
        {
            modifier = 1f - (0.25f * (averageLevelParty - averageLevelEnemy));
        }
        else {
            modifier = 1;
        }
        Debug.Log("modifier: " + modifier);
        return modifier;
    }

    public IEnumerator StartRewardsScreen()
    {
        rewardSelected = false;
        rewardGameObject.SetActive(true);
        rewardGameObject.transform.localScale = Vector3.zero;
        //TODO Change Xp to Trainer XP;
        rewardGameObject.GetComponent<RewardScreenChoiceMenu>().SetUI();
        MenuTransitionsController.Instance.StartTransition(4, false);
        yield return new WaitForSecondsRealtime(0.3f);
        MenuTransitionsController.Instance.StartTransition(4, true);
        yield return StartCoroutine(CoreUI.OpenMenu(rewardGameObject, 0, 0.25f));        
    }
    public void CloseRewardMenu()
    {
       StartCoroutine(CloseRewardMenuCoroutine());
    }

    private IEnumerator CloseRewardMenuCoroutine()
    {

        if (PreBattleSelectionController.Instance.GameDetails.Floor != 4 && PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor == 10 && !addNewCreatureToParty.gameObject.activeInHierarchy)
        {
            StartCoroutine(StartAddNewCreatureToPartyScreen());
        }
        else
        {
            PreBattleSelectionController.Instance.SetFloor(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
            if (PreBattleSelectionController.Instance.GameDetails.Floor != 4 && PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor != 10)
                MenuTransitionsController.Instance.StartTransition(4, false);
            AudioManager.Instance.PlayMusicWithMultiplePartsFromAudioManager(UIAudio.Instance.WorldfloorBGM[0].AudioList);
            yield return new WaitForSecondsRealtime(0.5f);
            if (PreBattleSelectionController.Instance.GameDetails.Floor != 4 && PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor != 10)
                MenuTransitionsController.Instance.StartTransition(4, true);
            CoreUI.Instance.BattleCanvasTransform.gameObject.SetActive(false);
            if (addNewCreatureToParty.gameObject.activeInHierarchy) {
                addNewCreatureToParty.gameObject.SetActive(false);
            } 
            else CoreUI.Instance.RewardsScreen.rewardGameObject.SetActive(false);
            CoreUI.Instance.RewardsScreen.gameObject.SetActive(false);
            CoreUI.Instance.ToggleMenuBars(true);
        } 
    }
    private IEnumerator StartAddNewCreatureToPartyScreen() {

        var creatureSO = new CreatureSO[3];

        creatureSO = ReturnCreatureSO(creatureSO.Length, PreBattleSelectionController.Instance.GameDetails.Floor);

        addNewCreatureToParty.GetComponent<AddNewCreatureToPartyUI>().SetUI(creatureSO);
        MenuTransitionsController.Instance.StartTransition(4, false);
        yield return new WaitForSecondsRealtime(0.3f);
        MenuTransitionsController.Instance.StartTransition(4, true);
        CoreUI.Instance.RewardsScreen.rewardGameObject.SetActive(false);
        addNewCreatureToParty.SetActive(true);

    }

    private CreatureSO[] ReturnCreatureSO(int length, int floor)
    {
        var creatureSO = new CreatureSO[length];
        for (int i = 0; i < length; i++)
        { 
            CreatureSO cSO = new CreatureSO();

            switch (floor)
            {

                case 1:
                    cSO = floorOneCreatures[UnityEngine.Random.Range(0, floorOneCreatures.Count)];
                    for (int j = 0; j < length; j++)
                    {
                        if (creatureSO[j] == cSO)
                            i--;
                        continue;
                    }
                    creatureSO[i] = cSO;

                    break;
                case 2:
                    cSO = floorTwoCreatures[UnityEngine.Random.Range(0, floorTwoCreatures.Count)];

                    for (int j = 0; j < length; j++)
                    {
                        if (creatureSO[j] == cSO)
                            i--;
                        continue;
                    }
                    creatureSO[i] = cSO;
                    break;
                case 3:
                    cSO = floorThreeCreatures[UnityEngine.Random.Range(0, floorThreeCreatures.Count)];

                    for (int j = 0; j < length; j++)
                    {
                        if (creatureSO[j] == cSO)
                            i--;
                        continue;
                    }
                    creatureSO[i] = cSO;
                    break;
            }
        }
        return creatureSO;
    }
}
