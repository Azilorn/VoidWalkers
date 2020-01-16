using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class NewCreatureUIDetails : MonoBehaviour, IUIMenu
{
    private bool menuClosing = false;

    private CreatureSO creature;
    [SerializeField] private Image creatureImage;
    [SerializeField] private Image creatureImageShadow;
    [SerializeField] private TextMeshProUGUI creatureName;
    [SerializeField] private TextMeshProUGUI creatureBio;
    [SerializeField] private TextMeshProUGUI primaryElement;
    [SerializeField] private TextMeshProUGUI secondaryElement;
    [SerializeField] private TextMeshProUGUI weight;
    [SerializeField] private TextMeshProUGUI height;

    [SerializeField] private List<CreatureDetailsAbility> abilityTexts = new List<CreatureDetailsAbility>();

    [SerializeField] private GameObject mainBody;
    [SerializeField] private GameObject abilitiesBody;

    public GameObject MainBody { get => mainBody; set => mainBody = value; }
    public GameObject AbilitiesBody { get => abilitiesBody; set => abilitiesBody = value; }

    private void OnEnable()
    {
        MainBody.transform.localScale = Vector3.zero;
        AbilitiesBody.transform.localScale = Vector3.zero;
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
        BattleUI.DoFadeOut(MainBody, 0.15f);
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

    public void SetMenu(CreatureSO stats)
    {
        creature = stats;
        creatureImageShadow.sprite = creature.creaturePlayerIcon;
        creatureImage.sprite = creature.creaturePlayerIcon;

        creatureName.text = creature.creatureName;
        creatureBio.text = creature.creatureBio;
        if (creature.primaryElement != ElementType.None)
        {
            primaryElement.text = creature.primaryElement.ToString();
            primaryElement.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            primaryElement.transform.parent.gameObject.SetActive(false);
        }
        if (creature.secondaryElement != ElementType.None)
        {
            secondaryElement.text = creature.secondaryElement.ToString();
            secondaryElement.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            secondaryElement.transform.parent.gameObject.SetActive(false);
        }

        weight.text = creature.characteristics.weight.ToString() + " lbs";
        height.text = creature.characteristics.height.ToString();
        

        for (int i = 0; i < creature.startingAbilities.Count; i++)
        {
            if (creature.startingAbilities[i] != null)
            {
                abilityTexts[i].gameObject.SetActive(true);
                abilityTexts[i].SetDetails(creature.startingAbilities[i].abilityName, creature.startingAbilities[i].abilityStats.maxCount, creature.startingAbilities[i].abilityStats.maxCount, creature.startingAbilities[i]);
            }
            else abilityTexts[i].gameObject.SetActive(false);
        }
        for (int i = creature.startingAbilities.Count; i < 4; i++)
        {
            abilityTexts[i].gameObject.SetActive(false);
        }
    }

}
