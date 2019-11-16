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
        StartCoroutine(BattleUI.OpenMenu(gameObject, 0, 0.25f));
    }

    public void EnableNextMenu(GameObject go) {

        gameObject.transform.DOScale(Vector3.zero, 0.25f);
       
        BattleUI.DoFadeOut(gameObject, 0.15f);
     
        if (go.GetComponent<AttackOptions>()) {
            attackOptions.SetButtons(BattleController.Instance.TurnController.PlayerParty.ReturnCreatureStats(BattleController.Instance.TurnController.PlayerParty.selectedCreature));
            go.transform.localScale = Vector3.zero;
            StartCoroutine(BattleUI.OpenMenu(go,0, 0.35f));
        }
        if (go.GetComponent<PartyOptions>())
        {
            partyOptions.SetUI();
            partyOptions.gameObject.SetActive(true);
            partyOptions.transform.localScale = Vector3.one;
            BattleUI.DoFadeIn(partyOptions.gameObject, 0.10f);
            StartCoroutine(BattleUI.OpenMenuFromSideToCenter(partyOptions.GetGameObjects(), 0.02f, 0.35f, Camera.main.pixelWidth * 2));
            StartCoroutine(BattleUI.ToggleMenuFromBottomToCenter(partyOptions.BottomBar, 0f, 0.25f, -250, 0));
            StartCoroutine(BattleUI.ToggleMenuFromBottomToCenter(partyOptions.Header, 0f, 0.25f, 250, 0));
        }
        if (go.GetComponent<ItemOptions>())
        {
            go.SetActive(true);
            BattleUI.DoFadeIn(ItemOptions.MainBody, 0.35f);
            BattleUI.DoFadeIn(go, 0.35f);
            StartCoroutine(BattleUI.OpenMenu(ItemOptions.MainBody, 0, 0.25f));
            StartCoroutine(BattleUI.ToggleMenuFromAtoB(itemOptions.Header, 0, 0.25f, new Vector3(0, 200, 0), Vector3.zero));
            StartCoroutine(BattleUI.ToggleMenuFromAtoB(itemOptions.BottomBar, 0, 0.25f, new Vector3(0, -250, 0), Vector3.zero));
          
        }
    }

   
}
