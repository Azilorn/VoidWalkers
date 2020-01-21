using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class PlayerStatsUI : MonoBehaviour
{

    [SerializeField] private CreatureDetailsUI detailsUI;
    [SerializeField] private TextMeshProUGUI creatureName;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private PlayerCreatureStats stats;
    [SerializeField] private RemainingPartyUI remainingPartyUI;
    [SerializeField] private StatusEffectUI statusEffectUI;

    [SerializeField] private PlayerParty party;
    private float holdTimer;
    [SerializeField] private float holdDurationRequired = 0.35f;
    private bool buttonClicked;
    private bool buttonHeld;

    public TextMeshProUGUI CreatureName { get => creatureName; set => creatureName = value; }
    public TextMeshProUGUI HpText { get => hpText; set => hpText = value; }
    public Slider HpSlider { get => hpSlider; set => hpSlider = value; }
    public RemainingPartyUI RemainingPartyUI { get => remainingPartyUI; set => remainingPartyUI = value; }
    public StatusEffectUI StatusEffectUI { get => statusEffectUI; set => statusEffectUI = value; }
    public TextMeshProUGUI LvlText { get => lvlText; set => lvlText = value; }

    public void Update()
    {
        if (detailsUI.gameObject.activeInHierarchy)
            return;
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                detailsUI.SetMenu(stats);
                detailsUI.gameObject.SetActive(true);
                BattleUI.DoFadeIn(detailsUI.gameObject, 0.25f);
                StartCoroutine(BattleUI.OpenMenu(detailsUI.MainBody.gameObject, 0f, 0.25f));
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                buttonClicked = false;
                if (StatusEffectUI.gameObject.activeInHierarchy)
                    StatusEffectUI.gameObject.SetActive(false);
                else
                {
                    StatusEffectUI.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnButtonClickedDown()
    {
        if (detailsUI.gameObject.activeInHierarchy)
            return;
        holdTimer = 0;
        buttonHeld = true;
        buttonClicked = true;
    }
    public void OnButtonClickedUp()
    {
        if (detailsUI.gameObject.activeInHierarchy)
            return;
        if (!buttonHeld)
            return;
        if (!buttonClicked)
            return;
        if (holdTimer < holdDurationRequired)
        {
            buttonClicked = true;
            buttonHeld = false;     
         
        }
        else if (holdTimer > holdDurationRequired)
        {
            buttonClicked = false;
            buttonHeld = false;
            holdTimer = 0;
        }
    }

    public void SetPlayerStatsMatchStart(PlayerCreatureStats player, PlayerParty p) {

        if (stats != player)
            stats = player;
        party = p;
        creatureName.text = stats.creatureSO.creatureName;
        hpText.text = stats.creatureStats.HP + "/" + stats.creatureStats.MaxHP;
        LvlText.text = "LVL: " + stats.creatureStats.level;
        SetMaxValue(stats.creatureStats.MaxHP);
        UpdateHPSliderStill(stats.creatureStats.HP);
        SetReaminingUI(p);
        SetStatusEffects(player);
        StatusEffectUI.gameObject.SetActive(true);
    }

    private void SetStatusEffects(PlayerCreatureStats player)
    {
        for (int i = 0; i < statusEffectUI.statusEffects.Count; i++)
        {
            statusEffectUI.statusEffects[i].SetActive(false);
        }
        for (int i = 0; i < player.ailments.Count; i++)
        {
            if (player.ailments[i] != null)
                statusEffectUI.ReturnElement(player.ailments[i]).SetActive(true);
        }
        for (int i = player.ailments.Count - 1; i > 0; i--)
        {
            if (player.ailments[i] == null)
                player.ailments.RemoveAt(i);
        }
    }

    public void SetPlayerStats(PlayerCreatureStats player, PlayerParty p) {

        if(stats != player)
            stats = player;
        party = p;
        if(BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            StartCoroutine(UpdateHPSlider(stats.creatureStats.HP, p));
        creatureName.text = stats.creatureSO.creatureName;
        SetMaxValue(stats.creatureStats.MaxHP);
        SetStatusEffects(player);
    } 

    public void SetMaxValue(int value) {
        HpSlider.maxValue = value;
    }
    public IEnumerator UpdateHPSlider(int value, PlayerParty p) {

        int startValue = (int)HpSlider.value;
        HpSlider.DOValue(value, 1f);
        UpdateSliderEvent();
        yield return new WaitForSeconds(1f);
        SetReaminingUI(p);
    }
    public void UpdateSliderEvent() {
        StartCoroutine(UpdateText());
    }
    public IEnumerator UpdateText() {

        float duration = 1f;
        float timer = 0f;
        while (timer < duration) {
            timer += Time.deltaTime;
            hpText.text = ((int)hpSlider.value).ToString() + "/" + stats.creatureStats.MaxHP;
            yield return null;
        }
        hpText.text = hpSlider.value.ToString() + "/" + stats.creatureStats.MaxHP;
    }
    public void UpdateHPSliderStill(int value) {
        HpSlider.value = value;
    }
    public void SetReaminingUI(PlayerParty p)
    {
        remainingPartyUI.SetPartyUI(p);
    }


}
