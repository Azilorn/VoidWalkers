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
    public GlobalSaveData globalSaveData;
    public static string gameSaveDataLocation = "/saveData.sav";
    public static string globalSaveDataLocation = "/globalSaveData.sav";

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
        FileStream stream = new FileStream(Application.persistentDataPath + gameSaveDataLocation, FileMode.Create);
        bf.Serialize(stream, saveData);
        stream.Close();

        BinaryFormatter bf1 = new BinaryFormatter();
        FileStream stream1 = new FileStream(Application.persistentDataPath + globalSaveData, FileMode.Create);
        bf1.Serialize(stream1, globalSaveData);
        stream1.Close();

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
        if (File.Exists(Application.persistentDataPath + gameSaveDataLocation)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + gameSaveDataLocation, FileMode.Open);

            saveData = new GameSaveDetailsData();
            saveData = bf.Deserialize(stream) as GameSaveDetailsData;
            stream.Close();
        }
        if (File.Exists(Application.persistentDataPath + globalSaveDataLocation)) {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream stream1 = new FileStream(Application.persistentDataPath + globalSaveDataLocation, FileMode.Open);

            globalSaveData = new GlobalSaveData();
            globalSaveData = bf1.Deserialize(stream1) as GlobalSaveData;
            stream1.Close();
        }
    }
    public static GameSaveDetailsData LoadDataEventMenu()
    {
        if (File.Exists(Application.persistentDataPath + gameSaveDataLocation))
        {
            GameSaveDetailsData sd = new GameSaveDetailsData();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + gameSaveDataLocation, FileMode.Open);
            sd = bf.Deserialize(stream) as GameSaveDetailsData;
            stream.Close();
            return sd;
        }
        return null;
    }
    public void SetSaveDataEvent()
    {
        GameObject go = Resources.Load("CreatureTable") as GameObject;
        CreatureTable creatureTable = go.GetComponent<CreatureTable>();
        globalSaveData.unlockedCreature = new List<bool>();
        globalSaveData.unlockedCreature = creatureTable.UnlockedCreature;

        saveData = new GameSaveDetailsData();
        saveData.SaveSeed = CoreGameInformation.currentSavedSeed;
        saveData.currentShopItems = ShopUI.shopSaveData;
        saveData.ProgressOnCurrentFloor = PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor;
        saveData.Floor = PreBattleSelectionController.Instance.GameDetails.Floor;
        saveData.Gold = PreBattleSelectionController.Instance.GameDetails.Gold;
        saveData.selectionInts = PreBattleSelectionController.Instance.selectionInts;
        saveData.bossInts = PreBattleSelectionController.Instance.bossInts;
        BattleController.Instance.MasterPlayerParty.SetPartySaveData();
        saveData.playerParty = BattleController.Instance.MasterPlayerParty.partySaveData;
        saveData.ownedItems = InventoryController.Instance.GetDataToSaveForItems();
        saveData.ownedRelic = InventoryController.Instance.GetDataToSaveForRelics();
        saveData.ownedAbilities = InventoryController.Instance.GetDataToSaveForAbilities();
        saveData.currentProgressDateTime = DateTime.Now;
    }
    public void SetLoadDataEvent()
    {
        CreatureTable creatureTable = CreatureTable.Instance;
        creatureTable.UnlockedCreature = globalSaveData.unlockedCreature;
        CoreGameInformation.currentXPEarned = globalSaveData.XpEarned;
        CoreGameInformation.currentLVL = globalSaveData.lvl;

        CoreGameInformation.currentSavedSeed = saveData.SaveSeed;
        if(saveData.currentShopItems != null)
            ShopUI.shopSaveData = saveData.currentShopItems;
        PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor = saveData.ProgressOnCurrentFloor;
        PreBattleSelectionController.Instance.GameDetails.Floor = saveData.Floor;
        PreBattleSelectionController.Instance.GameDetails.Gold = saveData.Gold;
        PreBattleSelectionController.Instance.selectionInts = saveData.selectionInts;
        PreBattleSelectionController.Instance.bossInts = saveData.bossInts;
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
public class GlobalSaveData
{
    public int lvl;
    public int XpEarned;
    public List<bool> unlockedCreature;

}
[Serializable]
public class GameSaveDetailsData
{
    public DateTime currentProgressDateTime;
    public int SaveSeed;
    public int ProgressOnCurrentFloor;
    public int Floor;
    public int Gold;
    public int currentSelectedint;
    public List<int> selectionInts = new List<int>();
    public List<int> bossInts = new List<int>();
    public PlayerCreatureStatsSaveData[] playerParty;
    public Dictionary<int, int> ownedItems;
    public Dictionary<int, bool> ownedRelic;
    public Dictionary<int, int> ownedAbilities;
    public List<ShopSaveData> currentShopItems;
}
