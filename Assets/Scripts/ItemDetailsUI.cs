﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsUI : MonoBehaviour, IUIMenu
{
    private bool menuClosing = false;

    public Item item;
    public GameObject MainBody;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI masterItemTypeText;
    public Image itemImage;
    public Image itemImageShadow;

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
        BattleUI.DoFadeOut(gameObject, 0.15f);
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

    public void SetMenu(Item itm)
    {
        item = itm;
        nameText.text = itm.itemName;
        descriptionText.text = itm.bio;
        masterItemTypeText.text = item.itemMasterType.ToString();
        itemImage.sprite = itm.itemIcon;
        itemImageShadow.sprite = item.itemIcon;
    }
    public void SetMenu(Ability a) {

    }
}
