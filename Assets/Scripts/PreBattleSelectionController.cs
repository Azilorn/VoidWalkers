using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BattleType { Normal, Elite, Boss }
public class PreBattleSelectionController : MonoBehaviour
{
    public BattleType currentBattleType = BattleType.Normal;
    public static PreBattleSelectionController Instance;

    [SerializeField] private GameDetails gameDetails;
    [SerializeField] private PreBattleSelectionUI ui;


    public List<PlayerParty> enemies = new List<PlayerParty>();
    public List<PlayerParty> bosses = new List<PlayerParty>();
    public List<int> selectionInts = new List<int>();
    public List<int> bossInts = new List<int>();
    public int eventInt;
    public List<int> completedEvents = new List<int>();


    public int selectedInt;
    public GameDetails GameDetails { get => gameDetails; set => gameDetails = value; }
    public PreBattleSelectionUI UI { get => ui; set => ui = value; }

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
            bossInts = new List<int>();
            SaveLoadManager.Instance.SetLoadEvents();
            SaveLoadManager.Load();
            SaveLoadManager.Instance.SetSaveEvents();
            SetFloorText();
            UI.SetGoldText(GameDetails.Gold.ToString());
            SetWorldUIAfterLoad();
            if (CoreGameInformation.isRetry)
            {
                CoreGameInformation.AddToRetries();
                Debug.Log("AddToRetries");
                CoreGameInformation.isRetry = false;
            }
        }
        else if (!CoreGameInformation.isLoadedGame)
        {
            for (int i = 0; i < PartyBetweenScenes.party.party.Length; i++)
            {
                BattleController.Instance.MasterPlayerParty.party[i] = PartyBetweenScenes.party.party[i];
            }
            CoreGameInformation.SetGameIsNew();
            SetFloorText();
            UI.SetGoldText(GameDetails.Gold.ToString());
            SaveLoadManager.Instance.SetSaveEvents();
            SetOptions();
            bossInts = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                bossInts.Add(Random.Range(0, bosses.Count));
            }
            var sortedResults = from r in bossInts orderby Guid.NewGuid() ascending select r;
            bossInts = sortedResults.ToList();
            ShopUI.shopSaveData.Clear();

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
        selectionInts.Clear();
        if (GameDetails.ProgressOnCurrentFloor == 10)
        {
            selectionInts.Add(2004);
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
                        if (i != 0 && selectionInts[i - 1] == 2001)
                        {
                            i--;
                           
                        }
                        selectionInts.Add(2001);
                    }
                    else if (rand >= 25 && rand < 50)
                    {
                        if (i != 0 && selectionInts[i - 1] == 2002)
                        {
                            i--;
                       
                        }
                        selectionInts.Add(2002);
                    }
                    else if (rand >= 50 && rand <= 75)
                    {
                        if (i != 0 && selectionInts[i - 1] == 2003)
                        {
                            i--;
                      
                        }
                        selectionInts.Add(2003);
                    }
                    else if (rand >= 75 && rand <= 100)
                    {
                        int rnd = Random.Range(0, WorldMenuEventsUI.Instance.events.Count);
                        if (!completedEvents.Contains(rnd))
                        {
                            eventInt = rnd;
                            selectionInts.Add(2005);
                        }
                        else
                        {
                            i--;
                        }
                    }
                }
                else
                {
                    selectionInts.Add(ReturnEnemyIndex(enemies[UnityEngine.Random.Range(0, enemies.Count)]));
                }
            }
        }
        selectionInts = selectionInts.Distinct().ToList();
    }
    private int ReturnEnemyIndex(PlayerParty playerParty)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == playerParty)
            {
                return i;
            }
        }
        return 0;
    }
    public void SetSelectedUI()
    {
        UI.SetAllUIInactive();
        int nonEnemyCount = 0;
        for (int i = 0; i < selectionInts.Count; i++)
        {
            if (selectionInts[i] <= enemies.Count)
            {
                UI.previewUI.AddOptionSelectionUI(i);

                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                if (i - nonEnemyCount >= UI.selectionUI.Count)
                {

                    GameObject go = Instantiate(UI.selectionUI[0].gameObject, UI.selectionUI[0].transform.parent) as GameObject;
                    UI.selectionUI.Add(go);
                }
                UI.selectionUI[i - nonEnemyCount].GetComponent<EnemySelectUI>().SetPartyCountIcons(enemies[selectionInts[i]]);
                UI.selectionUI[i - nonEnemyCount].transform.SetSiblingIndex(i);
                UI.selectionUI[i - nonEnemyCount].gameObject.SetActive(true);
            }
            //Tavern
            else if (selectionInts[i] == 2001)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.tavernSelctionUI.transform.SetSiblingIndex(i);
                UI.tavernSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            //Reward
            else if (selectionInts[i] == 2002)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.rewardSelctionUI.transform.SetSiblingIndex(i);
                UI.rewardSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            //Shop
            else if (selectionInts[i] == 2003)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.shopSelctionUI.transform.SetSiblingIndex(i);
                UI.shopSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            //Boss
            else if (selectionInts[i] == 2004)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.bossSelctionUI.transform.SetSiblingIndex(i);
                UI.bossSelctionUI.SetActive(true);
                nonEnemyCount++;
            }
            else if (selectionInts[i] == 2005)
            {
                UI.previewUI.AddOptionSelectionUI(i);
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.eventSelectionUI.transform.SetSiblingIndex(i);
                UI.eventSelectionUI.SetActive(true);
                nonEnemyCount++;
            }
        }
    }
    public void StartBattle(int battleType)
    {
        //BattleType {Normal = 0, Elite = 1, Boss = 2 }
        currentBattleType = (BattleType)battleType;
        if (BattleController.Instance.TurnController.EnemyParty != null)
        {
            Destroy(BattleController.Instance.TurnController.EnemyParty.gameObject);
        }
        if (currentBattleType == BattleType.Normal)
        {
            GameObject partyGO = Instantiate(enemies[selectionInts[selectedInt]].gameObject) as GameObject;
            PlayerParty party = partyGO.GetComponent<PlayerParty>();
            party.SetAveragelevelAcrossParty(((GameDetails.Floor - 1) * 10 + GameDetails.ProgressOnCurrentFloor));
            BattleController.Instance.SetupBattle(party);
        }
        //AddElites
        else if (currentBattleType == BattleType.Elite)
        {

        }
        else if (currentBattleType == BattleType.Boss)
        {
            GameObject partyGO1 = Instantiate(bosses[bossInts[GameDetails.Floor - 1]].gameObject) as GameObject;
            PlayerParty party1 = partyGO1.GetComponent<PlayerParty>();
            party1.SetAveragelevelAcrossParty(10 * GameDetails.Floor);
            BattleController.Instance.SetupBattle(party1);
        }
        AudioManager.Instance.activeBackgroundMusic = StartCoroutine(AudioManager.Instance.PlayMusicWithMultipleParts(BattleAudio.Instance.BattleMusic[0].AudioList, 0.6f));
    }
    public void SetPostFloorOptionDetails()
    {
        UI.swipe.Content.anchoredPosition = Vector3.zero;
        if (GameDetails.Floor > 10)
        {
            GameDetails.Floor += 1;
            GameDetails.ProgressOnCurrentFloor = 1;

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
    public void SetPostFloorOptionDetails(int floor, int battle)
    {
        UI.swipe.Content.anchoredPosition = Vector3.zero;
        if (battle > 10)
        {
            GameDetails.Floor = floor + 1;
            GameDetails.ProgressOnCurrentFloor = 1;
            CoreGameInformation.currentRunDetails.BossesDefeated++;
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