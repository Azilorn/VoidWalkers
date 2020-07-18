using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemMenuDetails : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private AttackDetailsUI attackDetails;
    [SerializeField] private GameObject addReplaceAbilityOptions;
    private Item itm;
    private Ability ability;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private int itemIndex;
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
                SetItemInformation();
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                if (ItemOptions.lastItemSelectedMenu == 2)
                {
                    if (AddReplaceAbilityOptions.Instance == null) {
                        addReplaceAbilityOptions.gameObject.SetActive(true);
                    }
                    AddReplaceAbilityOptions.Instance.OpenCreatureSelectAbilityMenu(ability);
                }
                else {
                    SetMenuHelper();

                }
                buttonClicked = false;
            }
        }
    }

    public void SetItemInformation()
    {
        if (ability != null)
        {
            attackDetails.SetMenu(ability);
        }
        if (ItemOptions.lastItemSelectedMenu == 2)
        {
            attackDetails.gameObject.SetActive(true);
            CoreUI.DoFadeIn(attackDetails.gameObject, 0.15f);
            StartCoroutine(CoreUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
        }
    }

    public void SetItemDetails(Item item, int count, int i) {

        ability = null;
        itm = item;
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        itemCount.text = "x " + count.ToString();
        itemDescription.gameObject.SetActive(true);
        itemDescription.text = item.bio;
        itemIndex = i;
    }
    public void SetAbilityDetails(Ability a, int count)
    {
        itm = null;
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = a.icon;
        ability = a;
        itemName.text = a.abilityName;
        itemCount.text = "x " + count.ToString();
        itemDescription.gameObject.SetActive(false);
    }
    public void SetMenuHelper() {

        if (ItemOptions.lastItemSelectedMenu == 2) {
            Debug.Log("SetAbility");
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
}
