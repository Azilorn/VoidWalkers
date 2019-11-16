using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemMenuDetails : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private ItemDetailsUI itemDetails;
    [SerializeField] private AttackDetailsUI attackDetails;

    private Item itm;
    private Ability ability;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private int itemIndex;
    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDurationRequired;
    [SerializeField] private bool buttonClicked;
    [SerializeField] private bool buttonHeld;
    private bool dragging = false;
    [SerializeField] private ScrollRect scrollRect;

    public void LateUpdate()
    {
        if (dragging)
            return;
        if (itemDetails.gameObject.activeInHierarchy)
            return;
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                if (itm != null)
                {
                    itemDetails.SetMenu(itm);
                }
                else if(ability != null)
                {
                    attackDetails.SetMenu(ability);
                }
                if (ItemOptions.lastItemSelectedMenu == 2)
                {
                    attackDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(attackDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else {
                    itemDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(itemDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(itemDetails.MainBody.gameObject, 0f, 0.25f));
                }
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                SetMenuHelper();
                buttonClicked = false;
            }
        }
    }


    public void SetItemDetails(Item item, int count, int i) {

        ability = null;
        itm = item;
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        itemCount.text = "x " + count.ToString();
        itemIndex = i;
    }
    public void SetAbilityDetails(Ability a, int count)
    {
        itm = null;
        itemIcon.gameObject.SetActive(false);
        ability = a;
        itemName.text = a.abilityName;
        itemCount.text = "x " + count.ToString();
    }
    public void SetMenuHelper() {

        if (ItemOptions.lastItemSelectedMenu == 2) {

        }
        else
        {
            ItemController.Instance.SetMenuOption(itemIndex);
            ItemController.Instance.CurrentlySelectedItem = itemIndex;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragging)
            return;
        if (itemDetails.gameObject.activeInHierarchy)
            return;
        holdTimer = 0;
        buttonHeld = true;
        buttonClicked = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragging)
            return;
        if (itemDetails.gameObject.activeInHierarchy)
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
}
