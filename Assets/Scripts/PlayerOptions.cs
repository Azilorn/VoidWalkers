using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerOptions : MonoBehaviour
{
    [SerializeField] private AttackOptions attackOptions;
    [SerializeField] private ItemOptions itemOptions;
    [SerializeField] private PartyOptions partyOptions;
    [SerializeField] private RunOptions runOptions;
    private bool menuClosing = false;
    [SerializeField] private GameObject previousMenu;

    public AttackOptions AttackOptions { get => attackOptions; set => attackOptions = value; }
    public ItemOptions ItemOptions { get => itemOptions; set => itemOptions = value; }
    public PartyOptions PartyOptions { get => partyOptions; set => partyOptions = value; }
    public RunOptions RunOptions { get => runOptions; set => runOptions = value; }

    public void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(CoreUI.OpenMenu(gameObject, 0, 0.25f));
    }

    public void EnableNextMenu(GameObject go) {

        gameObject.transform.DOScale(Vector3.zero, 0.25f);
       
        CoreUI.DoFadeOut(gameObject, 0.15f);
     
        if (go.GetComponent<AttackOptions>()) {
            attackOptions.SetButtons(BattleController.Instance.MasterPlayerParty.ReturnCreatureStats(BattleController.Instance.MasterPlayerParty.selectedCreature));
            go.transform.localScale = Vector3.zero;
            StartCoroutine(CoreUI.OpenMenu(go,0, 0.15f));
        }
        if (go.GetComponent<PartyOptions>())
        {
            partyOptions.SetUI();
            partyOptions.gameObject.SetActive(true);
            CoreUI.DoFadeIn(partyOptions.gameObject, 0.15f);
        }
        if (go.GetComponent<ItemOptions>())
        {
            go.SetActive(true);
            CoreUI.DoFadeIn(ItemOptions.gameObject, 0.15f);
            CoreUI.DoFadeIn(go, 0.35f);
        }
    }

   
}
