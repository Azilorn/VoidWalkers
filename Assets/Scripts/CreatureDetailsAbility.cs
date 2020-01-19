using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CreatureDetailsAbility : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    private Ability ability;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI remainingCount;
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
                attackDetails.SetMenu(ability);

                attackDetails.gameObject.SetActive(true);
                BattleUI.DoFadeIn(attackDetails.gameObject, 0.15f);
                StartCoroutine(BattleUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                if (AddReplaceAbilityOptions.Instance.gameObject.activeInHierarchy) {
                    AddReplaceAbilityOptions.Instance.ReplaceAbilityOnClick(ability, buttonIndex);
                }
                buttonClicked = false;
            }
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
    public void SetDetails(string name, int rc, int mc, Ability a) {

        abilityName.text = name;
        if (rc == 0 && mc == 0)
            remainingCount.text = "";
        else
            remainingCount.text = rc + "/" + mc;
        ability = a;
    }
}
