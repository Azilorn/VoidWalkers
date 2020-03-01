using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class CreatureListOptionUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public Image icon;
    public TextMeshProUGUI creatureName;
    public TextMeshProUGUI creatureNumber;
    public CreatureSO creatureSO;
    public NewCreatureUIDetails creatureDetails;
    private float holdTimer;
    private float holdDurationRequired = 0.35f;
    private bool buttonClicked;
    private bool buttonHeld;
    private bool dragging = false;
    [SerializeField] private ScrollRect scrollRect;

    public void LateUpdate()
    {
        if (dragging)
            return;
       
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                OpenMenu();
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                if(NewGameSelectCreatureUI.Instance.gameObject.activeInHierarchy)
                    SetSelectedCreature();
                buttonClicked = false;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragging)
            return;
        holdTimer = 0;
        buttonHeld = true;
        buttonClicked = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragging)
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

        dragging = true;
        buttonClicked = false;
        buttonHeld = false;
        holdTimer = 0;
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        buttonClicked = false;
        buttonHeld = false;
        holdTimer = 0;
        scrollRect.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        buttonClicked = false;
        buttonHeld = false;
        holdTimer = 0;
        scrollRect.OnDrag(eventData);
    }
    public void SetCreatureListOption(CreatureSO creature) {

        icon.sprite = creature.creaturePlayerIcon;
        creatureName.text = creature.creatureName;
        string creatureNumberString = creature.name;
        string replacementString = "#";
        creatureNumberString = creatureNumberString.Replace("C.", replacementString);
        creatureNumber.text = creatureNumberString;
        creatureSO = creature;
    }

    public void SetSelectedCreature() {

        MenuTransitionsController.Instance.StartTransition(0, true);
        StartCoroutine(NewGameSelectCreatureUI.creatureSelectOptions[NewGameSelectCreatureUI.currentlySelectedOption].SetCreatureOptionCoroutine(creatureSO, 0.3f));
        NewGameSelectCreatureUI.creaturesSelected[NewGameSelectCreatureUI.currentlySelectedOption] = creatureSO;
    }
    public void OpenMenu() {

        creatureDetails.SetMenu(creatureSO);
        creatureDetails.gameObject.SetActive(true);
        CoreUI.DoFadeIn(creatureDetails.gameObject, 0.25f);
        StartCoroutine(CoreUI.OpenMenu(creatureDetails.MainBody.gameObject, 0f, 0.25f));
        StartCoroutine(CoreUI.OpenMenu(creatureDetails.AbilitiesBody.gameObject, 0f, 0.25f));
    }
}
