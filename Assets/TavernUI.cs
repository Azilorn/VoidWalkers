using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernUI : MonoBehaviour
{
    public void MoveToNextFloor()
    {
        PreBattleSelectionController.Instance.SetPostBattleUIDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
    }
    public void Rest()
    {
        StartCoroutine(RestCoroutine());
    }
    public IEnumerator RestCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
    public void Gamble()
    {
        StartCoroutine(GambleCoroutine());
    }
    public IEnumerator GambleCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
    public void EditParty()
    {
        StartCoroutine(EditPartyCoroutine());
    }
    public IEnumerator EditPartyCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
    public void Revive()
    {
        StartCoroutine(ReviveCoroutine());
    }
    public IEnumerator ReviveCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
}
