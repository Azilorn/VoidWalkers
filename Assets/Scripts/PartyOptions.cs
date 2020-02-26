using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class PartyOptions : MonoBehaviour, IUIMenu
{
    [SerializeField] private List<PartyCreatureUI> partyCreatureUIs = new List<PartyCreatureUI>();
    [SerializeField] private GameObject previousMenu;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject helpButton;
    bool menuClosing;
    public List<PartyCreatureUI> PartyCreatureUIs { get => partyCreatureUIs; set => partyCreatureUIs = value; }
    public GameObject BottomBar { get => bottomBar; set => bottomBar = value; }
    public GameObject Header { get => header; set => header = value; }
    public GameObject HelpButton { get => helpButton; set => helpButton = value; }

    public void OnEnable()
    {
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuOpenAudio, 1, false);
    }
    public IEnumerator OnMenuActivated()
    {
        throw new System.NotImplementedException();
    }
    public void OnMenuBackwards(bool option) {

        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
            return;
        if (menuClosing == false)
        {
            if(BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                StartCoroutine(OnMenuBackwardsBattle());
            else StartCoroutine(OnMenuBackwardsWorld());
        }
    }
    public IEnumerator OnMenuBackwardsBattle()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
        {
            previousMenu.SetActive(true);
            previousMenu.transform.DOScale(Vector3.one, 0.25f);
            BattleUI.DoFadeIn(previousMenu, 0.35f);
        }
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.Normal)
        {
            if (AttackController.Turncount != 1)
            {
                previousMenu.SetActive(true);
                previousMenu.transform.DOScale(Vector3.one, 0.25f);
                BattleUI.DoFadeIn(previousMenu, 0.35f);
                new WaitForSeconds(0.25f);
            }
        }
        else if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            WorldMenuUI.Instance.OpenAndSetInventory();
        }
        StartCoroutine(BattleUI.CloseMenuFromSideToCenter(GetGameObjects(), 0.02f, 0.35f, Camera.main.scaledPixelWidth * 2));
        BattleUI.DoFadeOut(gameObject, 0.35f);
        yield return new WaitForSeconds(0.35f);
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        gameObject.SetActive(false);
        menuClosing = false;
     
    }
    public IEnumerator OnMenuBackwardsIgnoreMenuStatus()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
        {
          
        }
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.Normal)
        {
            if (AttackController.Turncount == 1)
            {
                Debug.Log("TurnCount: " + AttackController.Turncount);
                previousMenu.SetActive(true);
                previousMenu.transform.DOScale(Vector3.one, 0.25f);
                BattleUI.DoFadeIn(previousMenu, 0.35f);
                yield return new WaitForSeconds(0.25f);
            }
        }
        else if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            WorldMenuUI.Instance.OpenAndSetInventory();
            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        StartCoroutine(BattleUI.CloseMenuFromSideToCenter(GetGameObjects(), 0.02f, 0.35f, Camera.main.scaledPixelWidth * 2));
        BattleUI.DoFadeOut(gameObject, 0.35f);
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        menuClosing = false;

    }

    public IEnumerator OnMenuBackwardsWorld()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            WorldMenuUI.Instance.OpenAndSetInventory();
            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive)
        {
            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
            WorldMenuUI.Instance.OpenAndSetInventory();
        }
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive)
        {
            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility && AddReplaceAbilityOptions.Instance.creatureSelected == false)
        {
            WorldMenuUI.Instance.OpenAndSetInventory();
            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
            transform.GetChild(0).GetChild(1).GetComponent<VerticalLayoutGroup>().enabled = false;
        StartCoroutine(WorldMenuUI.CloseMenuFromSideToCenter(GetGameObjects(), 0.02f, 0.35f, 1091));
        WorldMenuUI.DoFadeOut(gameObject, 0.35f);
        yield return new WaitForSeconds(0.45f);
        gameObject.SetActive(false);
        menuClosing = false;
    }

    public List<GameObject> GetGameObjects()
    {
        List<GameObject> go = new List<GameObject>();

        for (int i = 0; i < partyCreatureUIs.Count; i++)
        {
            go.Add(partyCreatureUIs[i].gameObject);
        }
        return go;
    }

    public IEnumerator OnMenuDeactivated()
    {
        throw new System.NotImplementedException();
    }
    public IEnumerator OnMenuFoward()
    {
        throw new System.NotImplementedException();
    }
    public void SetUI() {

        for (int i = 0; i < partyCreatureUIs.Count; i++)
        {
            if (i >= BattleController.Instance.MasterPlayerParty.party.Length)
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
                continue;
            }
            if (BattleController.Instance.MasterPlayerParty.party[i] == null)
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
                continue;
            }
            if (BattleController.Instance.MasterPlayerParty.party[i].creatureSO != null)
            {
                partyCreatureUIs[i].SetPartyCreatureUI(false, BattleController.Instance.MasterPlayerParty.party[i]);
            }
            else
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetUI(PlayerParty playerParty)
    {
        for (int i = 0; i < partyCreatureUIs.Count; i++)
        {
            if (i >= BattleController.Instance.MasterPlayerParty.party.Length) {
                partyCreatureUIs[i].gameObject.SetActive(false);
                continue;
            }
            if (BattleController.Instance.MasterPlayerParty.party[i] == null)
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
                continue;
            }
            if (BattleController.Instance.MasterPlayerParty.party[i].creatureSO != null)
            {
                partyCreatureUIs[i].SetPartyCreatureUI(false, BattleController.Instance.MasterPlayerParty.party[i]);
            }
            else
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
            }
        }
    }
}
