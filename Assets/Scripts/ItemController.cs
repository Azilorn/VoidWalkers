using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance;
    private int currentlySelectedCreature = 0;
    [SerializeField] private int currentlySelectedItem = 0;
    [SerializeField] private Item selectedItem;
    [SerializeField] private CreatureDetailsUI creatureDetails;
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
            case ItemType.APUp:
                if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                    OpenMenuToSelectCreatureInBattle();
                else OpenMenuToSelectCreatureOutsideOfBattle();
                break;
            default:
                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                break;
        }
    }
    private void OpenMenuToSelectCreatureOutsideOfBattle()
    {
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.ItemSelectCreature;
        BattleUI.Instance.PlayerOptions.ItemOptions.OnMenuBackwards(true);
        WorldMenuUI.Instance.OpenAndSetParty();
    }
    private void OpenMenuToSelectCreatureInBattle()
    {
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.ItemSelectCreature;
        BattleUI.Instance.PlayerOptions.ItemOptions.OnMenuBackwards(true);
        StartCoroutine(BattleUI.Instance.OpenPartyOptions());
    }
    public void UseItem(int creatureIndex) {

      StartCoroutine(UseItemCoroutine(creatureIndex));
    }
    public IEnumerator UseItemCoroutine(int creatureIndex) {

        currentlySelectedCreature = creatureIndex;
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.ItemSelectCreature || BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive || BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive)
        {
            switch (InventoryController.Instance.ReturnItem(CurrentlySelectedItem).itemType)
            {
                case ItemType.Potion:
                    yield return StartCoroutine(HealCreature(creatureIndex));
                    if (canUse)
                    {
                        yield return StartCoroutine(RemoveItemCheck());
                        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                        {
                            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                            yield return StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsBattle());
                            StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                            canUse = false;
                        }
                        else
                        {
                            if (!InventoryController.Instance.ownedItems.ContainsKey(CurrentlySelectedItem))
                            {
                                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                                StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsWorld());
                                WorldMenuUI.Instance.OpenAndSetInventory();
                            }
                        }
                    }
                    break;
                case ItemType.Revive:
                  
                    yield return StartCoroutine(ReviveCreatre(creatureIndex));
                    if (canUse)
                    {
                        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive || BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive)
                        {
                            Debug.Log("Dont Remove");
                        }
                        else {
                            yield return StartCoroutine(RemoveItemCheck());
                        }
                        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                        {
                            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                            yield return StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0, 0.25f));
                            StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                            canUse = false;
                        }
                        else
                        {
                            if (!InventoryController.Instance.ownedItems.ContainsKey(CurrentlySelectedItem))
                            {
                                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                                StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsWorld());
                                if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldUIRevive || BattleUI.Instance.CurrentMenuStatus == MenuStatus.WorldTavernRevive) {

                                }
                                else WorldMenuUI.Instance.OpenAndSetInventory();
                               
                            }
                        }
                        TavernUI.isReviveUsed = true;
                        BattleUI.Instance.PlayerOptions.PartyOptions.BottomBar.SetActive(true);
                    }
                    break;
                case ItemType.AntiAilment:
                    yield return StartCoroutine(RemoveAilment(creatureIndex));
                    if (canUse)
                    {
                        yield return StartCoroutine(RemoveItemCheck());
                        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                        {
                            BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                            yield return StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0, 0.15f));
                            StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                            canUse = false;
                        }
                        else
                        {
                            if (!InventoryController.Instance.ownedItems.ContainsKey(CurrentlySelectedItem))
                            {
                                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                                StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsWorld());
                                WorldMenuUI.Instance.OpenAndSetInventory();
                            }
                        }
                    }
                    break;
                case ItemType.Escape:
                    if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                    {
                        BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                        yield return StartCoroutine(BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.PartyOptions.gameObject, 0, 0.15f));
                        StartCoroutine(BattleController.Instance.AttackController.SkipPlayerAttack());
                    }
                    yield return StartCoroutine(RemoveItemCheck());
                    break;
                case ItemType.APUp:
                    creatureDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(creatureDetails.gameObject, 0.25f);
                    BattleUI.DoFadeIn(creatureDetails.MainBody.gameObject, 0.25f);
                    BattleUI.DoFadeIn(creatureDetails.AbilitiesBody.gameObject, 0.25f);
                    StartCoroutine(BattleUI.OpenMenu(creatureDetails.MainBody.gameObject, 0f, 0.25f));
                    StartCoroutine(BattleUI.OpenMenu(creatureDetails.AbilitiesBody.gameObject, 0f, 0.25f));
                    creatureDetails.SetMenu(BattleController.Instance.MasterPlayerParty.party[creatureIndex]);
                    break;
                default:
                    BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
                    break;
            }
        }
        else yield return null;
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator UseAP(int abilityIndex, CreatureDetailsAbility creatureDetailsAbility) {

        CreatureAbility ability = BattleController.Instance.MasterPlayerParty.party[currentlySelectedCreature].creatureAbilities[abilityIndex];
        if (ability.remainingCount != ability.ability.abilityStats.maxCount)
        {
            //Add Audio
            int difference = (ability.ability.abilityStats.maxCount - ability.remainingCount);

            int plusAmount = 0;
            if (InventoryController.Instance.ReturnItem(CurrentlySelectedItem).effectAmount >difference) {
                plusAmount = difference;
            }
            else plusAmount = InventoryController.Instance.ReturnItem(CurrentlySelectedItem).effectAmount;

          bool  finished = false;
            while (!finished) {
                float waitDuration = (0.75f / plusAmount);
                for (int i = 1; i < plusAmount + 1; i++)
                {
                    creatureDetailsAbility.remainingCount.text = (ability.remainingCount + i) + "/" + ability.ability.abilityStats.maxCount;
                    yield return new WaitForSecondsRealtime(waitDuration);
                }
                finished = true;    
            }
            ability.remainingCount += InventoryController.Instance.ReturnItem(CurrentlySelectedItem).effectAmount;
            ability.remainingCount = Mathf.Clamp(ability.remainingCount, 0, ability.ability.abilityStats.maxCount);
            creatureDetailsAbility.remainingCount.text = ability.remainingCount + "/" + ability.ability.abilityStats.maxCount;

            yield return StartCoroutine(RemoveItemCheck());

            if (!InventoryController.Instance.ownedItems.ContainsKey(CurrentlySelectedItem))
            {
                BattleUI.DoFadeOut(creatureDetails.gameObject, 0.25f);
                BattleUI.DoFadeOut(creatureDetails.MainBody.gameObject, 0.25f);
                BattleUI.DoFadeOut(creatureDetails.AbilitiesBody.gameObject, 0.25f);
                StartCoroutine(BattleUI.CloseMenu(creatureDetails.MainBody.gameObject, 0f, 0.25f));
                StartCoroutine(BattleUI.CloseMenu(creatureDetails.AbilitiesBody.gameObject, 0f, 0.25f));

                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;

                StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsWorld());

                WorldMenuUI.Instance.OpenAndSetInventory();
            }
        }
        else {
            UIAudio.Instance.PlayDenyAudio();
        }
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator RemoveItemCheck()
    {
        if (InventoryController.Instance.relicsScripts[(int)RelicName.MalachiteQuill].CalculateChance() == false)
        {
            InventoryController.Instance.RemoveItem(CurrentlySelectedItem);
        }
        else {
            yield return StartCoroutine(WorldMenuUI.Instance.UseRelicEvent(RelicName.MalachiteQuill, true));
        }

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator HealCreature(int creatureIndex)
    {
        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP == BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.MaxHP)
        {
            string dialogueText = BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + " Already At Max HP.";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
            canUse = false;
        }
        else if(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP <= 0)
        {
            string dialogueText = BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + " is dead!";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
            canUse = false;
        }
        else 
        {
            int startingHp = BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP;
            BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP += InventoryController.Instance.ReturnItem(CurrentlySelectedItem).effectAmount;
            BattleController.Instance.MasterPlayerParty.party[creatureIndex].ClampHP();
            if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                BattleUI.Instance.SetPlayerBattleUI();
            if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
                yield return StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.PartyCreatureUIs[creatureIndex].UpdateHPSlider(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP));
            else yield return StartCoroutine(WorldMenuUI.Instance.PartyOptions.PartyCreatureUIs[creatureIndex].UpdateHPSlider(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP));
            yield return new WaitForSeconds(1f);
            string dialogueText = (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP - startingHp) + " HP Restored.";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
            canUse = true;
        }
    }
    public IEnumerator ReviveCreatre(int creatureIndex)
    {
        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureStats.HP > 0)
        {
            string dialogueText = BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + " is not dead";
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
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
            string dialogueText = "<b>" + BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + "</b>" + " has been returned from the void!"; 
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
            BattleUI.Instance.PlayerOptions.PartyOptions.BottomBar.SetActive(false);
            canUse = true;
        }
    }
    // Out Paramater Dialogue Text for Dialogue Box and Remove Ailment code. 
    public IEnumerator RemoveAilment(int creatureIndex)
    {
        string dialogueText = "";
        if (BattleController.Instance.MasterPlayerParty.party[creatureIndex].CheckForAilment(InventoryController.Instance.ReturnItem(CurrentlySelectedItem).negativeAilment, out dialogueText) == true )
        {
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(BattleController.Instance.MasterPlayerParty.party[creatureIndex].creatureSO.creatureName + " " + dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
            canUse = true;
        }
        else {
            yield return StartCoroutine(BattleUI.Instance.TypeDialogue(InventoryController.Instance.ReturnItem(CurrentlySelectedItem).itemName + " " + dialogueText, BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
            canUse = false;
        }
    }   
    public IEnumerator EscapeBattle()
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator RestoreAP(int creatureIndex, int abilityIndex) {
        yield return new WaitForEndOfFrame();
    }
}
