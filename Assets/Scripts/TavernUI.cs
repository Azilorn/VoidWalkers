using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TavernUI : MonoBehaviour
{
    [SerializeField] private GameObject experienceGameObject;
    public GameObject creatureEvolutionScreen;
    public static bool isReviveUsed;

    public GameObject ExperienceGameObject { get => experienceGameObject; set => experienceGameObject = value; }

    public void MoveToNextFloor()
    {
        PreBattleSelectionController.Instance.SetFloor(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
    }
    public void Rest()
    {
        StartCoroutine(RestCoroutine());
    }
    public IEnumerator RestCoroutine()
    {
        CoreUI.Instance.PlayerOptions.PartyOptions.BottomBar.SetActive(false);
        yield return StartCoroutine(CoreUI.Instance.OpenAndSetPartyCoroutine());
        yield return new WaitForSeconds(1f);
        List<int> hpValues = new List<int>();
        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party.Length; i++)
        {
            if (BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP > 0)
            {
                int val = Mathf.RoundToInt(BattleController.Instance.MasterPlayerParty.party[i].creatureStats.MaxHP * 0.3f);
                BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP += val;
                BattleController.Instance.MasterPlayerParty.party[i].ClampHP();
                hpValues.Add(BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP);
            }
            else {
                hpValues.Add(BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP);
            }
        }
        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party.Length; i++)
        {
            StartCoroutine(CoreUI.Instance.PlayerOptions.PartyOptions.PartyCreatureUIs[i].UpdateHPSlider(hpValues[i]));
        }
        yield return new WaitForSeconds(1.5f);
        CoreUI.UnlockUI();
        CoreUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwards(true);
        CoreUI.Instance.PlayerOptions.PartyOptions.BottomBar.SetActive(true);
        gameObject.SetActive(false);
        MoveToNextFloor();
    }
    public void Gamble()
    {
        StartCoroutine(GambleCoroutine());
    }
    public IEnumerator GambleCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
    public void Revive()
    {
        StartCoroutine(ReviveCoroutine());
    }
    public IEnumerator ReviveCoroutine()
    {
        isReviveUsed = false;
        CoreUI.Instance.CurrentMenuStatus = MenuStatus.WorldTavernRevive;
        yield return StartCoroutine(CoreUI.Instance.OpenPartyOptions());
        yield return new WaitForSeconds(1f);
        while (!isReviveUsed) {
            yield return new WaitForEndOfFrame();
        }
        yield return StartCoroutine(CoreUI.Instance.ClosePartyOptions());
        MoveToNextFloor();
        gameObject.SetActive(false);
    }
    public void Train() {
        StartCoroutine(TrainCoroutine());
    }
    public IEnumerator TrainCoroutine() {

        CoreUI.Instance.RewardsScreen.XpFinished = false;
        experienceGameObject.SetActive(true);

        CanvasGroup canvasGroup = experienceGameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.55f);
        experienceGameObject.transform.DOScale(1, 0.55f);
        GameObject bg = new GameObject();
        bg.transform.parent = experienceGameObject.transform;
        bg.transform.SetSiblingIndex(0);
        RectTransform rect = bg.AddComponent<RectTransform>();
        Image img =  bg.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.8f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one;

        experienceGameObject.GetComponent<RewardScreenXPMenu>().SetUI(
           (PreBattleSelectionController.Instance.GameDetails.Floor *
            PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor) *
            10);

        while(!CoreUI.Instance.RewardsScreen.XpFinished)
            yield return null;

        yield return StartCoroutine(creatureEvolutionScreen.GetComponent<CreatureEvolutionUI>().EvolveCreatureCoroutine(BattleController.Instance.MasterPlayerParty));
        MoveToNextFloor();
        canvasGroup.DOFade(0, 0.1f);
        experienceGameObject.SetActive(false);
        Destroy(bg);
        gameObject.SetActive(false);
    }

}