using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RewardContentUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI rewardName;

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

    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDurationRequired = 0.35f;
    [SerializeField] private bool buttonClicked;
    [SerializeField] private bool buttonHeld;

    [SerializeField] private ItemDetailsUI itemDetails;
    [SerializeField] private AttackDetailsUI attackDetails;
    [SerializeField] private NewCreatureUIDetails newCreatureDetails;

    [SerializeField] private RewardsScreen rewardscreen;

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
                    BattleUI.DoFadeIn(attackDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else if (Itm != null)
                {
                    itemDetails.SetMenu(Itm);
                    itemDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(itemDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(itemDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else if (Relic != null)
                {
                    //TODO implement Relic Details Menu
                }
                else if (Creature != null)
                {
                    newCreatureDetails.SetMenu(Creature);
                    newCreatureDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(newCreatureDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(newCreatureDetails.MainBody.gameObject, 0f, 0.25f));
                    StartCoroutine(BattleUI.OpenMenu(newCreatureDetails.AbilitiesBody.gameObject, 0f, 0.25f));
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
                rewardscreen.CloseRewardMenu();
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
        Debug.Log("Creature: " + Creature + " Item: " + Itm  + " Relic: " + Relic  + " Ability: " + Ability );
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
