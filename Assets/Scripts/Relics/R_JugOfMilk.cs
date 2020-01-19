using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class R_JugOfMilk : Relics
{
    public override IEnumerator RunEffect()
    {
        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party.Length; i++) {

            PlayerCreatureStats p = BattleController.Instance.MasterPlayerParty.party[i];
            p.creatureStats.HP += (int)(p.creatureStats.MaxHP * 0.2f);
            p.ClampHP();
        }
        BattleUI.Instance.PlayerStats[0].SetPlayerStats(BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature], BattleController.Instance.TurnController.PlayerParty);
        RelicUIIcon.Instance.img.sprite = InventoryController.Instance.relics[(int)RelicName.JugOfMilk].icon;
        RelicUIIcon.Instance.gameObject.SetActive(true);
        RelicUIIcon.Instance.SetDeactive(1.5f);

        while (RelicUIIcon.Instance.gameObject.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);


    }
    public override bool CalculateChance()
    {
        double rnd = Random.Range(0, 100);

        if (rnd <= 100)
        {
            return true;
        }
        else return false;
    }
}
