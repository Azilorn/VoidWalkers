using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatureDetailsAbility : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Ability ability;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI remainingCount;
    public TextMeshProUGUI elementType;
    public Image Border;
    public Image InfoButton;
    public int buttonIndex;
    private float holdTimer;
    [SerializeField] private float holdDurationRequired;
    private bool buttonClicked;
    private bool buttonHeld;
    [SerializeField] private AttackDetailsUI attackDetails;

    private void Awake()
    {
        buttonIndex = transform.GetSiblingIndex();
    }
    public void LateUpdate()
    {
      
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                    
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                if (AddReplaceAbilityOptions.Instance != null && SceneManager.GetActiveScene().buildIndex == 2 && BattleUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility)
                {
                    if (AddReplaceAbilityOptions.Instance.gameObject.activeInHierarchy && BattleUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility)
                    {
                        AddReplaceAbilityOptions.Instance.ReplaceAbilityOnClick(ability, buttonIndex);
                        Debug.Log("ReplaceAbility");
                    }
                }
                else if (SceneManager.GetActiveScene().buildIndex == 2 && BattleUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
                {
                    Debug.Log("UseAp");
                    StartCoroutine(ItemController.Instance.UseAP(buttonIndex, gameObject.GetComponent<CreatureDetailsAbility>()));
                }                
                buttonClicked = false;
            }
        }
    }

    public void SetAttackInfo()
    {
        Debug.Log("AttackMenu");
        attackDetails.SetMenu(ability);
        attackDetails.gameObject.SetActive(true);
        BattleUI.DoFadeIn(attackDetails.gameObject, 0.15f);
        StartCoroutine(BattleUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
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
    public void SetDetails(string name, int rc, int mc, Ability a) {

        abilityName.text = name;
        if (rc == 0 && mc == 0)
            remainingCount.text = "";
        else
            remainingCount.text = rc + "/" + mc;
        ability = a;
        if (elementType.text != null && a != null)
        {
            elementType.text = a.elementType.ToString();
            elementType.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
        }
        else {
            elementType.text = "";
            elementType.color = Color.grey;
        }
        if (Border != null && a != null)
            Border.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
        else {
            Border.color = Color.grey;
        }
        if (InfoButton != null && a != null)
        {
            InfoButton.color = ElementMatrix.Instance.ReturnElementColor(a.elementType);
            InfoButton.gameObject.SetActive(true);
        }
        else
        {
            InfoButton.gameObject.SetActive(false);
            InfoButton.color = Color.grey;
        }
    }
}
