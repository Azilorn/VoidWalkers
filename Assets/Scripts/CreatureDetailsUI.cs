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

    [SerializeField] private GameObject mainBody;

    public GameObject MainBody { get => mainBody; set => mainBody = value; }

    private void OnEnable()
    {
        MainBody.transform.localScale = Vector3.zero;
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
        creatureImageShadow.sprite = stats.creatureSO.creaturePlayerIcon;
        creatureImage.sprite = stats.creatureSO.creaturePlayerIcon;

        creatureName.text = stats.creatureSO.creatureName;
        creatureBio.text = stats.creatureSO.creatureBio;
        if (stats.creatureSO.primaryElement != ElementType.None)
        {
            primaryElement.text = stats.creatureSO.primaryElement.ToString();
            primaryElement.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            primaryElement.transform.parent.gameObject.SetActive(false);
        }
        if (stats.creatureSO.secondaryElement != ElementType.None)
        {
            secondaryElement.text = stats.creatureSO.secondaryElement.ToString();
            secondaryElement.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            secondaryElement.transform.parent.gameObject.SetActive(false);
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
    }

}
