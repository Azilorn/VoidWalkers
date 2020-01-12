using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernUI : MonoBehaviour
{

    public static bool isReviveUsed;

    public void MoveToNextFloor()
    {
        PreBattleSelectionController.Instance.SetPostFloorOptionDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
    }
    public void Rest()
    {
        StartCoroutine(RestCoroutine());
    }
    public IEnumerator RestCoroutine()
    {
        BattleUI.Instance.PlayerOptions.PartyOptions.BottomBar.SetActive(false);
        yield return StartCoroutine(BattleUI.Instance.OpenPartyOptions());
        yield return new WaitForSeconds(1f);
        List<int> hpValues = new List<int>();
        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party.Length; i++)
        {
            if (BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP > 0)
            {
                int val = Mathf.RoundToInt(BattleController.Instance.MasterPlayerParty.party[i].creatureStats.MaxHP * 0.3f);
                BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP += val;
                BattleController.Instance.MasterPlayerParty.party[i].ClampHP();
                hpValues.Add(BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP);
            }
            else {
                hpValues.Add(BattleController.Instance.MasterPlayerParty.party[i].creatureStats.HP);
            }
        }
        for (int i = 0; i < BattleUI.Instance.PlayerOptions.PartyOptions.PartyCreatureUIs.Count; i++)
        {
            StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.PartyCreatureUIs[i].UpdateHPSlider(hpValues[i]));
        }
        yield return new WaitForSeconds(1.5f);
        BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwards(true);
        BattleUI.Instance.PlayerOptions.PartyOptions.BottomBar.SetActive(true);
        gameObject.SetActive(false);
        MoveToNextFloor();
    }
    public void Gamble()
    {
        StartCoroutine(GambleCoroutine());
    }
    public IEnumerator GambleCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
    public void Revive()
    {
        StartCoroutine(ReviveCoroutine());
    }
    public IEnumerator ReviveCoroutine()
    {
        isReviveUsed = false;
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.WorldTavernRevive;
        yield return StartCoroutine(BattleUI.Instance.OpenPartyOptions());
        yield return new WaitForSeconds(1f);
        while (!isReviveUsed) {
            yield return  new WaitForEndOfFrame();
        }
        yield return StartCoroutine(BattleUI.Instance.ClosePartyOptions());
        MoveToNextFloor();
        gameObject.SetActive(false);
    }
}
