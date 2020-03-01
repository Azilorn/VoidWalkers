using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class AttackDetailsUI : MonoBehaviour, IUIMenu
{

    private bool menuClosing = false;

    public Ability ability;
    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI abilityDescriptionText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI AccuracyText;
    public TextMeshProUGUI elementType;
    public Image Border;
    public GameObject MainBody;
    public List<Sprite> abilityTypeSprites = new List<Sprite>();

    private void OnEnable()
    {
        MainBody.transform.localScale = Vector3.zero;
    }

    public IEnumerator OnMenuActivated()
    {
        throw new System.NotImplementedException();
    }
    public void OnMenuBackwards(bool option)
    {
        if (menuClosing == false)
            StartCoroutine(OnMenuBackwardsBattle());
    }
    public IEnumerator OnMenuBackwardsBattle()
    {
        menuClosing = true;
        MainBody.transform.DOScale(Vector3.zero, 0.25f);
        CoreUI.DoFadeOut(MainBody, 0.15f);
        CoreUI.DoFadeOut(gameObject, 0.15f);
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        menuClosing = false;
    }

    public IEnumerator OnMenuDeactivated()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator OnMenuFoward()
    {
        throw new System.NotImplementedException();
    }

    public void SetMenu(Ability a) {

        if (a == null)
            return;
        ability = a;
        abilityNameText.text = a.abilityName;
        abilityDescriptionText.text = a.abilityBio;
        powerText.text = a.abilityStats.power.ToString();
        AccuracyText.text = a.abilityStats.accuracy.ToString();
        elementType.text = a.elementType.ToString();
        elementType.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
        Border.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
    }

    private Sprite ReturnSprite(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.None:
                return null;
            case AbilityType.Attack:
                return abilityTypeSprites[0];
            case AbilityType.Weather:
                return abilityTypeSprites[1];
            case AbilityType.Buff:
                return abilityTypeSprites[2];
            case AbilityType.Debuff:
                return abilityTypeSprites[3];
            case AbilityType.Other:
                return abilityTypeSprites[4];
            case AbilityType.AttackSelf:
                return abilityTypeSprites[0];
        }
        return null;
    }
}