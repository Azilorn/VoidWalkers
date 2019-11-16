using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PreBattleSelectionController : MonoBehaviour
{

    public static PreBattleSelectionController Instance;

    [SerializeField] private GameDetails gameDetails;
    [SerializeField] private PreBattleSelectionUI ui;

    public List<PlayerParty> enemies = new List<PlayerParty>();
    public List<int> selectionInts = new List<int>();

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
            CoreGameInformation.SetGameIsLoaded();
        }
        else if (!CoreGameInformation.isLoadedGame)
        {
            CoreGameInformation.SetGameIsNew();
            SetFloorText();
            UI.SetGoldText(GameDetails.Gold.ToString());
        }
        SetOptions();

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
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    selectionInts.Add(ReturnEnemyIndex(enemies[UnityEngine.Random.Range(0, enemies.Count)]));
                }
                else
                {
                    int rand = UnityEngine.Random.Range(0, 100);
                    if (rand < 70)
                    {
                        selectionInts.Add(ReturnEnemyIndex(enemies[UnityEngine.Random.Range(0, enemies.Count)]));
                    }
                    else
                    {
                        if (rand >= 70 && rand < 80)
                        {
                            selectionInts.Add(2001);
                        }
                        else if (rand >= 80 && rand < 90)
                        {
                            selectionInts.Add(2002);
                        }
                        else if (rand >= 90 && rand <= 100)
                        {
                            selectionInts.Add(2003);
                        }
                    }
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
        for (int i = 0; i < selectionInts.Count; i++)
        {
            if (selectionInts[i] <= enemies.Count)
            {
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.selectionUI[i].GetComponent<EnemySelectUI>().SetPartyCountIcons(enemies[selectionInts[i]]);
                UI.selectionUI[i].transform.SetSiblingIndex(i);
                UI.selectionUI[i].gameObject.SetActive(true);
            }
            //Tavern
            else if (selectionInts[i] == 2001)
            {
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.tavernSelctionUI.transform.SetSiblingIndex(i);
                UI.tavernSelctionUI.SetActive(true);
            }
            //Reward
            else if (selectionInts[i] == 2002)
            {
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.rewardSelctionUI.transform.SetSiblingIndex(i);
                UI.rewardSelctionUI.SetActive(true);
            }
            //Shop
            else if (selectionInts[i] == 2003)
            {
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.shopSelctionUI.transform.SetSiblingIndex(i);
                UI.shopSelctionUI.SetActive(true);

            }
            //Boss
            else if (selectionInts[i] == 2004)
            {
                UI.SetOptionPreviewSprites(i, selectionInts[i]);
                UI.bossSelctionUI.transform.SetSiblingIndex(i);
                UI.bossSelctionUI.SetActive(true);
            }
        }
    }
    public void StartBattle()
    {
        if (BattleController.Instance.TurnController.EnemyParty != null)
        {
            Destroy(BattleController.Instance.TurnController.EnemyParty);
        }
        PlayerParty party = Instantiate(enemies[selectionInts[selectedInt]]) as PlayerParty;
        BattleController.Instance.SetupBattle(party);
    }
    public void SetPostBattleUIDetails(int floor, int battle)
    {
        UI.swipe.Content.anchoredPosition = Vector3.zero;
        if (battle > 10)
        {
            GameDetails.Floor = floor + 1;
            GameDetails.ProgressOnCurrentFloor = 1;
        }
        else
        {
            GameDetails.Floor = floor;
            GameDetails.ProgressOnCurrentFloor = battle;
        }
        SetFloorText();
        SetOptions();
        UI.SetGoldText(GameDetails.Gold.ToString());
    }
    public void SetFloorText()
    {
        UI.FloorText.text = GameDetails.Floor + "-" + GameDetails.ProgressOnCurrentFloor;
    }
}
[Serializable]
public class GameDetails
{
    public int ProgressOnCurrentFloor;
    public int Floor;
    [SerializeField]private int gold;
    public int Gold {
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