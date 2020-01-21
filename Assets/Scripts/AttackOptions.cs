using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackOptions : MonoBehaviour, IUIMenu
{
    private bool menuClosing = false;
    [SerializeField] private GameObject previousMenu;
    public List<AttackButtonUI> attackButtonUIs = new List<AttackButtonUI>();

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && this.gameObject.activeInHierarchy && !menuClosing)
        {
            StartCoroutine(OnMenuBackwardsBattle());
        }
    }
    public IEnumerator OnMenuActivated()
    {
        throw new System.NotImplementedException();
    }
    public void OnMenuBackwards(bool option)
    {
        if(menuClosing == false)
            StartCoroutine(OnMenuBackwardsBattle());
    }
    public IEnumerator OnMenuBackwardsBattle()
    {
        menuClosing = true;
        previousMenu.SetActive(true);
        gameObject.transform.DOScale(Vector3.zero, 0.25f);
        BattleUI.DoFadeOut(gameObject, 0.15f);
        previousMenu.transform.DOScale(Vector3.one, 0.25f);
        BattleUI.DoFadeIn(previousMenu, 0.35f);
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
    public void SetButtons(PlayerCreatureStats playerCreatureStats) {
        for (int i = 0; i < playerCreatureStats.creatureAbilities.Length; i++) {

            if (playerCreatureStats.creatureAbilities[i] == null) {
                attackButtonUIs[i].attackNameText.text = "None";
                attackButtonUIs[i].attackCountText.text = "";
                attackButtonUIs[i].GetComponent<Button>().interactable = false;
                attackButtonUIs[i].gameObject.SetActive(true);
                continue;
            } else
            if (playerCreatureStats.creatureAbilities[i].ability == null) {
                attackButtonUIs[i].attackNameText.text = "None";
                attackButtonUIs[i].attackCountText.text = "";
                attackButtonUIs[i].GetComponent<Button>().interactable = false;
                attackButtonUIs[i].gameObject.SetActive(true);
                continue;
            }
            attackButtonUIs[i].SetText(playerCreatureStats.creatureAbilities[i].ability, playerCreatureStats.creatureAbilities[i].remainingCount, i);
            attackButtonUIs[i].gameObject.SetActive(true);
            if (playerCreatureStats.creatureAbilities[i].remainingCount == 0)
            {
                attackButtonUIs[i].GetComponent<Button>().targetGraphic.raycastTarget = false;
                attackButtonUIs[i].GetComponent<Button>().interactable = false;
            }
            else {
                attackButtonUIs[i].GetComponent<Button>().targetGraphic.raycastTarget = true;
                attackButtonUIs[i].GetComponent<Button>().interactable = true;
            }
        }
    }
}
