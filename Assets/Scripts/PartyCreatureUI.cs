﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PartyCreatureUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public CreatureDetailsUI detailsUI;
    [SerializeField] private GameObject emptyParty;
    [SerializeField] private GameObject creatureDetails;
    [SerializeField] private GameObject selectedBorder;
    [SerializeField] private GameObject swapButton;
    [SerializeField] private GameObject cannotLearnAbility;
    [SerializeField] private Image creatureIcon;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI creatureName;
    [SerializeField] private TextMeshProUGUI remainingHP;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private StatusEffectUI statusEffectUI;
    private PlayerCreatureStats creature;
    public int creatureIndex;

    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDurationRequired;
    [SerializeField] private bool buttonClicked;
    [SerializeField] private bool buttonHeld;
    private static bool isSwapingWithButton = false;
    public static Vector3 currentPos;
    public static Vector3 startPos;
    public static int startingIndex;

    public Slider HpSlider { get => hpSlider; set => hpSlider = value; }
    public TextMeshProUGUI CreatureName { get => creatureName; set => creatureName = value; }
    public TextMeshProUGUI RemainingHP { get => remainingHP; set => remainingHP = value; }
    public Image CreatureIcon { get => creatureIcon; set => creatureIcon = value; }
    public GameObject CreatureDetails { get => creatureDetails; set => creatureDetails = value; }
    public GameObject EmptyParty { get => emptyParty; set => emptyParty = value; }
    public StatusEffectUI StatusEffectUI { get => statusEffectUI; set => statusEffectUI = value; }
    public GameObject SelectedBorder { get => selectedBorder; set => selectedBorder = value; }

    public static GameObject dragCopy;

    public void OnEnable()
    {
        isSwapingWithButton = false;
        creatureIndex = transform.GetSiblingIndex();
        selectedBorder.SetActive(false);
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility) {

        }
        else cannotLearnAbility.SetActive(false);
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            selectedBorder.gameObject.SetActive(false);
            return;
        }

        if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
        {
            if (transform.GetSiblingIndex() == BattleController.Instance.MasterPlayerParty.selectedCreature)
                selectedBorder.gameObject.SetActive(true);
        }

    }
    public void Update()
    {
        if (detailsUI.gameObject.activeInHierarchy)
            return;
        if (buttonHeld)
        {
            if (dragCopy != null)
                return;
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {

                isSwapingWithButton = false;
                selectedBorder.SetActive(false);
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (CoreUI.Locked == true)
                return;
            if (buttonClicked)
            {
                isSwapingWithButton = false;
                if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature) {
                    selectedBorder.SetActive(true);
                }
                else selectedBorder.SetActive(false);
                if (CoreUI.Instance.DialogueBox.gameObject.activeInHierarchy)
                    return;
                if (CoreUI.Locked)
                    return;

                if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                {
                    //Add Audio                
                    ItemController.Instance.UseItem(creatureIndex);
                }
                else {
                    if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive || CoreUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive)
                    {
                        //Add Audio
                        ItemController.Instance.CurrentlySelectedItem = 12;
                        ItemController.Instance.UseItem(creatureIndex);
                    }
                    else if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility)
                    {
                        bool canAdd = true;
                        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureAbilities.Length; i++)
                        {
                            if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureAbilities[i] == null)
                                continue;
                            else if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureAbilities[i].ability == AddReplaceAbilityOptions.Instance.currentSelectedAbility)
                            {
                                StartCoroutine(CoreUI.Instance.TypeDialogue(AddReplaceAbilityOptions.Instance.currentSelectedAbility.abilityName + " Already Learnt", CoreUI.Instance.DialogueBox.Dialogue, 1f, true, true));
                                buttonClicked = false;
                                canAdd = false;
                                break;
                            }
                        }
                       
                        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.illegalAbilities.Count > 0 && BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.illegalAbilities.Contains(AddReplaceAbilityOptions.Instance.currentSelectedAbility))
                        {
                            StartCoroutine(CoreUI.Instance.TypeDialogue("Cannot Learn " + AddReplaceAbilityOptions.Instance.currentSelectedAbility.abilityName, CoreUI.Instance.DialogueBox.Dialogue, 1f, true, true));
                            buttonClicked = false;
                            canAdd = false;
                        }

                        if (canAdd == false)
                            return;
                        //Add Audio
                        AddReplaceAbilityOptions.Instance.SetAddReplaceAbilityMenu(creatureIndex);
                        CoreUI.Instance.CloseParty();
                    }
                    else {
                        //Add Audio
                        ItemController.Instance.UseItem(creatureIndex);
                    }
                }
                buttonClicked = false;
            }
        }
    }

    public void ShowCreatureInformation()
    {
        detailsUI.SetMenu(creature);
        detailsUI.gameObject.SetActive(true);
        CoreUI.DoFadeIn(detailsUI.gameObject, 0.25f);
        CoreUI.DoFadeIn(detailsUI.MainBody.gameObject, 0.25f);
        CoreUI.DoFadeIn(detailsUI.AbilitiesBody.gameObject, 0.25f);
        StartCoroutine(CoreUI.OpenMenu(detailsUI.MainBody.gameObject, 0f, 0.25f));
        StartCoroutine(CoreUI.OpenMenu(detailsUI.AbilitiesBody.gameObject, 0f, 0.25f));
    }

    public void SetPartyCreatureUI(bool empty, PlayerCreatureStats stats)
    {
        cannotLearnAbility.gameObject.SetActive(false);
        if (empty)
        {
            EmptyParty.SetActive(true);
            CreatureDetails.SetActive(false);
        }
        else {
            if (AddReplaceAbilityOptions.Instance.currentSelectedAbility != null)
            {
                bool containsIllegalAbility = BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.illegalAbilities.Contains(AddReplaceAbilityOptions.Instance.currentSelectedAbility);
                if (containsIllegalAbility)
                {
                    cannotLearnAbility.gameObject.SetActive(true);
                    cannotLearnAbility.GetComponentInChildren<TextMeshProUGUI>().text = "Cannot Learn ";
                }
                else if (!containsIllegalAbility)
                {
                    bool AlreadyHaveAbility = false;
                    for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureAbilities.Length; i++)
                    {
                        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureAbilities[i] != null)
                        {
                            if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureAbilities[i].ability == AddReplaceAbilityOptions.Instance.currentSelectedAbility)
                            {
                                cannotLearnAbility.gameObject.SetActive(true);
                                cannotLearnAbility.GetComponentInChildren<TextMeshProUGUI>().text = "Already Learnt";
                                AlreadyHaveAbility = true;
                                break;
                            }
                        }                    
                    }
                    Debug.Log(AlreadyHaveAbility);
                    if (AlreadyHaveAbility == false)
                    {
                            cannotLearnAbility.gameObject.SetActive(false);
                    }
                } 
            }
           

            EmptyParty.SetActive(false);
            CreatureDetails.SetActive(true);
            CreatureIcon.sprite = stats.creatureSO.creaturePlayerIcon;
            CreatureName.text = stats.creatureSO.creatureName;
            HpSlider.maxValue = stats.creatureStats.MaxHP;
            HpSlider.value = stats.creatureStats.HP;
            RemainingHP.text = stats.creatureStats.HP.ToString() + "/" + stats.creatureStats.MaxHP.ToString();
            level.text = "LVL:" + stats.creatureStats.level.ToString();
            creature = stats;
            SetStatusEffects(creature);

            if (HpSlider.value == 0) {

            }
        }
    }
    public IEnumerator UpdateHPSlider(int value)
    {
        CoreUI.Locked = true;
        int startValue = (int)HpSlider.value;
        HpSlider.DOValue(value, 1f);
        UpdateSliderEvent();
        yield return new WaitForSeconds(1f);
    }
    public void UpdateSliderEvent()
    {
        StartCoroutine(UpdateText());
    }
    public IEnumerator UpdateText()
    {
        float duration = 1f;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            RemainingHP.text = (Mathf.RoundToInt(HpSlider.value)).ToString() + "/" + creature.creatureStats.MaxHP;
            yield return null;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSwapingWithButton = false;
        if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        if (dragCopy != null) {
            Destroy(dragCopy);
        }
        startingIndex = transform.GetSiblingIndex();
        startPos = transform.position;
        dragCopy = Instantiate(gameObject, CoreUI.Instance.PartyOptions.transform, true);
        dragCopy.transform.eulerAngles = new Vector3(0, 0, 2);
        dragCopy.GetComponent<PartyCreatureUI>().selectedBorder.SetActive(true);
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        if (BattleController.Instance.MasterPlayerParty.party[startingIndex].creatureStats.HP <= 0)
            return;
        SetCreatureIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        currentPos = Input.mousePosition;
        dragCopy.transform.position = new Vector3(dragCopy.transform.position.x, currentPos.y);
        if (BattleController.Instance.MasterPlayerParty.party[startingIndex].creatureStats.HP <= 0)
            return;
        for (int i = 0; i < CoreUI.Instance.PartyOptions.PartyCreatureUIs.Count; i++)
            if (RectTransformUtility.RectangleContainsScreenPoint(CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<RectTransform>(), currentPos) && i != startingIndex)
            {
                CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha = 0.75f;
                CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position = new Vector3(CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].transform.position.x + 50, CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position.y);
            }
            else if (i != startingIndex)
            {
                CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha = 1f;
                CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position = new Vector3(CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].transform.position.x, CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position.y);
            }
            else {
                CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position = new Vector3(CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].transform.position.x, CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position.y);
            }
        SetCreatureIndex();
    }

    //User for Drag and Drop change party in World Menu
    public void OnEndDrag(PointerEventData eventData)
    {
        if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        selectedBorder.SetActive(false);
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        Destroy(dragCopy);
        dragCopy = null;
        if (BattleController.Instance.MasterPlayerParty.party[startingIndex].creatureStats.HP <= 0)
            return;
        for (int i = 0; i < CoreUI.Instance.PartyOptions.PartyCreatureUIs.Count; i++)
        {
            if (CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha == 0 || CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha == 0.75f)
            {
                CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha = 1;
            }
            if (RectTransformUtility.RectangleContainsScreenPoint(CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<RectTransform>(), currentPos))
            {
                int draggedIndex = startingIndex;
                int droppedIndex = i;

                SwapCreature(droppedIndex);
                transform.position = startPos;
                break;
            }

        }
        SetCreatureIndex();
        transform.position = startPos;
        currentPos = Vector3.zero;
    }
    public void SwapCreatureViaButton() {

        Debug.Log(isSwapingWithButton);
        if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
        {
            //Add Audio
            BattleController.Instance.AttackController.SwitchPlayerCreature(creatureIndex);
            return;
        }

        if (isSwapingWithButton == true)
        {
            int indexToSwap = 0;

            foreach (Transform child in transform.parent)
            {
                if (child != transform)
                {
                    continue;
                }
                else
                {
                    CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].SelectedBorder.SetActive(false);
                    indexToSwap = child.GetSiblingIndex();
                    isSwapingWithButton = false;
                    SwapCreature(indexToSwap);
                    SetCreatureIndex();
                    break;
                }
            }
        }
        else
        {
            CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].SelectedBorder.SetActive(false);
            foreach (Transform child in transform.parent)
            {
                if (child != transform)
                {
                    continue;
                }
                else
                {
                    startingIndex = child.GetSiblingIndex();
                    CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].SelectedBorder.SetActive(true);
                    isSwapingWithButton = true;
                    break;
                }
            }
        }
    }
    private void SwapCreature(int droppedIndex)
    {
        BattleController.Instance.SwapPartyIndex(startingIndex, droppedIndex);
        PartyCreatureUI one = CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex];
        PartyCreatureUI two = CoreUI.Instance.PartyOptions.PartyCreatureUIs[droppedIndex];

        CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].creatureIndex = droppedIndex;
        CoreUI.Instance.PartyOptions.PartyCreatureUIs[droppedIndex].creatureIndex = startingIndex;
        CoreUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex] = two;
        CoreUI.Instance.PartyOptions.PartyCreatureUIs[droppedIndex] = one;
    }

    public void SetCreatureIndex() {
        for (int i = 0; i < CoreUI.Instance.PartyOptions.PartyCreatureUIs.Count; i++) {

            CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.SetSiblingIndex(i);
            CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].creatureIndex = CoreUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.GetSiblingIndex();
        }
    }
    private void SetStatusEffects(PlayerCreatureStats player)
    {
        for (int i = 0; i < StatusEffectUI.statusEffects.Count; i++)
        {
            StatusEffectUI.statusEffects[i].SetActive(false);
        }
        for (int i = 0; i < player.ailments.Count; i++)
        {
            if (player.ailments[i] != null)
                StatusEffectUI.ReturnElement(player.ailments[i]).SetActive(true);
        }
        for (int i = player.ailments.Count - 1; i > 0; i--)
        {
            if (player.ailments[i] == null)
                player.ailments.RemoveAt(i);
        }
    }
}
