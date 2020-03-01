using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class R_PrayerBeads : Relics
{
    public override IEnumerator RunEffect()
    {
        if (BattleController.Instance.MasterPlayerParty.party[BattleController.Instance.MasterPlayerParty.selectedCreature].creatureStats.HP <= 0)
        {
            BattleController.Instance.MasterPlayerParty.party[BattleController.Instance.MasterPlayerParty.selectedCreature].creatureStats.HP = 1;
            RelicUIIcon.Instance.img.sprite = InventoryController.Instance.relics[(int)RelicName.PrayerBeads].icon;
            RelicUIIcon.Instance.gameObject.SetActive(true);
            RelicUIIcon.Instance.SetDeactive(1.5f);

        }
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

        if (rnd <= 10) {
            return true;
        } else return false;
    }
}
