using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance;

    [SerializeField] private int currentlySelectedItem = 0;
    [SerializeField] private Item selectedItem;
    private bool canUse = false;

    public int CurrentlySelectedItem { get => currentlySelectedItem; set => currentlySelectedItem = value; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMenuOption(int itemIndex)
    {
        switch (InventoryController.Instance.ReturnItem(itemIndex).itemType)
        {
            case ItemType.Potion:
                if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                    OpenMenuToSelectCreatureInBattle();
                else OpenMenuToSelectCreatureOutsideOfBattle();
                
                break;
            case ItemType.Revive:
                if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                    OpenMenuToSelectCreatureInBattle();
                else OpenMenuToSelectCreatureOutsideOfBattle();
                break;
            case ItemType.AntiAilment:
                if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                    OpenMenuToSelectCreatureInBattle();
                else OpenMenuToSelectCreatureOutsideOfBattle();
                break;
            case ItemType.Escape:
                BattleUI.Instance.CurrentMenuStatus = MenuStatus.CloseMenu;
                break;
            default:
                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                break;
        }
    }
    private void OpenMenuToSelectCreatureOutsideOfBattle()
    {
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.ItemSelectCreature;
        StartCoroutine(BattleUI.CloseMenu(WorldMenuUI.Instance.ItemOptions.gameObject, 0, 0.25f));
        WorldMenuUI.Instance.OpenAndSetParty();
    }
    private void OpenMenuToSelectCreatureInBattle()
    {
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.ItemSelectCreature;
        StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.ItemOptions.gameObject, 0, 0.25f));
        BattleUI.Instance.PlayerOptions.PartyOptions.SetUI();

        BattleUI.Instance.PlayerOptions.PartyOptions.gameObject.SetActive(true);
        BattleUI.Instance.PlayerOptions.PartyOptions.transform.localScale = Vector3.one;
        BattleUI.DoFadeIn(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0.10f);
        StartCoroutine(BattleUI.OpenMenuFromSideToCenter(BattleUI.Instance.PlayerOptions.PartyOptions.GetGameObjects(), 0.02f, 0.35f, Camera.main.pixelWidth * 2));
        StartCoroutine(BattleUI.ToggleMenuFromBottomToCenter(BattleUI.Instance.PlayerOptions.PartyOptions.BottomBar, 0f, 0.25f, -250, 0));
        StartCoroutine(BattleUI.ToggleMenuFromBottomToCenter(BattleUI.Instance.PlayerOptions.PartyOptions.Header, 0f, 0.25f, 250, 0));
    }
    public void UseItem(int creatureIndex) {

        StartCoroutine(UseItemCoroutine(creatureIndex));
    }
    public IEnumerator UseItemCoroutine(int creatureIndex) {

        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature)
        {
            switch (InventoryController.Instance.ReturnItem(CurrentlySelectedItem).itemType)
            {
                case ItemType.Potion:
                    yield return StartCoroutine(HealCreature(creatureIndex));
                    if (canUse)
                    {
                        InventoryController.Instance.RemoveItem(CurrentlySelectedItem);
                        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                        {
                             BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                            yield return StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsBattle());
                            StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                            canUse = false;
                        }
                    }
                    break;
                case ItemType.Revive:
                    yield return StartCoroutine(ReviveCreatre(creatureIndex));
                    if (canUse)
                    {
                        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                        {
                            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                            yield return StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0, 0.25f));
                            StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                            canUse = false;
                        }
                         InventoryController.Instance.RemoveItem(CurrentlySelectedItem);
                    }
                    break;
                case ItemType.AntiAilment:
                    yield return StartCoroutine(RemoveAilment(creatureIndex));
                    if (canUse)
                    {
                        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                        {
                            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                            yield return StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0, 0.15f));
                            StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                            canUse = false;
                        }
                        InventoryController.Instance.RemoveItem(CurrentlySelectedItem);
                    }
                    break;
                case ItemType.Escape:
                    if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                    {
                        BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                        yield return StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0, 0.15f));
                        StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                    }
                    InventoryController.Instance.RemoveItem(CurrentlySelectedItem);
                    break;
                default:
                    BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                    break;
            }
        }
        else yield return null;
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator HealCreature(int creatureIndex)
    {
        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP == BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.MaxHP)
        {
            string dialogueText = "Already At Max HP.";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = false;
        }
        else if(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP <= 0)
        {
            string dialogueText = "Void Walker is dead!";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = false;
        }
        else 
        {
            BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP += InventoryController.Instance.ReturnItem(CurrentlySelectedItem).effectAmount;
            BattleController.Instance.MasterPlayerParty.party[creatureIndex].ClampHP();
            if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                BattleUI.Instance.SetPlayerBattleUI();
            if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                yield return StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.PartyCreatureUIs[creatureIndex].UpdateHPSlider(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP));
            else yield return StartCoroutine(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[creatureIndex].UpdateHPSlider(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP));
            yield return new WaitForSeconds(1f);
            string dialogueText = "HP Restored.";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = true;
        }
    }
    public IEnumerator ReviveCreatre(int creatureIndex)
    {
        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP > 0)
        {
            string dialogueText = BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + " is not dead";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = false;
        }
        else if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP <= 0)
        {
            BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP += InventoryController.Instance.ReturnItem(CurrentlySelectedItem).effectAmount;
            BattleController.Instance.MasterPlayerParty.party[creatureIndex].ClampHP();
            if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                BattleUI.Instance.SetPlayerBattleUI();
            if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                yield return StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.PartyCreatureUIs[creatureIndex].UpdateHPSlider(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP));
            else yield return StartCoroutine(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[creatureIndex].UpdateHPSlider(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP));
            yield return new WaitForSeconds(1f);
            string dialogueText = "Void Walker has been returned to the void!";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = true;
        }
    }
    // Out Paramater Dialogue Text for Dialogue Box and Remove Ailment code. 
    public IEnumerator RemoveAilment(int creatureIndex)
    {
        string dialogueText = "";
        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].CheckForAilment(InventoryController.Instance.ReturnItem(CurrentlySelectedItem).negativeAilment, out dialogueText) == true )
        {
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + Environment.NewLine  + dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = true;
        }
        else {
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(InventoryController.Instance.ReturnItem(CurrentlySelectedItem).itemName + " " + dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f));
            canUse = false;
        }
    }   
    public IEnumerator EscapeBattle()
    {
        yield return new WaitForSeconds(1f);
    }
}
