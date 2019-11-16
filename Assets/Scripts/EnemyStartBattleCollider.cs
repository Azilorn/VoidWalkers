using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartBattleCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(BattleController.Instance == null)
            return;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        BattleController.Instance.SetupBattle(GetComponent<PlayerParty>());
    }
}
