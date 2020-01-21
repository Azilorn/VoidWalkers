using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CurrentProgressMenuUI : MonoBehaviour
{

    public GameObject contents;
    public List<CurrentProgressCreatureUI> creatureUIs = new List<CurrentProgressCreatureUI>();
    public TextMeshProUGUI saveDataDetails;
    public TextMeshProUGUI saveDetailsFloor;
    public TextMeshProUGUI saveDetailsGold;

    public void OnEnable()
    {
        SetCurrentProgress();
    }
    public void OnDisable()
    {
        for (int i = 0; i < creatureUIs.Count; i++)
        {
            creatureUIs[i].ClearUI();
        }
    }
    public void SetCurrentProgress()
    {
        GameSaveDetailsData saveData = new GameSaveDetailsData();

        saveData =  SaveLoadManager.LoadDataEventMenu();

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
    public void EmptyCurrentProgress() { }
}
