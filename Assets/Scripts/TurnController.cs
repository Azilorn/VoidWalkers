using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] private bool playerFirst = false;
    [SerializeField] private PlayerParty playerParty;
    [SerializeField] private PlayerParty enemyParty;

    public PlayerParty PlayerParty { get => playerParty; set => playerParty = value; }
    public PlayerParty EnemyParty { get => enemyParty; set => enemyParty = value; }
    public bool PlayerFirst { get => playerFirst; set => playerFirst = value; }

    public void SetParties(PlayerParty player, PlayerParty enemy) {

        playerFirst = false;
        playerParty = player;

        if (EnemyParty != null) {
            Destroy(EnemyParty.gameObject);
            EnemyParty = null;
        }
        enemyParty = enemy;
    }

    public void SetFirstAttacker() {

        playerFirst = CompareSpeeds(PlayerParty, EnemyParty);
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
        else {
            return false;
        }
    }
}
