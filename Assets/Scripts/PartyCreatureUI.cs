using System.Collections;
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

    public static GameObject dragCopy;

    public void OnEnable()
    {
        creatureIndex = transform.GetSiblingIndex();
    }
    public void Update()
    {
        if (detailsUI.gameObject.activeInHierarchy)
            return;
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {

                if (!WithinBounds(gameObject, currentPos) && dragCopy == null)
                {
                    detailsUI.SetMenu(creature);
                    detailsUI.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(detailsUI.gameObject, 0.25f);
                    BattleUI.DoFadeIn(detailsUI.MainBody.gameObject, 0.25f);
                    BattleUI.DoFadeIn(detailsUI.AbilitiesBody.gameObject, 0.25f);
                    StartCoroutine(BattleUI.OpenMenu(detailsUI.MainBody.gameObject, 0f, 0.25f));
                    StartCoroutine(BattleUI.OpenMenu(detailsUI.AbilitiesBody.gameObject, 0f, 0.25f));
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
                if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                {
                    //Add Audio
                    BattleController.Instance.AttackController.SwitchPlayerCreature(creatureIndex);
                    ItemController.Instance.UseItem(creatureIndex);
                }
                else {
                    if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive || BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive)
                    {
                        //Add Audio
                        ItemController.Instance.CurrentlySelectedItem = 12;
                        ItemController.Instance.UseItem(creatureIndex);
                    }
                    else if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility)
                    {
                        //Add Audio
                        AddReplaceAbilityOptions.Instance.SetAddReplaceAbilityMenu(creatureIndex);
                        WorldMenuUI.Instance.CloseParty();
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
    public void SetPartyCreatureUI(bool empty, PlayerCreatureStats stats)
    {
        if (empty)
        {
            EmptyParty.SetActive(true);
            CreatureDetails.SetActive(false);
        }
        else {
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
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        if (dragCopy != null) {
            Destroy(dragCopy);
        }
        startingIndex = transform.GetSiblingIndex();
        startPos = transform.position;
        dragCopy = Instantiate(gameObject, WorldMenuUI.Instance.PartyOptions.transform, true);
        dragCopy.transform.eulerAngles = new Vector3(0, 0, 2);
        GetComponent<CanvasGroup>().alpha = 0;
        if (BattleController.Instance.MasterPlayerParty.party[startingIndex].creatureStats.HP <= 0)
            return;
        SetCreatureIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        currentPos = Input.mousePosition;
        dragCopy.transform.position = new Vector3(dragCopy.transform.position.x, currentPos.y);
        if (BattleController.Instance.MasterPlayerParty.party[startingIndex].creatureStats.HP <= 0)
            return;
        for (int i = 0; i < WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs.Count; i++)
            if (WithinBounds(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].gameObject, currentPos) && i != startingIndex)
            {
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha = 0.75f;
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position = new Vector3(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].transform.position.x + 50, WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position.y);
            }
            else if (i != startingIndex)
            {
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha = 1f;
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position = new Vector3(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].transform.position.x, WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position.y);
            }
            else {
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position = new Vector3(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].transform.position.x, WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.position.y);
            }
        SetCreatureIndex();
    }

    //User for Drag and Drop change party in World Menu
    public void OnEndDrag(PointerEventData eventData)
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        GetComponent<CanvasGroup>().alpha = 1;
        Destroy(dragCopy);
        dragCopy = null;
        if (BattleController.Instance.MasterPlayerParty.party[startingIndex].creatureStats.HP <= 0)
            return;
        for (int i = 0; i < WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs.Count; i++)
        {
            if (WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha == 0 || WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha == 0.75f)
            {
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].GetComponent<CanvasGroup>().alpha = 1;
            }
            if (WithinBounds(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].gameObject, currentPos) && i != startingIndex)
            {
                int draggedIndex = startingIndex;
                int droppedIndex = i;

                BattleController.Instance.SwapPartyIndex(startingIndex, droppedIndex);
                PartyCreatureUI one = WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex];
                PartyCreatureUI two = WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[droppedIndex];

                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex].creatureIndex = droppedIndex;
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[droppedIndex].creatureIndex = startingIndex;
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[startingIndex] = two;
                WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[droppedIndex] = one;
                transform.position = startPos;
                break;
            }
           
        }
        SetCreatureIndex();
        transform.position = startPos;
        currentPos = Vector3.zero;
    }
    public void SetCreatureIndex() {
        for (int i = 0; i < WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs.Count; i++) {

            WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.SetSiblingIndex(i);
            WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].creatureIndex = WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[i].transform.GetSiblingIndex();
        }
    }
    private bool WithinBounds(GameObject gameObject, Vector3 currentPos)
    {
        if (currentPos.y > gameObject.transform.position.y - 30 && currentPos.y < gameObject.transform.position.y + 30 &&
            currentPos.x > gameObject.transform.position.x - 250 && currentPos.x < gameObject.transform.position.x + 250)
        {
            return true;
        }
        else return false;
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
