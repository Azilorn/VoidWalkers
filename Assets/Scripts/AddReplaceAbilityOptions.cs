using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class AddReplaceAbilityOptions : MonoBehaviour
{
    public static AddReplaceAbilityOptions Instance;
    public bool creatureSelected;
    public int creatureStats;
    public int buttonIndex;
    public Ability currentSelectedAbility;
    public Ability replaceAbility;
    public List<CreatureDetailsAbility> creatureDetailsAbilities = new List<CreatureDetailsAbility>();

    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI primaryElementText;
    public TextMeshProUGUI description;

    public TextMeshProUGUI confirmationText;

    public Transform optionSelectBody;
    public Transform yesButton;
    public Transform noButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        currentSelectedAbility = null;
    }
    public void OpenCreatureSelectAbilityMenu(Ability a)
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
            return;
        currentSelectedAbility = a;
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.AddReplaceAbility;
        WorldMenuUI.Instance.OpenAndSetParty();
    }
    public void SetAddReplaceAbilityMenu(int c)
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        creatureSelected = true;
        replaceAbility = null;
        creatureStats = c;
        abilityNameText.text = currentSelectedAbility.abilityName;
        powerText.text = currentSelectedAbility.abilityStats.power.ToString();
        accuracyText.text = currentSelectedAbility.abilityStats.accuracy.ToString();
        primaryElementText.text = currentSelectedAbility.elementType.ToString();
        primaryElementText.color = ElementMatrix.Instance.ReturnElementColor(currentSelectedAbility.elementType);
        description.text = currentSelectedAbility.abilityBio;

        PlayerParty playerParty = BattleController.Instance.MasterPlayerParty;
        for (int i = 0; i < creatureDetailsAbilities.Count; i++)
        {
            if (playerParty.party[creatureStats].creatureAbilities[i] == null)
            {
                creatureDetailsAbilities[i].SetDetails("Empty", 0, 0, null);
                continue;
            }
            else
            {
                creatureDetailsAbilities[i].SetDetails(playerParty.party[creatureStats].creatureAbilities[i].ability.abilityName,
                playerParty.party[creatureStats].creatureAbilities[i].remainingCount,
                playerParty.party[creatureStats].creatureAbilities[i].ability.abilityStats.maxCount,
                playerParty.party[creatureStats].creatureAbilities[i].ability);
            }
        }
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
        transform.GetChild(0).DOScale(1, 0.35f);
        canvasGroup.DOFade(1, 0.2f);
        canvasGroup.blocksRaycasts = true;
    }
    public void ReplaceAbilityOnClick(Ability a, int i) {

        buttonIndex = i;
        replaceAbility = a;
        optionSelectBody.localScale = Vector3.zero;
        confirmationText.text = ReturnConfirmationText(replaceAbility);
        optionSelectBody.gameObject.SetActive(true);
        optionSelectBody.DOScale(1, 0.35f);
    }
    public void YesOnClickButton()
    {
       
        PlayerParty playerParty = BattleController.Instance.MasterPlayerParty;
        if (playerParty.party[creatureStats].creatureAbilities[buttonIndex] == null)
        {
            playerParty.party[creatureStats].creatureAbilities[buttonIndex] = new CreatureAbility();
        }
        playerParty.party[creatureStats].creatureAbilities[buttonIndex].ability = currentSelectedAbility;
        playerParty.party[creatureStats].creatureAbilities[buttonIndex].remainingCount = playerParty.party[creatureStats].creatureAbilities[buttonIndex].ability.abilityStats.maxCount;

        optionSelectBody.DOScale(0, 0.35f);
        transform.GetChild(0).DOScale(0, 0.35f);

        InventoryController.Instance.RemoveAbility(currentSelectedAbility);
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        WorldMenuUI.Instance.OpenAndSetInventory();
        StartCoroutine(WorldMenuUI.CloseMenu(gameObject, 0, 0.35f));
    }
    public void NoOnClickButton() {

        optionSelectBody.DOScale(0, 0.35f);
        replaceAbility = null;
        creatureSelected = false;
    }

    private string ReturnConfirmationText(Ability a)
    {
        string s = "";

        if (replaceAbility != null)
            s = "Are you sure you want to replace <color=#7ED1CA>" + currentSelectedAbility.abilityName + "</color> with <color=#7ED1CA>" + a.abilityName + "</color>?";
        else s = "Are you sure you want to use <color=#7ED1CA>" + currentSelectedAbility.abilityName + "</color> on an empty slot?";

        return s;
    }
    public void BackButton() {

        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0, 0.35f);
        canvasGroup.blocksRaycasts = false;
        transform.GetChild(0).DOScale(0, 0.35f);
        WorldMenuUI.Instance.OpenAndSetParty();
    }
}
