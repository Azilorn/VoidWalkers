using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureDetailsUI : MonoBehaviour, IUIMenu
{

    private bool menuClosing = false;

    private PlayerCreatureStats creatureStats;
    [SerializeField] private Image creatureImage;
    [SerializeField] private Image creatureImageShadow;
    [SerializeField] private TextMeshProUGUI creatureName;
    [SerializeField] private TextMeshProUGUI creatureBio;
    [SerializeField] private TextMeshProUGUI primaryElement;
    [SerializeField] private TextMeshProUGUI secondaryElement;
    [SerializeField] private TextMeshProUGUI height;
    [SerializeField] private TextMeshProUGUI weight;
    [SerializeField] private TextMeshProUGUI level;

    [SerializeField] private List<TextMeshProUGUI> statsTexts = new List<TextMeshProUGUI>();
    [SerializeField] private List<CreatureDetailsAbility> abilityTexts = new List<CreatureDetailsAbility>();

    [SerializeField] private GameObject mainBody;
    [SerializeField] private GameObject abilitiesBody;

    public GameObject MainBody { get => mainBody; set => mainBody = value; }
    public GameObject AbilitiesBody { get => abilitiesBody; set => abilitiesBody = value; }

    private void OnEnable()
    {
        MainBody.transform.localScale = Vector3.zero;
        AbilitiesBody.transform.localScale = Vector3.zero;
        creatureImageShadow.transform.localScale = Vector3.zero;
        creatureImageShadow.transform.DOScale(1, 0.35f);
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
        mainBody.transform.DOScale(Vector3.zero, 0.25f);
        BattleUI.DoFadeOut(mainBody, 0.15f);
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

    public void SetMenu(PlayerCreatureStats stats)
    {
        creatureStats = stats;
        CreatureSO creature = stats.creatureSO;
        creatureImageShadow.sprite = stats.creatureSO.creaturePlayerIcon;
        creatureImage.sprite = stats.creatureSO.creaturePlayerIcon;

        creatureName.text = stats.creatureSO.creatureName;
        creatureBio.text = stats.creatureSO.creatureBio;
        if (creature.primaryElement != ElementType.None)
        {
            primaryElement.text = creature.primaryElement.ToString();
            primaryElement.color = ElementMatrix.Instance.ReturnElementColor(creature.primaryElement);
            primaryElement.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            primaryElement.text = "None";
            primaryElement.color = Color.grey;
            primaryElement.transform.parent.gameObject.SetActive(true);
        }
        if (creature.secondaryElement != ElementType.None)
        {
            secondaryElement.text = creature.secondaryElement.ToString();
            secondaryElement.color = ElementMatrix.Instance.ReturnElementColor(creature.secondaryElement);
            secondaryElement.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            secondaryElement.text = "None";
            secondaryElement.color = Color.grey;
            secondaryElement.transform.parent.gameObject.SetActive(true);
        }

        weight.text = stats.creatureSO.characteristics.weight.ToString() + " lbs";
        height.text = stats.creatureSO.characteristics.height.ToString();
        level.text = "LVL:" + stats.creatureStats.level.ToString();

        statsTexts[0].text = stats.creatureStats.HP.ToString() + "/" + stats.creatureStats.MaxHP.ToString();
        statsTexts[1].text = stats.creatureStats.strength.ToString();
        statsTexts[2].text = stats.creatureStats.defence.ToString();
        statsTexts[3].text = stats.creatureStats.speed.ToString();
        statsTexts[4].text = stats.creatureStats.criticalAttack.ToString();
        statsTexts[5].text = stats.creatureStats.criticalDefence.ToString();

        for (int i = 0; i < stats.creatureAbilities.Length; i++) {

            if (stats.creatureAbilities[i] != null) {

                if (stats.creatureAbilities[i].ability != null)
                {
                    abilityTexts[i].gameObject.SetActive(true);
                    abilityTexts[i].SetDetails(stats.creatureAbilities[i].ability.abilityName, stats.creatureAbilities[i].remainingCount, stats.creatureAbilities[i].ability.abilityStats.maxCount, stats.creatureAbilities[i].ability);
                }
                else abilityTexts[i].gameObject.SetActive(false);

            } else abilityTexts[i].gameObject.SetActive(false);
        }
    }
}
