using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    [SerializeField] private AnimationController animationController;
    [SerializeField] private AttackController attackController;
    [SerializeField] private Image player1CreatureImage;
    [SerializeField] private Image player2CreatureImage;
    [SerializeField] private PlayerParty masterPlayerParty;
    [SerializeField] private PlayerParty enemyParty;

    private int turnCount;
    [SerializeField] private bool playerFirst = false;
    [SerializeField] private PlayerParty playerParty;

    public PlayerParty EnemyParty { get => enemyParty; set => enemyParty = value; }
    public bool PlayerFirst { get => playerFirst; set => playerFirst = value; }
    public int TurnCount { get => turnCount; set => turnCount = value; }

    public AnimationController AnimationController { get => animationController; set => animationController = value; }
    public Image PlayerCreatureImage { get => player1CreatureImage; set => player1CreatureImage = value; }
    public Image EnemyCreatureImage { get => player2CreatureImage; set => player2CreatureImage = value; }
    public AttackController AttackController { get => attackController; set => attackController = value; }
    public PlayerParty MasterPlayerParty { get => masterPlayerParty; set => masterPlayerParty = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
      
    }

    public void SetupBattle(PlayerParty e)
    {
        AttackController.SetDefaultStarts();
        SetParties(e);

        for (int i = 0; i < MasterPlayerParty.party.Length; i++) {
            if (MasterPlayerParty.party[i].creatureStats.HP <= 0)
            {
                continue;
            }
            else {
                MasterPlayerParty.selectedCreature = i;
                break;
            }
        }

        for (int i = 0; i < e.party.Length; i++)
            if (e.party[i].creatureStats.HP > 0)
            {
                e.selectedCreature = i;
                break;
            }

        for (int i = 0; i < MasterPlayerParty.party.Length; i++)
        {
            if (MasterPlayerParty.party[i] != null) {
                MasterPlayerParty.party[i].creatureStats.SetCreatureBattleStats();
            }
        }
        for (int i = 0; i < e.party.Length; i++)
        {
            if (e.party[i] != null)
            {
                e.party[i].creatureStats.SetCreatureBattleStats();
            }
        }
        MenuTransitionsController.Instance.StartTransition(4, false);
        CoreUI.Instance.SetBattleUIAtStart();
        StartCoroutine(MenuTransitionsController.Instance.StartTransitionWithDelay(4, true, 0.5f));
        StartCoroutine(CoreUI.OpenMenu(CoreUI.Instance.BattleCanvasTransform.gameObject, 0.5f, 0));
        StartCoroutine(CoreUI.CloseMenu(CoreUI.Instance.BottomBar, 0.5f, 0));
        StartCoroutine(CoreUI.CloseMenu(CoreUI.Instance.TopBar, 0.5f, 0));
    }

    public void SwapPartyIndex(int startingIndex, int i)
    {
        PlayerCreatureStats startingCreature = MasterPlayerParty.party[startingIndex];
        PlayerCreatureStats droppedCreature = MasterPlayerParty.party[i];

        MasterPlayerParty.party[startingIndex] = droppedCreature;
        MasterPlayerParty.party[i] = startingCreature;
    }

    public void SetParties(PlayerParty enemy)
    {

        TurnCount = 1;
        playerFirst = false;

        if (EnemyParty != null)
        {
            Destroy(EnemyParty.gameObject);
        }
        EnemyParty = enemy;
    }

    public void SetFirstAttacker()
    {

        playerFirst = CompareSpeeds(MasterPlayerParty, EnemyParty);
    }
    private bool CompareSpeeds(PlayerParty p1, PlayerParty p2)
    {
        PlayerCreatureStats p1Stats = p1.party[p1.selectedCreature];
        PlayerCreatureStats p2Stats = p2.party[p2.selectedCreature];

        if (p1Stats.creatureStats.BattleSpeed > p2Stats.creatureStats.BattleSpeed)
        {
            return true;
        }
        else if (p1Stats.creatureStats.BattleSpeed == p2Stats.creatureStats.BattleSpeed)
        {
            if ((p1Stats.creatureStats.BattleSpeed * p1Stats.creatureStats.level) > (p2Stats.creatureStats.BattleSpeed * p2Stats.creatureStats.level))
            {
                return true;
            }
            else return false;
        }
        else
        {
            return false;
        }
    }
}

