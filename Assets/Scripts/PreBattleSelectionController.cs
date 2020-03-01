using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ReturnXPBattleType { Normal, Elite, Boss }
public class PreBattleSelectionController : MonoBehaviour
{
    public ReturnXPBattleType currentBattleType = ReturnXPBattleType.Normal;
    public static PreBattleSelectionController Instance;

    [SerializeField] private GameDetails gameDetails;
    [SerializeField] private PreBattleSelectionUI ui;


    public List<PlayerParty> enemies = new List<PlayerParty>();
    public List<int> battleSelectionInts = new List<int>();
    public int eventInt;
    public List<int> completedEvents = new List<int>();


    public int selectedInt;
    public GameDetails GameDetails { get => gameDetails; set => gameDetails = value; }
    public PreBattleSelectionUI UI { get => ui; set => ui = value; }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) {
            SetFloor();
        }
    }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GameDetails = new GameDetails();
    }
    public void Start()
    {
        if (CoreGameInformation.isLoadedGame)
        {
            Debug.Log("isLoaded");
            SaveLoadManager.Instance.SetLoadEvents();
            SaveLoadManager.Load();
            SetFloorText();
            UI.SetGoldText(GameDetails.Gold.ToString());
            SetWorldUIAfterLoad();
            if (CoreGameInformation.isRetry)
            {
                CoreGameInformation.AddToRetries();
                Debug.Log("AddToRetries");
                CoreGameInformation.isRetry = false;
            }
            SaveLoadManager.Instance.SetSaveEvents();
        }
        else if (!CoreGameInformation.isLoadedGame)
        {
            BattleController.Instance.MasterPlayerParty.party = new PlayerCreatureStats[PartyBetweenScenes.party.party.Length];
            for (int i = 0; i < PartyBetweenScenes.party.party.Length; i++)
            {
                BattleController.Instance.MasterPlayerParty.party[i] = PartyBetweenScenes.party.party[i];
            }
            CoreGameInformation.totalLevelUps = 0;
            CoreGameInformation.SetGameIsNew();
            SetFloorText();
            UI.SetGoldText(GameDetails.Gold.ToString());
            SaveLoadManager.Instance.SetSaveEvents();
            SetOptions();
            ShopUI.shopSaveData.Clear();

            InventoryController.Instance.AddRelic(PartyBetweenScenes.startingRelic);
            SaveLoadManager.Save();
        }
    }
    public void SetOptions()
    {
        SetInts();
        SetSelectedUI();
    }
    public void SetInts()
    {
        selectedInt = 0;
        battleSelectionInts.Clear();
        if (GameDetails.ProgressOnCurrentFloor == 10)
        {
            battleSelectionInts.Add(ReturnEnemyIndex(PartyType.Boss));
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                int rand = UnityEngine.Random.Range(0, 100);

                if (GameDetails.ProgressOnCurrentFloor % 3 == 0)
                {
                    if (rand >= 0 && rand < 25)
                    {
                        if (i != 0 && battleSelectionInts[i - 1] == 2001)
                        {
                            i--;
                           
                        }
                        battleSelectionInts.Add(2001);
                    }
                    else if (rand >= 25 && rand < 50)
                    {
                        if (i != 0 && battleSelectionInts[i - 1] == 2002)
                        {
                            i--;
                       
                        }
                        battleSelectionInts.Add(2002);
                    }
                    else if (rand >= 50 && rand <= 75)
                    {
                        if (i != 0 && battleSelectionInts[i - 1] == 2003)
                        {
                            i--;
                      
                        }
                        battleSelectionInts.Add(2003);
                    }
                    else if (rand >= 75 && rand <= 100)
                    {
                        int rnd = Random.Range(0, WorldMenuEventsUI.Instance.events.Count);
                        if (!completedEvents.Contains(rnd))
                        {
                            eventInt = rnd;
                            battleSelectionInts.Add(2005);
                        }
                        else
                        {
                            i--;
                        }
                    }
                }
                else
                {
                    int rnd2 = Random.Range(0, 100);

                    if(GameDetails.ProgressOnCurrentFloor == 1)
                    {
                        battleSelectionInts.Add(ReturnEnemyIndex(PartyType.Battle));
                    }
                    else  if (rnd2 > 20)
                        battleSelectionInts.Add(ReturnEnemyIndex(PartyType.Battle));
                    else
                    {
                        battleSelectionInts.Add(ReturnEnemyIndex(PartyType.Elite));
                    }
                }
            }
        }
        battleSelectionInts = battleSelectionInts.Distinct().ToList();
    }
    private int ReturnEnemyIndex(PartyType partyType)
    {
        int rnd = Random.Range(0, enemies.Count);
        for (int i = rnd; i < enemies.Count; i++)
        {
            if ((int)enemies[i].floorAvailable <= GameDetails.Floor)
            {
                if (enemies[i].partyType == partyType)
                    return i;
            }
            if (i == enemies.Count - 1) {
                i = 0;
            }
        }
        return 0;
    }
    public void SetSelectedUI()
    {
        UI.SetAllUIInactive();
        int nonEnemyCount = 0;
        for (int i = 0; i < battleSelectionInts.Count; i++)
        {
            if (battleSelectionInts[i] <= enemies.Count)
            {
                UI.previewUI.AddOptionSelectionUI(i);

                UI.SetOptionPreviewSprites(i, battleSelectionInts[i]);
                if (i - nonEnemyCount >= UI.battleSelectionUI.Count)
                {

                    GameObject go = Instantiate(UI.battleSelectionUI[0].gameObject, UI.battleSelectionUI[0].transform.parent) as GameObject;
                    UI.battleSelectionUI.Add(go);
                }
                UI.battleSelectionUI[i - nonEnemyCount].GetComponent<EnemySelectUI>().SetPartyCountIcons(enemies[battleSelectionInts[i]]);
                UI.battleSelectionUI[i - nonEnemyCount].transform.SetSiblingIndex(i);
                UI.battleSelectionUI[i - nonEnemyCount].gameObject.SetActive(true);
            }
            //Tavern
            else if (battleSelectionInts[i] == 2001)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, battleSelectionInts[i]);
                UI.tavernSelctionUI.transform.SetSiblingIndex(i);
                UI.tavernSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            //Reward
            else if (battleSelectionInts[i] == 2002)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, battleSelectionInts[i]);
                UI.rewardSelctionUI.transform.SetSiblingIndex(i);
                UI.rewardSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            //Shop
            else if (battleSelectionInts[i] == 2003)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, battleSelectionInts[i]);
                UI.shopSelctionUI.transform.SetSiblingIndex(i);
                UI.shopSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            else if (battleSelectionInts[i] == 2005)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, battleSelectionInts[i]);
                UI.eventSelectionUI.transform.SetSiblingIndex(i);
                UI.eventSelectionUI.SetActive(true);
                nonEnemyCount++;
            }
        }
    }
    public void StartBattle(int battleType)
    {
        //BattleType {Normal = 0, Elite = 1, Boss = 2 }
        currentBattleType = (ReturnXPBattleType)battleType;
        if (BattleController.Instance.EnemyParty != null)
        {
            Destroy(BattleController.Instance.EnemyParty.gameObject);
        }
        if (currentBattleType == ReturnXPBattleType.Normal)
        {
            GameObject partyGO = Instantiate(enemies[battleSelectionInts[selectedInt]].gameObject) as GameObject;
            PlayerParty party = partyGO.GetComponent<PlayerParty>();
            party.SetAveragelevelAcrossParty(((GameDetails.Floor - 1) * 10 + GameDetails.ProgressOnCurrentFloor));
            BattleController.Instance.SetupBattle(party);
        }
        //AddElites
        else if (currentBattleType == ReturnXPBattleType.Elite)
        {
            GameObject partyGO = Instantiate(enemies[battleSelectionInts[selectedInt]].gameObject) as GameObject;
            PlayerParty party = partyGO.GetComponent<PlayerParty>();
            party.SetAveragelevelAcrossParty(((GameDetails.Floor - 1) * 10 + GameDetails.ProgressOnCurrentFloor));
            BattleController.Instance.SetupBattle(party);
        }
        else if (currentBattleType == ReturnXPBattleType.Boss)
        {
            GameObject partyGO1 = Instantiate(enemies[battleSelectionInts[selectedInt]].gameObject) as GameObject;
            PlayerParty party1 = partyGO1.GetComponent<PlayerParty>();
            party1.SetAveragelevelAcrossParty(10 * GameDetails.Floor);
            BattleController.Instance.SetupBattle(party1);
        }
        AudioManager.Instance.activeBackgroundMusic = StartCoroutine(AudioManager.Instance.PlayMusicWithMultipleParts(BattleAudio.Instance.BattleMusic[0].AudioList, 0.6f));
    }
    public void SetFloor()
    {
        UI.swipe.Content.anchoredPosition = Vector3.zero;
        if (GameDetails.ProgressOnCurrentFloor > 10)
        {
            GameDetails.Floor += 1;
            GameDetails.ProgressOnCurrentFloor = 1;
            CoreGameInformation.currentRunDetails.BossesDefeated++;
            BattleController.Instance.MasterPlayerParty.RestorePartyHPandAP();
        }
        else
        {
            GameDetails.ProgressOnCurrentFloor += 1;
        }
        SetFloorText();
        SetOptions();
        UI.SetGoldText(GameDetails.Gold.ToString());
        CoreGameInformation.currentRunDetails.RoutesTaken++;
        ShopUI.shopSaveData.Clear();
        SaveLoadManager.Save();
    }
    public void SetFloor(int floor, int battle)
    {
        UI.swipe.Content.anchoredPosition = Vector3.zero;
        if (battle > 10)
        {
            if (floor >= 4)
            {
                CoreUI.DoFadeIn(CoreUI.Instance.RunWinScreen, 0.35f, 0.70f);
                return;
            }
            else
            {
                GameDetails.Floor = floor + 1;
                GameDetails.ProgressOnCurrentFloor = 1;
                CoreGameInformation.currentRunDetails.BossesDefeated++;
                BattleController.Instance.MasterPlayerParty.RestorePartyHPandAP();
            }
        }
        else
        {
            GameDetails.Floor = floor;
            GameDetails.ProgressOnCurrentFloor = battle;
        }
        SetFloorText();
        SetOptions();
        UI.SetGoldText(GameDetails.Gold.ToString());
        CoreGameInformation.currentRunDetails.RoutesTaken++;
        ShopUI.shopSaveData.Clear();
        WorldRewardMenuUI.Rewards.Clear();
        SaveLoadManager.Save();
    }
    public void SetFloorText()
    {
        UI.FloorText.text = GameDetails.Floor + "-" + GameDetails.ProgressOnCurrentFloor;
    }

    public void SetWorldUIAfterLoad()
    {
        SetFloorText();
        UI.SetGoldText(GameDetails.Gold.ToString());
        SetSelectedUI();
    }
}
[Serializable]
public class GameDetails
{
    public int ProgressOnCurrentFloor;
    public int Floor;
    [SerializeField] private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            int previousGold = gold;
            gold = value;
            if (previousGold != gold)
            {
                PreBattleSelectionController.Instance.UI.GoldText.text = gold.ToString();
            }
        }
    }
}