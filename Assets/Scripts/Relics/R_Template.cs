using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class R_Template : Relics
{
    public override IEnumerator RunEffect()
    {
      
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
