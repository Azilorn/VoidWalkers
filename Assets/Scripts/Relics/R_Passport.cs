using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class R_Passport : Relics
{
    public override IEnumerator RunEffect()
    {
        if (PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor == 10)
        {
            Debug.Log("TODO Show negative Message for trying to skip boss floor ");
        }
        else
        {
            RelicUIIcon.Instance.img.sprite = InventoryController.Instance.relics[(int)RelicName.Passport].icon;
            RelicUIIcon.Instance.gameObject.SetActive(true);
            RelicUIIcon.Instance.SetDeactive(1.5f);

            PreBattleSelectionController.Instance.SetPostFloorOptionDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);

            while (RelicUIIcon.Instance.gameObject.activeInHierarchy == true)
            {
                yield return null;
            }
            if (InventoryController.Instance.relics[(int)RelicName.Passport].relicLostOnUse == RelicLostOnUse.Yes)
            {
                InventoryController.Instance.RemoveRelic((int)RelicName.Passport);
            }

            WorldMenuUI.Instance.CloseRelicOptions();
            yield return new WaitForSeconds(2.5f);
        }
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
