using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RewardContentUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool collected = false;
    private CanvasGroup cg;
    private Vector3 startingPos;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI rewardName;
    [SerializeField] private TextMeshProUGUI rewardDescription;

    private CreatureSO creature;
    private Item itm;
    private RelicSO relic;
    private Ability ability;
    public CreatureSO Creature { get => creature; set => creature = value; }
    public Item Itm { get => itm; set => itm = value; }
    public RelicSO Relic { get => relic; set => relic = value; }
    public Ability Ability { get => ability; set => ability = value; }

    public Image Icon { get => icon; set => icon = value; }
    public TextMeshProUGUI RewardName { get => rewardName; set => rewardName = value; }
    public TextMeshProUGUI RewardDescription { get => rewardDescription; set => rewardDescription = value; }
    public bool Collected { get => collected; set => collected = value; }
    public Vector3 StartingPos { get => startingPos; set => startingPos = value; }

    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDurationRequired = 0.35f;
    [SerializeField] private bool buttonClicked;
    [SerializeField] private bool buttonHeld;

    [SerializeField] private ItemDetailsUI itemDetails;
    [SerializeField] private AttackDetailsUI attackDetails;
    [SerializeField] private NewCreatureUIDetails newCreatureDetails;

    [SerializeField] private RewardsScreen rewardscreen;

    private void OnEnable()
    {
        if (cg == null)
            cg = GetComponent<CanvasGroup>();
        if (cg == null)
            return;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void Start()
    {
        startingPos = transform.position;
        cg = GetComponent<CanvasGroup>();
    }
    public void Update()
    {
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                if (Ability != null)
                {
                    attackDetails.SetMenu(Ability);
                    attackDetails.gameObject.SetActive(true);
                    CoreUI.DoFadeIn(attackDetails.gameObject, 0.15f);
                    StartCoroutine(CoreUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else if (Itm != null)
                {
                    itemDetails.SetMenu(Itm);
                    itemDetails.gameObject.SetActive(true);
                    CoreUI.DoFadeIn(itemDetails.gameObject, 0.15f);
                    StartCoroutine(CoreUI.OpenMenu(itemDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else if (Relic != null)
                {
                    //TODO implement Relic Details Menu
                }
                else if (Creature != null)
                {
                    newCreatureDetails.SetMenu(Creature);
                    newCreatureDetails.gameObject.SetActive(true);
                    CoreUI.DoFadeIn(newCreatureDetails.gameObject, 0.25f);
                    StartCoroutine(CoreUI.OpenMenu(newCreatureDetails.MainBody.gameObject, 0f, 0.25f));
                    StartCoroutine(CoreUI.OpenMenu(newCreatureDetails.AbilitiesBody.gameObject, 0f, 0.25f));
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
                AddObjectToPlayer();

                int totalActiveCount = 0;
                foreach (RewardContentUI ui in rewardscreen.RewardGameObject.GetComponent<RewardScreenChoiceMenu>().rewardContentUIs) {
                    if (ui.gameObject.activeInHierarchy)
                    {
                        totalActiveCount++;
                    }
                }
                if (totalActiveCount <= 1) {
                    rewardscreen.RewardGameObject.GetComponent<RewardScreenChoiceMenu>().takeAll.SetActive(false);
                }
                buttonClicked = false;
            }
        }
    }

    public void NullObjects()
    {
        Creature = null;
        Itm = null;
        Relic = null;
        Ability = null;
    }

    public void AddObjectToPlayer()
    {
        if (Creature != null)
        {
           //TODO Create Creature Backlog mechanic
        }
        if (Itm != null)
        {
            InventoryController.Instance.AddItem(Itm);
        }
        if (Relic != null)
        {
            InventoryController.Instance.AddRelic(relic);
        }
        if (Ability != null)
        {
            InventoryController.Instance.AddAbility(ability);
        }
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 1), 0.35f, 1, 1f).SetDelay(0.15f);
        cg.DOFade(0, 0.5f).SetDelay(0.15f);
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        holdTimer = 0;
        buttonHeld = true;
        buttonClicked = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
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
}
