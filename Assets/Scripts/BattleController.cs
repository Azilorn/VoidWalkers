using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    [SerializeField] private TurnController turnController;
    [SerializeField] private StatusController statusController;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private AttackController attackController;
    [SerializeField] private ElementMatrix elementMatrix;
    [SerializeField] private BattleUI battleUI;
    [SerializeField] private Image player1CreatureImage;
    [SerializeField] private Image player2CreatureImage;
    [SerializeField] private PlayerParty masterPlayerParty;

    public TurnController TurnController { get => turnController; set => turnController = value; }
    public StatusController StatusController { get => statusController; set => statusController = value; }
    public AnimationController AnimationController { get => animationController; set => animationController = value; }
    public BattleUI BattleUI { get => battleUI; set => battleUI = value; }
    public Image Player1CreatureImage { get => player1CreatureImage; set => player1CreatureImage = value; }
    public Image Player2CreatureImage { get => player2CreatureImage; set => player2CreatureImage = value; }
    public ElementMatrix ElementMatrix { get => elementMatrix; set => elementMatrix = value; }
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
        TurnController.SetParties(MasterPlayerParty, e);

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
        MenuTransitionsController.Instance.StartTransition(1, false);
        MenuTransitionsController.Instance.transitions[1].gameObject.SetActive(true);
    }

    public void SwapPartyIndex(int startingIndex, int i)
    {
        PlayerCreatureStats startingCreature = MasterPlayerParty.party[startingIndex];
        PlayerCreatureStats droppedCreature = MasterPlayerParty.party[i];

        MasterPlayerParty.party[startingIndex] = droppedCreature;
        MasterPlayerParty.party[i] = startingCreature;
    }
}

