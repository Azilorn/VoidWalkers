using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AttackButtonUI : MonoBehaviour
{
    public AttackDetailsUI detailsUI;
    public Ability ability;
    public Image Border;
    public Image InfoButton;
    public TextMeshProUGUI attackNameText;
    public TextMeshProUGUI attackElementText;
    public TextMeshProUGUI attackCountText;
    public int abilityIndex;
    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDurationRequired;
    [SerializeField] private bool buttonClicked;
    [SerializeField] private bool buttonHeld;

    public void Update()
    {
        if (detailsUI.gameObject.activeInHierarchy)
            return;
        if (buttonHeld) {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld) {
            if (buttonClicked) {
                if (ability != null)
                {
                    BattleController.Instance.AttackController.AttackPlayerButton(abilityIndex);
                }
                buttonClicked = false;
            }
        }
    }

    public void SetItemInfo()
    {
        if (ability != null)
        {
            detailsUI.SetMenu(ability);
            detailsUI.gameObject.SetActive(true);
            CoreUI.DoFadeIn(detailsUI.gameObject, 0.25f);
            CoreUI.DoFadeIn(detailsUI.MainBody, 0.25f);
            StartCoroutine(CoreUI.OpenMenu(detailsUI.MainBody.gameObject, 0f, 0.25f));
        }
    }

    public void SetText(Ability a, int remainingCount, int index)
    {
        ability = a;
        attackNameText.text = a.abilityName;
        attackCountText.text = remainingCount.ToString() + "/" + a.abilityStats.maxCount.ToString();
        attackElementText.text = a.elementType.ToString();
        attackElementText.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
        Border.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
        InfoButton.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
        abilityIndex = index;
    }

    

    public void UpdateText(Ability a, int remainingCount)
    {
        attackCountText.text = remainingCount.ToString() + "/" + a.abilityStats.maxCount.ToString();
    }

    public void OnButtonClickedDown() {
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
        else if(holdTimer > holdDurationRequired)
        {
            buttonClicked = false;
            buttonHeld = false;
            holdTimer = 0;
        }
    }
}
