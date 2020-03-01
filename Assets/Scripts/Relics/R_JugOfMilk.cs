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
            if (p.creatureStats.HP <= 0)
                continue;
            p.creatureStats.HP += (int)(p.creatureStats.MaxHP * 0.2f);
            p.ClampHP();
        }
        CoreUI.Instance.PlayerStats[0].SetPlayerStats(BattleController.Instance.MasterPlayerParty.party[BattleController.Instance.MasterPlayerParty.selectedCreature], BattleController.Instance.MasterPlayerParty);
        RelicUIIcon.Instance.img.sprite = InventoryController.Instance.relics[(int)RelicName.JugOfMilk].icon;
        RelicUIIcon.Instance.gameObject.SetActive(true);
        RelicUIIcon.Instance.SetDeactive(1.5f);

        while (RelicUIIcon.Instance.gameObject.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return StartCoroutine(CoreUI.Instance.TypeDialogue(
            "Your party is healed for <color=#7ED1CA>20%</color> of their maximum HP", 
            CoreUI.Instance.DialogueBox.Dialogue, 1f, true));
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
    public override bool CalculateChance()
    {
        double rnd = Random.Range(0, 100);

        if (rnd <= 10)
        {
            return true;
        }
        else return false;
    }
}
