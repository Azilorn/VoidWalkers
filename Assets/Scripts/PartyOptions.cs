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

        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
            return;
        if (menuClosing == false)
        {
            if(CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                StartCoroutine(OnMenuBackwards());
            else StartCoroutine(OnMenuBackwardsWorld());
        }
    }
    public IEnumerator OnMenuBackwards()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
        {
            previousMenu.SetActive(true);
            previousMenu.transform.DOScale(Vector3.one, 0.25f);
            CoreUI.DoFadeIn(previousMenu, 0.35f);
        }
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.Normal)
        {
            if (AttackController.Turncount != 1)
            {
                previousMenu.SetActive(true);
                previousMenu.transform.DOScale(Vector3.one, 0.25f);
                CoreUI.DoFadeIn(previousMenu, 0.35f);
                new WaitForSeconds(0.25f);
            }
        }
        else if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            CoreUI.Instance.OpenAndSetInventory();
        }
        CoreUI.DoFadeOut(gameObject, 0.35f);
        yield return new WaitForSeconds(0.35f);
        CoreUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        gameObject.SetActive(false);
        menuClosing = false;
     
    }
    public IEnumerator OnMenuBackwardsIgnoreMenuStatus()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
        {
          
        }
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.Normal)
        {
            if (AttackController.Turncount == 1)
            {
                Debug.Log("TurnCount: " + AttackController.Turncount);
                previousMenu.SetActive(true);
                previousMenu.transform.DOScale(Vector3.one, 0.25f);
                CoreUI.DoFadeIn(previousMenu, 0.35f);
                yield return new WaitForSeconds(0.25f);
            }
        }
        else if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            CoreUI.Instance.OpenAndSetInventory();
            CoreUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        CoreUI.DoFadeOut(gameObject, 0.35f);
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        menuClosing = false;

    }

    public IEnumerator OnMenuBackwardsWorld()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            CoreUI.Instance.OpenAndSetInventory();
            CoreUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive)
        {
            CoreUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
            CoreUI.Instance.OpenAndSetInventory();
        }
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive)
        {
            CoreUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        if (CoreUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility && AddReplaceAbilityOptions.Instance.creatureSelected == false)
        {
            CoreUI.Instance.OpenAndSetInventory();
            CoreUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        CoreUI.DoFadeOut(gameObject, 0.35f);
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
            else if (BattleController.Instance.MasterPlayerParty.party[i] == null)
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
                continue;
            }
            else if (BattleController.Instance.MasterPlayerParty.party[i].creatureSO != null)
            {
                partyCreatureUIs[i].SetPartyCreatureUI(false, BattleController.Instance.MasterPlayerParty.party[i]);
                continue;
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
            else if (BattleController.Instance.MasterPlayerParty.party[i] == null)
            {
                partyCreatureUIs[i].gameObject.SetActive(false);
                continue;
            }
            else if (BattleController.Instance.MasterPlayerParty.party[i].creatureSO != null)
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
