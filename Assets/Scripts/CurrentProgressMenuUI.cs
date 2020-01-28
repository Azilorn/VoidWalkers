using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class CurrentProgressMenuUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject contents;
    public GameObject deleteSaveMenu;
    public GameObject continueButton;
    public List<CurrentProgressCreatureUI> creatureUIs = new List<CurrentProgressCreatureUI>();
    public TextMeshProUGUI saveDataDetails;
    public TextMeshProUGUI saveDetailsFloor;
    public TextMeshProUGUI saveDetailsGold;
    private bool buttonHeld;
    private bool buttonClicked;
    private float holdTimer;
    private float holdDurationRequired = 0.35f;

    public void OnEnable()
    {
        SetCurrentProgress();
        deleteSaveMenu.transform.localScale = Vector3.zero;
    }
    public void OnDisable()
    {
        for (int i = 0; i < creatureUIs.Count; i++)
        {
            creatureUIs[i].ClearUI();
        }
    }
    public void LateUpdate()
    {
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                if (File.Exists(Application.persistentDataPath + "/savedata.sav"))
                {
                    StartCoroutine(WorldMenuUI.OpenMenu(deleteSaveMenu, 0, 0.35f));
                    WorldMenuUI.DoFadeIn(deleteSaveMenu, 0.25f);
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
                buttonClicked = false;
                return;
            }
        }
    }
    public void DeleteSave() {
        SaveLoadManager.DeleteSave();
        SetUINoSave();
        StartCoroutine(WorldMenuUI.CloseMenu(deleteSaveMenu, 0, 0.35f));
        WorldMenuUI.DoFadeOut(deleteSaveMenu, 0.25f);
        continueButton.SetActive(false);
    }
    public void NoButton()
    {
        StartCoroutine(WorldMenuUI.CloseMenu(deleteSaveMenu, 0, 0.35f));
        WorldMenuUI.DoFadeOut(deleteSaveMenu, 0.25f);
    }
    public void SetCurrentProgress()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.sav"))
        {
            SetUISave();
        }
        else
        {
            SetUINoSave();
        }
    }

    private void SetUISave()
    {
        GameSaveDetailsData saveData = new GameSaveDetailsData();

        saveData = SaveLoadManager.LoadDataEventMenu();

        CreatureTable creatureTable = CreatureTable.Instance;

        DateTime dateTime = saveData.currentProgressDateTime;
        saveDataDetails.text = "Current Progress - " + dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString();
        saveDetailsFloor.text = saveData.Floor + "-" + saveData.ProgressOnCurrentFloor;
        saveDetailsGold.text = saveData.Gold.ToString();
        for (int i = 0; i < saveData.playerParty.Length; i++)
        {
            creatureUIs[i].creatureSO = creatureTable.Creatures[saveData.playerParty[i].CreatureSO];
            creatureUIs[i].image.sprite = creatureTable.Creatures[saveData.playerParty[i].CreatureSO].creaturePlayerIcon;
            creatureUIs[i].creatureLevel.text = saveData.playerParty[i].creatureStat.level.ToString();
            creatureUIs[i].gameObject.SetActive(true);
        }
        for (int i = saveData.playerParty.Length; i < creatureUIs.Count; i++)
        {
            creatureUIs[i].gameObject.SetActive(false);
        }
    }

    private void SetUINoSave()
    {
        saveDataDetails.text = "No Progress Found";
        saveDetailsFloor.text = "N/A";
        saveDetailsGold.text = "N/A";
        for (int i = 0; i < creatureUIs.Count; i++)
        {
            creatureUIs[i].gameObject.SetActive(false);
        }
    }

    public void EmptyCurrentProgress() { }

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
}
