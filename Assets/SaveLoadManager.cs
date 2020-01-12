using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    public static bool finishedSaving;
    //public GameObject SavingLogo;

    public delegate void SaveDelegate();
    public delegate void LoadDelegate();
    public static SaveDelegate SaveDelegateEvent;
    public static LoadDelegate LoadDelegateEvent;

    public GameObject SavingLogo;
    public GameSaveDetailsData saveData;
    public string saveLocation = "/savedata.sav";

    private void Awake()
    {
        if (SaveDelegateEvent != null)
            SaveDelegateEvent = null;
        if (LoadDelegateEvent != null)
            LoadDelegateEvent = null;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
 
    }
    public void SetSaveEvents()
    {
        SaveDelegateEvent += SetSaveDataEvent;
        SaveDelegateEvent += SaveDataEvent;
        
    }
    public void SetLoadEvents()
    {
        LoadDelegateEvent += LoadDataEvent;
        LoadDelegateEvent += SetLoadDataEvent;
    }
    public static void Save()
    {
        SaveDelegateEvent();
    }
    public static void Load()
    {
        LoadDelegateEvent();
    }
    public void SaveDataEvent()
    {
        finishedSaving = false;
        StartCoroutine(ShowSavingLogo());
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + saveLocation, FileMode.Create);
        bf.Serialize(stream, saveData);
        stream.Close();
        finishedSaving = true;
        Debug.Log("Saved");
    }
    public IEnumerator ShowSavingLogo() {
        SavingLogo.SetActive(true);
        while (!finishedSaving)
            yield return null;
        SavingLogo.SetActive(false);
    }
    public void LoadDataEvent()
    {
        if (File.Exists(Application.persistentDataPath + saveLocation)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + saveLocation, FileMode.Open);

            saveData = new GameSaveDetailsData();
            saveData = bf.Deserialize(stream) as GameSaveDetailsData;
            stream.Close();
        }
    }
    public void SetSaveDataEvent()
    {
        saveData = new GameSaveDetailsData();
        saveData.ProgressOnCurrentFloor = PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor;
        saveData.Floor = PreBattleSelectionController.Instance.GameDetails.Floor;
        saveData.Gold = PreBattleSelectionController.Instance.GameDetails.Gold;
        saveData.currentSelectedint = PreBattleSelectionController.Instance.selectedInt;
        saveData.selectionInts = PreBattleSelectionController.Instance.selectionInts;
        BattleController.Instance.MasterPlayerParty.SetPartySaveData();
        saveData.playerParty = BattleController.Instance.MasterPlayerParty.partySaveData;
        saveData.ownedItems = InventoryController.Instance.GetDataToSaveForItems();
        saveData.ownedRelic = InventoryController.Instance.GetDataToSaveForRelics();
        saveData.ownedAbilities = InventoryController.Instance.GetDataToSaveForAbilities();
    }
    public void SetLoadDataEvent()
    {
        PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor = saveData.ProgressOnCurrentFloor;
        PreBattleSelectionController.Instance.GameDetails.Floor = saveData.Floor;
        PreBattleSelectionController.Instance.GameDetails.Gold = saveData.Gold;
        PreBattleSelectionController.Instance.selectedInt = saveData.currentSelectedint;
        PreBattleSelectionController.Instance.selectionInts = saveData.selectionInts;
        BattleController.Instance.MasterPlayerParty.partySaveData = new PlayerCreatureStatsSaveData[saveData.playerParty.Length];
        BattleController.Instance.MasterPlayerParty.partySaveData = saveData.playerParty;
        BattleController.Instance.MasterPlayerParty.SetPartyFromLoad();
        InventoryController.Instance.GetDataTLoadForItems(saveData.ownedItems);
        InventoryController.Instance.GetDataToLoadForRelics(saveData.ownedRelic);
        InventoryController.Instance.GetDataToLoadForAbilities(saveData.ownedAbilities);
        Debug.Log("Loaded");
    }
}
[Serializable]
public class GameSaveDetailsData
{
    public int ProgressOnCurrentFloor;
    public int Floor;
    public int Gold;
    public int currentSelectedint;
    public List<int> selectionInts = new List<int>();
    public PlayerCreatureStatsSaveData[] playerParty;
    public Dictionary<int, int> ownedItems;
    public Dictionary<int, bool> ownedRelic;
    public Dictionary<int, int> ownedAbilities;
}
