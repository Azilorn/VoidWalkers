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
    public static string currentSaveVersion;
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
        {
            SaveDelegateEvent = null;
            SetSaveEvents();
        }
        if (LoadDelegateEvent != null)
        {
            LoadDelegateEvent = null;
            SetLoadEvents();
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public static void DeleteSave() {
        if (File.Exists(Application.persistentDataPath + "/saveData.sav"))
        {
            File.Delete(Application.persistentDataPath + "/saveData.sav");
        }
        else Debug.Log("file does not exist");
    }

    public static void DeleteGlobalData()
    {
        if (File.Exists(Application.persistentDataPath + "/globalSaveData.sav"))
        {
            File.Delete(Application.persistentDataPath + "/globalSaveData.sav");
        }
        else Debug.Log("file does not exist");
    }

    public bool SaveDataExists()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.sav"))
        {
            return true;
        }
        else return false;
    }

    public bool GlobalDataExists()
    {
        if (File.Exists(Application.persistentDataPath + "/globalSaveData.sav"))
        {
            return true;
        }
        else return false;
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
        Debug.Log(SaveDelegateEvent);   
        SaveDelegateEvent();

    }
    public static void Load()
    {
        LoadDelegateEvent();
    }

    public void LoadGlobalSaveData() {

        if (File.Exists(Application.persistentDataPath + globalSaveDataLocation))
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream stream1 = new FileStream(Application.persistentDataPath + globalSaveDataLocation, FileMode.Open);

            globalSaveData = new GlobalSaveData();
            globalSaveData = bf1.Deserialize(stream1) as GlobalSaveData;
            stream1.Close();


            CreatureTable.Instance.UnlockedCreature = globalSaveData.unlockedCreature;
            CoreGameInformation.currentXPEarned = globalSaveData.XpEarned;
            CoreGameInformation.currentLVL = globalSaveData.lvl;

            AudioManager.Instance.UiSFXSource.volume = globalSaveData.SFXAudio;
            AudioManager.Instance.SFXSource.volume = globalSaveData.SFXAudio;
            AudioManager.Instance.BattleMusic.volume = globalSaveData.BGMAudio;
            AudioManager.Instance.MusicSource.volume = globalSaveData.BGMAudio;
            AudioManager.Instance.MusicSource2.volume = globalSaveData.BGMAudio;
        }
        else {

            CoreGameInformation.currentXPEarned = 1000;
            CoreGameInformation.currentLVL = 1;
        }
    }
    public void SaveGlobalSaveData()
    {
        finishedSaving = false;
        StartCoroutine(ShowSavingLogo());

        globalSaveData = new GlobalSaveData();
        globalSaveData.unlockedCreature = new List<bool>();
        globalSaveData.unlockedCreature = CreatureTable.Instance.UnlockedCreature;
        globalSaveData.lvl = CoreGameInformation.currentLVL;
        globalSaveData.XpEarned = CoreGameInformation.currentXPEarned;


        globalSaveData.SFXAudio = AudioManager.Instance.UiSFXSource.volume;
        globalSaveData.SFXAudio = AudioManager.Instance.SFXSource.volume;
        globalSaveData.BGMAudio = AudioManager.Instance.BattleMusic.volume;
        globalSaveData.BGMAudio = AudioManager.Instance.MusicSource.volume;
        globalSaveData.BGMAudio = AudioManager.Instance.MusicSource2.volume;


        BinaryFormatter bf1 = new BinaryFormatter();
        FileStream stream1 = new FileStream(Application.persistentDataPath + globalSaveDataLocation, FileMode.Create);
        bf1.Serialize(stream1, globalSaveData);
        stream1.Close();

        finishedSaving = true;
    }

    public void SaveDataEvent()
    {
        finishedSaving = false;
        //StartCoroutine(ShowSavingLogo());

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + gameSaveDataLocation, FileMode.Create);
        bf.Serialize(stream, saveData);
        stream.Close();

        BinaryFormatter bf1 = new BinaryFormatter();
        FileStream stream1 = new FileStream(Application.persistentDataPath + globalSaveDataLocation, FileMode.Create);
        bf1.Serialize(stream1, globalSaveData);
        stream1.Close();

        finishedSaving = true;

    }
    public IEnumerator ShowSavingLogo() {
        SavingLogo.SetActive(true);
        while (!finishedSaving)
            yield return null;
        yield return new WaitForSeconds(0.1f);
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

        globalSaveData = new GlobalSaveData();
        globalSaveData.unlockedCreature = new List<bool>();
        globalSaveData.unlockedCreature = creatureTable.UnlockedCreature;
        globalSaveData.lvl = CoreGameInformation.currentLVL;
        globalSaveData.XpEarned = CoreGameInformation.currentXPEarned;


        globalSaveData.SFXAudio = AudioManager.Instance.UiSFXSource.volume;
        globalSaveData.SFXAudio = AudioManager.Instance.SFXSource.volume;
        globalSaveData.BGMAudio = AudioManager.Instance.BattleMusic.volume;
        globalSaveData.BGMAudio = AudioManager.Instance.MusicSource.volume;
        globalSaveData.BGMAudio = AudioManager.Instance.MusicSource2.volume;

        saveData = new GameSaveDetailsData();
        saveData.currentVersion = Application.version;
        saveData.SaveSeed = CoreGameInformation.currentSavedSeed;
        saveData.currentShopItems = ShopUI.shopSaveData;

        saveData.ProgressOnCurrentFloor = PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor;
        saveData.Floor = PreBattleSelectionController.Instance.GameDetails.Floor;
        saveData.Gold = PreBattleSelectionController.Instance.GameDetails.Gold;
        saveData.selectionInts = PreBattleSelectionController.Instance.battleSelectionInts;
        saveData.eventInt = PreBattleSelectionController.Instance.eventInt;

        BattleController.Instance.MasterPlayerParty.SetPartySaveData();
        saveData.playerParty = BattleController.Instance.MasterPlayerParty.partySaveData;
        saveData.ownedItems = InventoryController.Instance.GetDataToSaveForItems();
        saveData.ownedRelic = InventoryController.Instance.GetDataToSaveForRelics();
        saveData.ownedAbilities = InventoryController.Instance.GetDataToSaveForAbilities();
        saveData.rewardOptions = WorldRewardMenuUI.Rewards;
        saveData.currentProgressDateTime = DateTime.Now;
        saveData.CurrentRunDetails = CoreGameInformation.currentRunDetails;
    }
    public void SetLoadDataEvent()
    {
        CreatureTable creatureTable = CreatureTable.Instance;

        creatureTable.UnlockedCreature = globalSaveData.unlockedCreature;
        CoreGameInformation.currentXPEarned = globalSaveData.XpEarned;
        CoreGameInformation.currentLVL = globalSaveData.lvl;

        AudioManager.Instance.UiSFXSource.volume = globalSaveData.SFXAudio;
        AudioManager.Instance.SFXSource.volume = globalSaveData.SFXAudio;
        AudioManager.Instance.BattleMusic.volume = globalSaveData.BGMAudio;
        AudioManager.Instance.MusicSource.volume = globalSaveData.BGMAudio;
        AudioManager.Instance.MusicSource2.volume = globalSaveData.BGMAudio;

        CoreGameInformation.currentSavedSeed = saveData.SaveSeed;
        if(saveData.currentShopItems != null)
            ShopUI.shopSaveData = saveData.currentShopItems;
        currentSaveVersion = saveData.currentVersion;

        PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor = saveData.ProgressOnCurrentFloor;
        PreBattleSelectionController.Instance.GameDetails.Floor = saveData.Floor;
        PreBattleSelectionController.Instance.GameDetails.Gold = saveData.Gold;
        PreBattleSelectionController.Instance.battleSelectionInts = saveData.selectionInts;
        PreBattleSelectionController.Instance.eventInt = saveData.eventInt;

        BattleController.Instance.MasterPlayerParty.partySaveData = new PlayerCreatureStatsSaveData[saveData.playerParty.Length];
        BattleController.Instance.MasterPlayerParty.partySaveData = saveData.playerParty;
        BattleController.Instance.MasterPlayerParty.SetPartyFromLoad(BattleController.Instance.MasterPlayerParty.partySaveData.Length);

        InventoryController.Instance.GetDataTLoadForItems(saveData.ownedItems);
        InventoryController.Instance.GetDataToLoadForRelics(saveData.ownedRelic);
        InventoryController.Instance.GetDataToLoadForAbilities(saveData.ownedAbilities);
        WorldRewardMenuUI.Rewards = saveData.rewardOptions;
        CoreGameInformation.currentRunDetails = saveData.CurrentRunDetails;

    }
    public static void SaveRetryData() {

        CurrentRunDetails temp = CoreGameInformation.currentRunDetails;
        Load();
        CoreGameInformation.currentRunDetails = temp;
        Save();
    }
}
[Serializable]
public class GlobalSaveData
{
    public int lvl;
    public int XpEarned;
    public List<bool> unlockedCreature;

    public float SFXAudio;
    public float BGMAudio;

}
[Serializable]
public class GameSaveDetailsData
{
    public string currentVersion;
    public DateTime currentProgressDateTime;
    public int SaveSeed;
    public int ProgressOnCurrentFloor;
    public int Floor;
    public int Gold;
    public int currentSelectedint;
    public List<int> selectionInts = new List<int>();
    public List<int> bossInts = new List<int>();
    public int eventInt;
    public PlayerCreatureStatsSaveData[] playerParty;
    public Dictionary<int, int> ownedItems;
    public Dictionary<int, bool> ownedRelic;
    public Dictionary<int, int> ownedAbilities;
    public Dictionary<string, int> rewardOptions;
    public CurrentRunDetails CurrentRunDetails;
    public List<ShopSaveData> currentShopItems;
}
