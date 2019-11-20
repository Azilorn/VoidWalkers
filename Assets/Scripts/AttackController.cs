using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    private Coroutine co;
     PlayerCreatureStats p1 = null;
     PlayerCreatureStats p2 = null;
     Ability ability = null;
    int playerAbilityIndex = 0;
    bool firstAttackerAlreadySet = false;
    bool turnedEnded = false;
    bool fightEnded = false;
     private static int turncount = 0;
    bool canAttack = true;
    bool attackSelf = false;

    public static int Turncount { get => turncount; set => turncount = value; }

    public void SetDefaultStarts() {
        p1 = new PlayerCreatureStats();
        p2 = new PlayerCreatureStats();
        ability = null;
        playerAbilityIndex = 0;
        firstAttackerAlreadySet = false;
        turnedEnded = false;
        Turncount = 0;
        canAttack = true;
        attackSelf = false;
        fightEnded = false;
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
    }

    public void AttackPlayerButton(int abilityIndex)
    {
        StartCoroutine(AttackPlayer(abilityIndex));
    }
    public IEnumerator AttackPlayer(int abilityIndex)
    {
        BattleUI.Instance.PlayerOptions.AttackOptions.gameObject.SetActive(false);
        BattleUI.Instance.PlayerOptions.gameObject.SetActive(false);
        bool deathFinished = true;
        if (BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature].creatureSO != null && BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature].creatureSO != null)
        {
            if (BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature].creatureStats.HP <= 0 || BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature].creatureStats.HP <= 0)
            {
                Debug.Log("Check If Remaining Creatures");
                //TODO Check if other party creatures are alive and allow a swap
                PlayerParty party = null;
                if (BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature].creatureStats.HP <= 0)
                {
                    Debug.Log("Selecting Enemy Party");
                    party = BattleController.Instance.TurnController.EnemyParty;
                }
                else if(BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature].creatureStats.HP <= 0){
                    party = BattleController.Instance.TurnController.PlayerParty;
                }
                if (party == BattleController.Instance.TurnController.PlayerParty)
                {
                    for (int i = 0; i < party.party.Length; i++)
                    {
                        if (party.party[i].creatureStats.HP > 0)
                        {
                            BattleUI.Instance.CurrentMenuStatus = MenuStatus.SelectNewCreaturePostDeath;
                        }
                    }
                }
                if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
                {
                    yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + party.party[party.selectedCreature].creatureSO.creatureName + "</b>" + " has returned to the void!", BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
                    BattleController.Instance.Player1CreatureImage.transform.DOScale(Vector3.zero, 0.5f);
                    yield return new WaitForSeconds(0.75f);
                    yield return StartCoroutine(BattleUI.Instance.SelectNewCreatureAfterDeath());
                    BattleUI.Instance.SetPlayerBattleUI();
                    p1 = new PlayerCreatureStats();
                    p2 = new PlayerCreatureStats();
                    Turncount = 2;
                    deathFinished = true;
                }
                else
                {
                    //TODO fix issue with Poison or other effects killing the player after his turn kills the enemy
                    for (int i = 0; i < party.party.Length; i++)
                    {
                        if (party.party[i].creatureStats.HP > 0)
                        {
                            if (party.party[i].creatureSO == null)
                            {
                                fightEnded = true;
                                continue;
                            }
                            yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + party.party[party.selectedCreature].creatureSO.creatureName + "</b>" + " has returned to the void!", BattleUI.Instance.DialogueBox.Dialogue, 1f, true));

                            if (ReturnImage(party.party[i]) == BattleController.Instance.Player1CreatureImage)
                            {
                                yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[0]));
                            }
                            else {
                                yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[1]));
                            }
                            Vector3 returnDirection = ReturnImage(party.party[i]).transform.localScale;
                            ReturnImage(party.party[i]).transform.DOScale(Vector3.zero, 0.5f);
                            BattleUI.DoFadeOut(ReturnImage(party.party[i]).gameObject, 0.5f);
                            yield return new WaitForSeconds(0.5f);

                           
                            party.selectedCreature = i;
                            ReturnImage(party.party[i]).sprite = party.party[i].creatureSO.creatureEnemyIcon;
                            ReturnImage(party.party[i]).rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, party.party[i].creatureSO.width);
                            ReturnImage(party.party[i]).rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, party.party[i].creatureSO.height);
                            if (ReturnImage(party.party[i]) == BattleController.Instance.Player1CreatureImage)
                            {
                                yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[0]));
                            }
                            else
                            {
                                yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[1]));
                            }
                            BattleUI.DoFadeIn(ReturnImage(party.party[i]).gameObject, 0.5f);
                            ReturnImage(party.party[i]).transform.DOScale(returnDirection, 0.5f);
                            yield return new WaitForSeconds(0.5f);
                            BattleUI.Instance.SetPlayerBattleUI();
                            p1 = new PlayerCreatureStats();
                            p2 = new PlayerCreatureStats();
                            yield return new WaitForSeconds(1f);
                            if (BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature].creatureStats.HP <= 0)
                            {
                                party = BattleController.Instance.TurnController.PlayerParty;
                                for (int k = 0; k < party.party.Length; k++)
                                {
                                    if (party.party[k].creatureStats.HP > 0)
                                    {
                                        BattleUI.Instance.CurrentMenuStatus = MenuStatus.SelectNewCreaturePostDeath;
                                    }
                                }
                                if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
                                {
                                    yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + party.party[party.selectedCreature].creatureSO.creatureName + "</b>" + " has returned to the void!", BattleUI.Instance.DialogueBox.Dialogue, 1f, true));
                                    BattleController.Instance.Player1CreatureImage.transform.DOScale(Vector3.zero, 0.5f);
                                    yield return new WaitForSeconds(0.75f);
                                    yield return StartCoroutine(BattleUI.Instance.SelectNewCreatureAfterDeath());
                                    BattleUI.Instance.SetPlayerBattleUI();
                                    p1 = new PlayerCreatureStats();
                                    p2 = new PlayerCreatureStats();
                                    Turncount = 2;
                                    deathFinished = true;
                                }
                            }
                            Turncount = 2;
                            break;
                        }
                        else
                        {
                            firstAttackerAlreadySet = false;
                            Turncount = 2;
                            turnedEnded = true;
                            yield return null;
                        }
                        if (i == party.party.Length - 1)
                        {
                            ReturnImage(party.party[i]).transform.DOScale(Vector3.zero, 0.5f);
                            yield return new WaitForSeconds(0.5f);
                            StartRewardsScreen();
                            fightEnded = true;
                            Debug.Log("no party left");
                        }
                    }
                }
            }
        }
        if (Turncount >= 2)
        {
            while (!deathFinished)
                yield return new WaitForEndOfFrame();
            Debug.Log("Turn Ended");
            Turncount = 0;
            turnedEnded = true;
            BattleUI.Instance.DialogueBox.gameObject.SetActive(false);
            firstAttackerAlreadySet = false;
            if (!fightEnded)
            {
                Debug.Log("Fight Not Ended");
                yield return StartCoroutine(BattleUI.OpenMenu(BattleUI.Instance.PlayerOptions.gameObject, 0, 0.25f));
            }
            else {
                Debug.Log("no party left");
                StartRewardsScreen();
            }
        }

        if (!turnedEnded)
        {
            BattleUI.Instance.DialogueBox.gameObject.SetActive(true);

            if (!firstAttackerAlreadySet)
            {
                playerAbilityIndex = abilityIndex;
                firstAttackerAlreadySet = true;
                BattleController.Instance.TurnController.SetFirstAttacker();
                Debug.Log("first attacker set");
            }

            if (!BattleController.Instance.TurnController.PlayerFirst)
            {
                BattleUI battleUI = BattleController.Instance.BattleUI;
                Turncount++;
                BattleController.Instance.TurnController.PlayerFirst = true;
                Debug.Log("Enemy Attack");
                p1 = BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature];
                p2 = BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature];
                int enemyAttackIndex = ChooseEnemyAction(p1);
                ability = p1.creatureAbilities[enemyAttackIndex].ability;

                yield return StartCoroutine(PreBattleAilmentCheck(p1, false));

                if (canAttack)
                {
                    string dialogueText = "<b>" + p1.creatureSO.creatureName + "</b>" + " uses " + ability.abilityName +".";
                    yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, true));
                    bool attackHit = true;
                    bool chanceHit = false;
                    if (ability.type == AbilityType.Attack || ability.type == AbilityType.Debuff || ability.type == AbilityType.Buff)
                    {
                        attackHit = HasAttackHit(p1, p2, ability);
                        chanceHit = RandomChance(ability.abilityStats.percentage);
                    }
                    if (attackHit)
                    {
                        //Loop through animations and play them
                        for (int i = 0; i < ability.animations.Count; i++)
                        {
                            //if (i == 1)
                            //{
                            //    if (chanceHit == false && ability.type == AbilityType.Debuff || chanceHit == false && ability.type == AbilityType.Buff)
                            //        break;
                            //}

                            yield return StartCoroutine(CheckAnimationRequirement(p1, p2, ability.animations[i]));
                            Debug.Log("Enemy Animation Ended");
                        }
                        if (ability.type == AbilityType.Attack)
                        {
                            List<string> strings = new List<string>();
                            strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Attack, false);
                            BattleUI.Instance.SetPlayerBattleUI();
                            
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            
                        }
                        else if (ability.type == AbilityType.Debuff)
                        {
                            if (ability.abilityStats.power > 0)
                            {
                                List<string> strings = new List<string>();
                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Attack, false);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }

                            if (chanceHit)
                            {
                                List<string> strings = new List<string>();
                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Debuff, true);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            else if (ability.abilityStats.power <= 0 && !chanceHit)
                            {
                                dialogueText = "but if failed!";
                                yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                        }
                        else if (ability.type == AbilityType.Buff)
                        {
                            if (ability.abilityStats.power > 0)
                            {
                                List<string> strings = new List<string>();
                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Attack, false);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            if (chanceHit)
                            {
                                List<string> strings = new List<string>();
                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Buff, false);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            else if (ability.abilityStats.power <= 0 && !chanceHit)
                            {
                                dialogueText = "but if failed!";
                                yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                        }
                    }
                    else
                    {
                        dialogueText = "Attack has missed";
                        yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));
                    }
                }
                if (attackSelf)
                {
                    string dialogueText = "<b>" + p1.creatureSO.creatureName + "</b>" + " attacks itself in confusion.";
                    yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));

                    Ability attackSelfAbility = new Ability(true, p1.creatureSO.primaryElement);
                    //Loop through animations and play them
                    for (int i = 0; i < attackSelfAbility.animations.Count; i++)
                    {
                        yield return StartCoroutine(CheckAnimationRequirement(p1, p1, attackSelfAbility.animations[i]));
                        Debug.Log("Player Animation Ended");
                    }
                    List<string> strings = new List<string>();

                    strings = UseBattleAction(playerAbilityIndex, p1, p1, attackSelfAbility, AbilityType.AttackSelf, false);
                    BattleUI.Instance.SetPlayerBattleUI();
                    yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                    attackSelf = false;
                }

                yield return StartCoroutine(PostBattleAilmentCheck(p1));
                //Wait 1 Second after action is finished
                canAttack = true;

                p1.CheckIfDead();
                if (co != null)
                {
                    StopCoroutine(co);
                }

                co = StartCoroutine(AttackPlayer(playerAbilityIndex));
            }
            else if (BattleController.Instance.TurnController.PlayerFirst)
            {
                BattleUI battleUI = BattleController.Instance.BattleUI;
                Turncount++;
                BattleController.Instance.TurnController.PlayerFirst = false;
                Debug.Log("Player Attack");
                p1 = BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature];
                p2 = BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature];
                ability = p1.creatureAbilities[playerAbilityIndex].ability;

                yield return StartCoroutine(PreBattleAilmentCheck(p1, true));

                if (canAttack)
                {
                    string dialogueText = "<b>" + p1.creatureSO.creatureName + "</b>" + " uses " + ability.abilityName + ".";
                    yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, true));
                    bool attackHit = true;
                    bool chanceHit = true;
                    if (ability.type == AbilityType.Attack || ability.type == AbilityType.Debuff || ability.type == AbilityType.Buff)
                    {
                        attackHit = HasAttackHit(p1, p2, ability);
                        chanceHit = RandomChance(ability.abilityStats.percentage);
                    }
                    if (attackHit)
                    {
                        //Loop through animations and play them
                        for (int i = 0; i < ability.animations.Count; i++)
                        {
                            //if (i == 1) {
                            //    if (chanceHit == false && ability.type == AbilityType.Debuff || chanceHit == false && ability.type == AbilityType.Buff)
                            //        break;
                            //}
                            yield return StartCoroutine(CheckAnimationRequirement(p1, p2, ability.animations[i]));
                            Debug.Log("Player Animation Ended");
                        }

                        if (ability.type == AbilityType.Attack)
                        {
                            List<string> strings = new List<string>();
                            strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Attack, false);
                            BattleUI.Instance.SetPlayerBattleUI();
                            yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                        }
                        else if (ability.type == AbilityType.Debuff)
                        {

                            if (ability.abilityStats.power > 0)
                            {
                                Debug.Log("Attack");
                                List<string> strings = new List<string>();
                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Attack, false);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            if (chanceHit)
                            {
                                Debug.Log("Debuff");
                                List<string> strings = new List<string>();
                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Debuff, false);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            else if (ability.abilityStats.power <= 0 && !chanceHit) {
                                dialogueText = "but if failed!";
                                yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                           
                        }
                        else if (ability.type == AbilityType.Buff)
                        {

                            if (ability.abilityStats.power > 0)
                            {
                                List<string> strings = new List<string>();

                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Attack, true);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            if (chanceHit)
                            {
                                List<string> strings = new List<string>();

                                strings = UseBattleAction(playerAbilityIndex, p1, p2, ability, AbilityType.Buff, true);
                                BattleUI.Instance.SetPlayerBattleUI();
                                yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                            else if (ability.abilityStats.power <= 0 && !chanceHit)
                            {
                                dialogueText = "but if failed!";
                                yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));
                            }
                        }
                    }
                    else
                    {
                        dialogueText = "Attack has missed";
                        yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));
                    }
                }
                if (attackSelf)
                {
                    string dialogueText = "<b>" + p1.creatureSO.creatureName + "</b>" + " attacks itself in confusion.";
                    yield return StartCoroutine(battleUI.TypeDialogue(dialogueText, battleUI.DialogueBox.Dialogue, 1f, false));

                    Ability attackSelfAbility = new Ability(true, p1.creatureSO.primaryElement);
                    //Loop through animations and play them
                    for (int i = 0; i < attackSelfAbility.animations.Count; i++)
                    {
                        yield return StartCoroutine(CheckAnimationRequirement(p1, p1, attackSelfAbility.animations[i]));
                        Debug.Log("Player Animation Ended");
                    }
                    List<string> strings = new List<string>();

                    strings = UseBattleAction(playerAbilityIndex, p1, p1, attackSelfAbility, AbilityType.AttackSelf, true);
                    BattleUI.Instance.SetPlayerBattleUI();
                    yield return StartCoroutine(battleUI.TypeDialogue(strings, battleUI.DialogueBox.Dialogue, 1f, false));
                    attackSelf = false;
                }

                yield return StartCoroutine(PostBattleAilmentCheck(p1));
                //Wait 1 Second after action is finished
                yield return new WaitForSeconds(0.25f);
                canAttack = true;

                if (co != null)
                {
                    StopCoroutine(co);
                }

                co = StartCoroutine(StartEnemyAttack(0));
            }
        }

        else if (turnedEnded)
        {
            turnedEnded = false;
        }
    }

    private void StartRewardsScreen()
    {
        BattleUI.Instance.RewardsScreen.transform.localScale = Vector3.one;
        BattleUI.Instance.RewardsScreen.gameObject.SetActive(true);
        StartCoroutine(BattleUI.Instance.RewardsScreen.StartVictoryScreen());
    }

    private bool RandomChance(float percentage)
    {
        float rnd = UnityEngine.Random.Range(0, 100);

        if (rnd < percentage)
        {
            return true;
        }
        else {
            return false;
        }
    }

    private bool HasAttackHit(PlayerCreatureStats p1, PlayerCreatureStats p2, Ability ability)
    {

        if (ability.type == AbilityType.Buff || ability.type == AbilityType.Debuff) {

        }

        float t = ability.abilityStats.accuracy * (p1.creatureStats.BattleAccuracy / 100f) / (p2.creatureStats.BattleDodge / 100f);
        int r = UnityEngine.Random.Range(0, 100);

        if (r <= t)
        {
            Debug.Log(p2.creatureSO.creatureName + " has been Hit True, Hit: " + t + " Random Number: " + r);
            return true;
        }
        else
        {
            Debug.Log(p2.creatureSO.creatureName + " Hit False, Hit: " + t + " Random Number: " + r);
            return false;
        }
    }
    // Defines the priority of the AI. Make it simple
    private int ChooseEnemyAction(PlayerCreatureStats p1)
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < p1.creatureAbilities.Length; i++) {

            if (p1.creatureAbilities[i].ability != null) {
                indexes.Add(i);
            }
        }
        return indexes[UnityEngine.Random.Range(0, indexes.Count)];
    }
    //Method to use the action required in the ability. Needs to be expanded to include Plug and Play effects
    private List<string> UseBattleAction(int abilityIndex, PlayerCreatureStats p1, PlayerCreatureStats p2, Ability ability, AbilityType abilityType, bool isPlayer)
    {
        List<string> strings = new List<string>();
        if (abilityType == AbilityType.Attack)
        {
            string actionText = "";
            string critText = "";
            p2.creatureStats.HP -= CalculateDamage(p1, p2, ability, out actionText, out critText);

            if (p2.creatureStats.HP <= 0)
            {
                p2.creatureStats.HP = 0;
            }
            p1.creatureAbilities[abilityIndex].remainingCount--;
            if (p1.creatureAbilities[abilityIndex].remainingCount <= 0)
            {
                p1.creatureAbilities[abilityIndex].remainingCount = 0;
            }
            BattleUI.Instance.PlayerOptions.AttackOptions.attackButtonUIs[abilityIndex].UpdateText(ability, p1.creatureAbilities[abilityIndex].remainingCount);

            if (critText != "")
            {
                strings.Add(critText);
            }
            if (actionText != "") {
                strings.Add(actionText);
            }
           
            return strings;
        }
        else if (abilityType == AbilityType.Buff)
        {
            string actionText = "";
            if (CheckForBuffToList(ability.positiveAilment))
            {
                p1.ailments.Add(AddAilment(ability.positiveAilment, ability.negativeAilment, out actionText, p1.ailments, isPlayer));
                p1.creatureAbilities[abilityIndex].remainingCount--;
            }
            else
            {
                ApplyPermanentBuffAndDebuff(p1, ability, out actionText);
                p1.creatureAbilities[abilityIndex].remainingCount--;
            }
            if (actionText.Contains("{p1}"))
            {
                actionText = actionText.Replace("{p1}", "<b>" + p1.creatureSO.creatureName + "</b>");
            }
            if (actionText.Contains("{p2}"))
            {
                actionText = actionText.Replace("{p2}", "<b>" + p2.creatureSO.creatureName + "</b>");
            }
            if (actionText != "")
            {
                strings.Add(actionText);
            }
            return strings;
        }
        else if (abilityType == AbilityType.Debuff)
        {
            string actionText = "";
            if (CheckForDebuffToList(ability.negativeAilment))
            {
                p2.ailments.Add(AddAilment(ability.positiveAilment, ability.negativeAilment, out actionText, p2.ailments, isPlayer));
                p1.creatureAbilities[abilityIndex].remainingCount--;
            }
            else
            {
                ApplyPermanentBuffAndDebuff(p2, ability, out actionText);
                p1.creatureAbilities[abilityIndex].remainingCount--;
            }
            if (actionText.Contains("{p1}"))
            {
                actionText = actionText.Replace("{p1}", "<b>" + p1.creatureSO.creatureName + "</b>");
            }
            if (actionText.Contains("{p2}"))
            {
                actionText = actionText.Replace("{p2}", "<b>" + p2.creatureSO.creatureName + "</b>");
            }

            if (actionText != "")
            {
                strings.Add(actionText);
            }
            return strings;         }
        else if (abilityType == AbilityType.Other)
        {
            //TODO
        }
        else if (abilityType == AbilityType.Weather)
        {
            //TODO
        }
        else if (abilityType == AbilityType.AttackSelf)
        {
            
            string actionText = "";
            string critText = ""; 
            p1.creatureStats.HP -= CalculateDamage(p1, p2, ability, out actionText, out critText);

            if (p1.creatureStats.HP <= 0)
            {
                p1.creatureStats.HP = 0;
            }
            if (p1.creatureAbilities[abilityIndex].remainingCount <= 0)
            {
                p1.creatureAbilities[abilityIndex].remainingCount = 0;
            }

            if (critText != "")
            {
                strings.Add(critText);
            }
            if (actionText != "")
            {
                strings.Add(actionText);
            }
            return strings;
        }
        return strings;
    }
    private IEnumerator PreBattleAilmentCheck(PlayerCreatureStats p1, bool isPlayer)
    {

        AnimationController ac = BattleController.Instance.AnimationController;
        canAttack = true;
        bool breakLoop = false;
        for (int i = 0; i < p1.ailments.Count; i++)
        {
            if (breakLoop) {
                Debug.Log("break loop");
            }
            else if (p1.ailments[i] is Shocked || p1.ailments[i] is Confused || p1.ailments[i] is Sleep || p1.ailments[i] is Frozen)
            {
                if (p1.ailments[i] is Sleep)
                {
                    yield return StartCoroutine(ac.Sleep(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                    yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " is asleep.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                    int rnd = UnityEngine.Random.Range(0, 100);
                    canAttack = false;
                    if (rnd < 25)
                    {
                        yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " wakes up.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                        if (isPlayer)
                            BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                        else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                        p1.ailments.RemoveAt(i);
                        canAttack = true;
                        break;
                    }
                    breakLoop = true;
                    break;

                }              
                else if (p1.ailments[i] is Shocked)
                {
                    int rnd = UnityEngine.Random.Range(0, 100);

                    if (rnd < 25)
                    {
                        yield return StartCoroutine(ac.Shocked(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                        yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " is shocked.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                        canAttack = false;

                        //Check if 25% and break out of shocked
                        int rnd2 = UnityEngine.Random.Range(0, 100);
                        if (rnd2 < 5)
                        {
                            yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " is no longer shocked.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                            canAttack = true;
                            if (isPlayer)
                                BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                            else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                            p1.ailments.RemoveAt(i);
                        }
                        breakLoop = true;
                        break;
                    }
                }
                else if (p1.ailments[i] is Frozen)
                {
                    int rnd = UnityEngine.Random.Range(0, 100);

                    if (rnd < 75)
                    {
                        Debug.Log("Frozen");
                        yield return StartCoroutine(ac.Frozen(ReturnImage(p1).transform, ReturnImage(p2).transform, ReturnImage(p1), 1f, 1f));
                        yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " is frozen.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                        canAttack = false;

                        bool dealDamage = true;
                        int rnd2 = UnityEngine.Random.Range(0, 100);
                        if (CheckIfNegativeAilment(p1.ailments, NegativeAilment.Burnt))
                        {
                            rnd2 = UnityEngine.Random.Range(0, 10);
                        }
                        if (rnd2 < 5)
                        {
                            yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " is no longer frozen.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                            dealDamage = false;
                            canAttack = true;
                            if (isPlayer)
                                BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                            else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                            p1.ailments.RemoveAt(i);
                        }
                        if (dealDamage) {
                            p1.creatureStats.HP -= (int)(p1.creatureStats.MaxHP * 0.1f);
                            BattleUI.Instance.SetPlayerBattleUI();
                            yield return new WaitForSeconds(1f);
                        }
                        breakLoop = true;
                        break;
                    }
                }
                else if (p1.ailments[i] is Confused)
                {
                    int rnd = UnityEngine.Random.Range(0, 100);

                    yield return StartCoroutine(ac.Sleep(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                    yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" + " is confused.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                    if (rnd < 50)
                    {
                        canAttack = false;
                        yield return StartCoroutine(ac.Confused(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                        attackSelf = true;

                        int rnd2 = UnityEngine.Random.Range(0, 100);
                        if (rnd2 < 5)
                        {
                            yield return StartCoroutine(BattleUI.Instance.TypeDialogue("<b>" + p1.creatureSO.creatureName + "</b>" +  " is no longer confused.", BattleUI.Instance.DialogueBox.Dialogue, 1f, false));
                            canAttack = true;
                            attackSelf = false;
                            if (isPlayer)
                                BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                            else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(p1.ailments[i]).SetActive(false);
                            p1.ailments.RemoveAt(i);
                            breakLoop = true;
                        }
                        break;
                    }
                }
            }
        }
        BattleUI.Instance.DialogueBox.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
    }

    private bool CheckIfNegativeAilment(List<Ailment> ailments, NegativeAilment negativeAilment)
    {
        foreach (Ailment a in ailments)
        {
            if (a is Burnt)
            {
                if (negativeAilment == NegativeAilment.Burnt)
                    return true;
            }
            else if (a is Poison)
            {
                if (negativeAilment == NegativeAilment.Poisoned)
                    return true;
            }
            else if (a is Frozen)
            {
                if (negativeAilment == NegativeAilment.Frozen)
                    return true;
            }
            else if (a is Shocked)
            {
                if (negativeAilment == NegativeAilment.Shocked)
                    return true;
            }
            else if (a is Confused)
            {
                if (negativeAilment == NegativeAilment.Confused)
                    return true;
            }
            else if (a is Sleep)
            {
                if (negativeAilment == NegativeAilment.Sleep)
                    return true;
            }
            else if (a is Ethereal)
            {
                if (negativeAilment == NegativeAilment.Etheral)
                    return true;
            }
        }
        return false;
    }

        private IEnumerator PostBattleAilmentCheck(PlayerCreatureStats p1)
    {
        AnimationController ac = BattleController.Instance.AnimationController;
        for (int i = 0; i < p1.ailments.Count; i++)
        {
            if (p1.ailments[i] is Poison || p1.ailments[i] is Burnt || p1.ailments[i] is Ethereal)
            {
                if (p1.ailments[i] is Poison)
                {
                    yield return StartCoroutine(ac.Poison(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                    p1.creatureStats.HP -= (int)(p1.creatureStats.MaxHP * 0.16f);
                    BattleUI.Instance.SetPlayerBattleUI();
                }
                else if (p1.ailments[i] is Burnt)
                {
                    yield return StartCoroutine(ac.Burn(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                    p1.creatureStats.HP -= (int)(p1.creatureStats.MaxHP * 0.10f);
                    BattleUI.Instance.SetPlayerBattleUI();
                    Debug.Log("Burnt");
                }
                else if (p1.ailments[i] is Ethereal)
                {
                    //TODO
                    if (p1.creatureStats.BattleStrength != p1.creatureStats.strength / 2)
                    {
                        yield return StartCoroutine(ac.Ethereal(ReturnImage(p1).transform, ReturnImage(p1), 1f, 1f));
                        p1.creatureStats.BattleStrength = p1.creatureStats.strength / 2;
                        BattleUI.Instance.SetPlayerBattleUI();
                        Debug.Log("Ethereal");
                    }
                    else {
                        Debug.Log("Already Ethereal");
                    }
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }
    private bool CheckForDebuffToList(NegativeAilment negativeAilment)
    {

        if (negativeAilment == NegativeAilment.Poisoned || negativeAilment == NegativeAilment.Shocked || negativeAilment == NegativeAilment.Frozen || negativeAilment == NegativeAilment.Etheral
            || negativeAilment == NegativeAilment.Confused || negativeAilment == NegativeAilment.Burnt || negativeAilment == NegativeAilment.Sleep)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckForBuffToList(PositiveAilment positiveAilment)
    {

        //if (positiveAilment == PositiveAilment.ATKp || positiveAilment == PositiveAilment.ATKpp || positiveAilment == PositiveAilment.DEFp || positiveAilment == PositiveAilment.DEFpp
        //    || positiveAilment == PositiveAilment.SPDp || positiveAilment == PositiveAilment.SPDpp)
        //{
        //    return true;
        //}
        //else return false;
        return false;
    }
    private Ailment AddAilment(PositiveAilment positiveAilment, NegativeAilment negativeAilment, out string s, List<Ailment> ailments, bool isPlayer)
    {
        if (positiveAilment != PositiveAilment.None)
        {
            switch (positiveAilment)
            {
                default:
                    break;
            }
        }
        else if (negativeAilment != NegativeAilment.None)
        {
            switch (negativeAilment)
            {
                case NegativeAilment.Burnt:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Burnt)
                        {
                            s = "{p2}" + " is already burnt!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been burnt!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Burnt).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Burnt).SetActive(true);

                    return new Burnt();
                case NegativeAilment.Shocked:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Shocked)
                        {
                            s = "{p2}" + "  is already shocked!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been shocked!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Shocked).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Shocked).SetActive(true);
                    return new Shocked();
                case NegativeAilment.Poisoned:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Poison)
                        {
                            s = "{p2}" + " is already poisoned!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been poisoned!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Poisoned).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Poisoned).SetActive(true);
                    return new Poison();
                case NegativeAilment.Frozen:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Frozen)
                        {
                            s = "{p2}" + " is already frozen!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been frozen!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Frozen).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Frozen).SetActive(true);
                    return new Frozen();
                case NegativeAilment.Confused:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Confused)
                        {
                            s = "{p2}" + " is already confused!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been confused!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Confused).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Confused).SetActive(true);
                    return new Confused();
                case NegativeAilment.Etheral:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Ethereal)
                        {
                            s = "{p2}" + " is already in the ethereal realm!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been sent to the ethereal realm!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Etheral).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Etheral).SetActive(true);
                    return new Ethereal();
                case NegativeAilment.Sleep:
                    foreach (Ailment a in ailments)
                    {
                        if (a is Sleep)
                        {
                            s = "{p2}" + " is already asleep!";
                            return null;
                        }
                    }
                    s = "{p2}" + " has been put to sleep!";
                    if (isPlayer)
                        BattleUI.Instance.PlayerStats[0].StatusEffectUI.ReturnElement(NegativeAilment.Sleep).SetActive(true);
                    else BattleUI.Instance.PlayerStats[1].StatusEffectUI.ReturnElement(NegativeAilment.Sleep).SetActive(true);
                    return new Sleep();
                case NegativeAilment.None:
                    break;

            }
        }
        s = "Should not go here";
        return null;
    }
    private void ApplyPermanentBuffAndDebuff(PlayerCreatureStats p1, Ability ability, out string actionText)
    {
        if (ability.positiveAilment != PositiveAilment.None)
        {
            switch (ability.positiveAilment)
            {
                case PositiveAilment.SpeedUp:
                    p1.creatureStats.BattleSpeed += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleSpeed * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " speed has increased!";
                    return;
                case PositiveAilment.SpeedUpUp:
                    p1.creatureStats.BattleSpeed += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleSpeed * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " speed has greatly increased!!";
                    return;
                case PositiveAilment.AttackUp:
                    p1.creatureStats.BattleStrength += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleStrength * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " strength has increased!";
                    return;
                case PositiveAilment.AttackUpUp:
                    p1.creatureStats.BattleStrength += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleStrength * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " strength has greatly increased!!";
                    return;
                case PositiveAilment.DefenceUp:
                    p1.creatureStats.BattleDefence += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDefence * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " defence has increased!";
                    return;
                case PositiveAilment.DefenceUpUp:
                    p1.creatureStats.BattleDefence += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDefence * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " defence has greatly increased!!";
                    return;
                case PositiveAilment.AccuracyUp:
                    p1.creatureStats.BattleAccuracy += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleAccuracy * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " accuracy has increased!";
                    return;
                case PositiveAilment.AccuracyUpUp:
                    p1.creatureStats.BattleAccuracy += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleAccuracy * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " accuracy has greatly increased!!";
                    return;
                case PositiveAilment.DodgeUp:
                    p1.creatureStats.BattleDodge += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDodge * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " dodge has increased!";
                    return;
                case PositiveAilment.DodgeUpUp:
                    p1.creatureStats.BattleDodge += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDodge * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " dodge has greatly increased!!";
                    return;
                case PositiveAilment.Heal:
                    actionText = "";
                    string critText = "";
                    int previousHp = p1.creatureStats.HP;
                    p1.creatureStats.HP += (int)(CalculateDamage(p1, p2, ability, out actionText, out critText) * (ability.abilityStats.percentage / 100));
                    p1.ClampHP();
                    actionText = "<b>" + p1.creatureSO.creatureName + "</b>" + " Healed for " + (p1.creatureStats.HP - previousHp) + " HP.";
                    return;
                case PositiveAilment.CritATKUp:
                    p1.creatureStats.BattleCritATK += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritATK * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " critical attack chance has increased!";
                    return;
                case PositiveAilment.CritATKUpUp:
                    p1.creatureStats.BattleCritATK += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritATK * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " critical attack chance has greatly increased!!";
                    return;
                case PositiveAilment.CritDEFUp:
                    p1.creatureStats.BattleCritDef += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritDef * 0.05f), 1, 255);
                    actionText = "{p1}'s" + " critical defence chance has increased!";
                    return;
                case PositiveAilment.CritDEFUpUp:
                    p1.creatureStats.BattleCritDef += Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritDef * 0.10f), 1, 255);
                    actionText = "{p1}'s" + " critical defence chance has greatly increased!!";
                    return;
            }
        }
        if (ability.negativeAilment != NegativeAilment.None)
        {
            switch (ability.negativeAilment)
            {
                case NegativeAilment.SpeedDown:
                    p1.creatureStats.BattleSpeed -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleSpeed * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " speed has decreased!";
                    return;
                case NegativeAilment.SpeedDownDown:
                    p1.creatureStats.BattleSpeed -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleSpeed * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " speed has greatly decreased!!";
                    return;
                case NegativeAilment.AttackDown:
                    p1.creatureStats.BattleStrength -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleStrength * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " strength has decreased!";
                    return;
                case NegativeAilment.AttackDownDown:
                    p1.creatureStats.BattleStrength -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleStrength * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " strength has greatly decreased!!";
                    return;
                case NegativeAilment.DefenceDown:
                    p1.creatureStats.BattleDefence -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDefence * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " defence has decreased!";
                    return;
                case NegativeAilment.DefenceDownDown:
                    p1.creatureStats.BattleDefence -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDefence * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " defence has greatly decreased!!";
                    return;
                case NegativeAilment.AccuracyDown:
                    p1.creatureStats.BattleAccuracy -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleAccuracy * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " accuracy has decreased!";
                    return;
                case NegativeAilment.AccuracyDownDown:
                    p1.creatureStats.BattleAccuracy -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleAccuracy * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " accuracy has greatly decreased!!";
                    return;
                case NegativeAilment.DodgeDown:
                    p1.creatureStats.BattleDodge -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDodge * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " dodge has decreased!";
                    return;
                case NegativeAilment.DodgeDownDown:
                    p1.creatureStats.BattleDodge -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleDodge * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " dodge has greatly decreased!!";
                    return;
                case NegativeAilment.CritATKDown:
                    p1.creatureStats.BattleCritATK -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritATK * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " critical attack chance has decreased!";
                    return;
                case NegativeAilment.CritATKDownDown:
                    p1.creatureStats.BattleCritATK -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritATK * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " critical attack chance has greatly decreased!!";
                    return;
                case NegativeAilment.CritDEFDown:
                    p1.creatureStats.BattleCritDef -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritDef * 0.05f), 1, 255);
                    actionText = "{p2}'s" + " critical defence chance has decreased!";
                    return;
                case NegativeAilment.CritDEFDownDown:
                    p1.creatureStats.BattleCritDef -= Mathf.Clamp(Mathf.RoundToInt(p1.creatureStats.BattleCritDef * 0.10f), 1, 255);
                    actionText = "{p2}'s" + " critical defence chance has greatly decreased!!";
                    return;
            }
        }
        actionText = "";
    }
    private IEnumerator StartEnemyAttack(int abilityIndex)
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(AttackPlayer(0));
    }
    private int CalculateDamage(PlayerCreatureStats p1, PlayerCreatureStats p2, Ability usedAbility, out string actionText, out string critText)
    {
        //Calculate the basic damage before modifier applied
        int damage = (((((2 * p1.creatureStats.level) / 5) + 2) * (usedAbility.abilityStats.power * p1.creatureStats.BattleStrength / p2.creatureStats.BattleDefence) / 50) + 2);

        float modifier = 1 * 1 * GetCriticalHit(p1, p2, usedAbility, out critText) * UnityEngine.Random.Range(0.70f, 1f) * SameType(p1, usedAbility) * EnemyType(p2, usedAbility, out actionText);
        int finalDamage = Mathf.RoundToInt(damage * modifier);
        Debug.Log("Damage: " + damage + " Modifier: " + modifier + " Final Damage: " + finalDamage);
        return finalDamage;
    }
    private float GetCriticalHit(PlayerCreatureStats p1, PlayerCreatureStats p2, Ability usedAbility, out string critText)
    {
        string s = "";
        //TODO get Critical Hit Info.
        int random = UnityEngine.Random.Range(0, (256 + p1.creatureStats.criticalAttack) - p2.creatureStats.criticalDefence);
        float critModifier = 1;

        if (random > 256)
        {
            s = "<b>" + p1.creatureSO.creatureName + "</b>" + " landed a critical hit!!";
            critModifier = 2;
        }
        else
        {
            s = "";
            critModifier = 1;
        }
        critText = s;
        return critModifier;
    }
    private float SameType(PlayerCreatureStats p1, Ability usedAbility)
    {

        float typeModifier = 1;

        if (usedAbility.elementType == p1.creatureSO.primaryElement)
        {
            return typeModifier = 1.5f;
        }
        else if (usedAbility.elementType == p1.creatureSO.secondaryElement)
        {
            return typeModifier = 1.25f;
        }
        else
        {
            return typeModifier;
        }
    }
    private float EnemyType(PlayerCreatureStats p2, Ability usedAbility, out string s)
    {
        float typeModifier = 1;

        if (BattleController.Instance.ElementMatrix.ReturnImpactType(p2, usedAbility) == ElementImpactType.Normal)
        {
            s = "It was a normal attack.";
            return typeModifier = 1f;
        }
        else if (BattleController.Instance.ElementMatrix.ReturnImpactType(p2, usedAbility) == ElementImpactType.NotEffective)
        {
            s = "This was not effective.";
            return typeModifier = 0f;
        }
        else if (BattleController.Instance.ElementMatrix.ReturnImpactType(p2, usedAbility) == ElementImpactType.VeryWeak)
        {
            s = "It was a very weak attack.";
            return typeModifier = 0.25f;
        }
        else if (BattleController.Instance.ElementMatrix.ReturnImpactType(p2, usedAbility) == ElementImpactType.Weak)
        {
            s = "It was a weaker than normal attack.";
            return typeModifier = 0.5f;
        }
        else if (BattleController.Instance.ElementMatrix.ReturnImpactType(p2, usedAbility) == ElementImpactType.Crit)
        {
            s = "It was a effective attack.";
            return typeModifier = 1.5f;
        }
        else if (BattleController.Instance.ElementMatrix.ReturnImpactType(p2, usedAbility) == ElementImpactType.MegaCrit)
        {
            s = "It was a mega effective attack.";
            return typeModifier = 2f;
        }
        else
        {
            s = "Should Not Get Here ";
            return typeModifier;
        }
    }
    private IEnumerator CheckAnimationRequirement(PlayerCreatureStats self, PlayerCreatureStats target, AnimationDetail animationDetail)
    {
        AnimationController ac = BattleController.Instance.AnimationController;

        switch (animationDetail.animation)
        {
            case ImageAnimation.None:
                break;
            case ImageAnimation.MoveHorizontal:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.MoveHorizontal(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.MoveHorizontal(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.MoveHorizontalAndRotationToRight:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.MoveHorizontalAndRotationToRight(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.MoveHorizontalAndRotationToRight(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.MoveVertical:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.MoveVertical(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.MoveVertical(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.MoveVerticalBounce:
                break;
            case ImageAnimation.Swirl:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Swirl(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Swirl(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Circle:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Circle(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Circle(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.RandomPositions:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.RandomPositions(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.RandomPositions(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.TakeDamage:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.TakeDamage(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.TakeDamage(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.TakeDamageShakeNoFade:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.TakeDamageShakeNoFade(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.TakeDamageShakeNoFade(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Glimmer:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Glimmer(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Glimmer(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Burn:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Burn(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Burn(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Poison:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Poison(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Poison(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Frozen:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Frozen(ReturnImage(self).transform, ReturnImage(target).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Frozen(ReturnImage(target).transform, ReturnImage(self).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Sleep:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Sleep(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Sleep(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Confused:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Confused(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Confused(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Shocked:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.Shocked(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.Shocked(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.Rampage:
                break;
            case ImageAnimation.SpawnAnimSprite:
                yield return StartCoroutine(ac.SpawnAnimSprite(ReturnImage(self).transform, ReturnImage(target).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay, animationDetail, animationDetail.SkipCoroutineWait));
                break;
            case ImageAnimation.Ethereal:
                break;
            case ImageAnimation.RotationSideToSide:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.RotationSideToSide(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.RotationSideToSide(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.SpawnAnimSpriteAsBackground:
                yield return StartCoroutine(ac.SpawnAnimSpriteAsBackground(ReturnImage(self).transform, ReturnImage(target).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay, animationDetail, animationDetail.SkipCoroutineWait));
                break;
            case ImageAnimation.MoveHorizontalBackwards:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.MoveHorizontalBackwards(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.MoveHorizontalBackwards(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
            case ImageAnimation.MultipleShake:
                if (animationDetail.targetType == TargetType.Self)
                {
                    yield return StartCoroutine(ac.MultipleShake(ReturnImage(self).transform, ReturnImage(self), animationDetail.duration, animationDetail.delay));
                }
                else if (animationDetail.targetType == TargetType.Target)
                {
                    yield return StartCoroutine(ac.MultipleShake(ReturnImage(target).transform, ReturnImage(target), animationDetail.duration, animationDetail.delay));

                }
                break;
        }
        yield return null;
    }
    public Image ReturnImage(PlayerCreatureStats playerCreatureStats)
    {

        for (int i = 0; i < BattleController.Instance.TurnController.PlayerParty.party.Length; i++)
        {

            if (playerCreatureStats == BattleController.Instance.TurnController.PlayerParty.party[i])
            {
                return BattleController.Instance.Player1CreatureImage;
            }
        }
        return BattleController.Instance.Player2CreatureImage;
    }
    public void SwitchPlayerCreature(int index)
    {
        StartCoroutine(SwitchCreature(index));
    }
    public IEnumerator SwitchCreature(int index)
    {
        if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.Normal || BattleUI.Instance.CurrentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
        {

            if (BattleController.Instance.TurnController.PlayerParty.party[index].creatureStats.HP <= 0)
            {
                Debug.Log("Dead");
                yield return null;
            }
            else if (BattleController.Instance.TurnController.PlayerParty.selectedCreature == index)
            {
                Debug.Log("already selected");
                yield return null;
            }
            else if (BattleController.Instance.TurnController.PlayerParty.party[index].creatureSO == null)
            {
                Debug.Log("no party member");
                yield return null;
            }
            else
            {
                if (BattleUI.Instance.CurrentMenuStatus != MenuStatus.SelectNewCreaturePostDeath)
                {
                    Debug.Log("Switching Creature");
                    StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsBattle());
                    yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[0]));
                    BattleController.Instance.Player1CreatureImage.transform.DOScale(Vector3.zero, 0.1f);
                    BattleUI.DoFadeOut(BattleController.Instance.Player1CreatureImage.gameObject, 0.5f);
                    yield return new WaitForSeconds(0.5f);
                }
                else {
                    Debug.Log("Switching Creature 2");
                    yield return BattleUI.CloseMenu(BattleUI.Instance.PlayerOptions.gameObject, 0, 0);
                    yield return StartCoroutine(BattleUI.Instance.PlayerOptions.PartyOptions.OnMenuBackwardsIgnoreMenuStatus());
                    yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[0]));
                }

                BattleController.Instance.TurnController.PlayerParty.selectedCreature = index;
                BattleUI.Instance.SetPlayerBattleUI();

                Vector3 returnDirection = Vector3.one;
  

                BattleController.Instance.Player1CreatureImage.sprite = BattleController.Instance.TurnController.PlayerParty.party[index].creatureSO.creaturePlayerIcon;
                BattleController.Instance.Player1CreatureImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BattleController.Instance.TurnController.PlayerParty.party[index].creatureSO.width);
                BattleController.Instance.Player1CreatureImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BattleController.Instance.TurnController.PlayerParty.party[index].creatureSO.height);
                yield return StartCoroutine(BattleUI.OpenPortal(BattleUI.Instance.portals[0]));
                BattleController.Instance.Player1CreatureImage.transform.DOScale(returnDirection, 0.5f);
                BattleUI.DoFadeIn(BattleController.Instance.Player1CreatureImage.gameObject, 0.5f);
                yield return new WaitForSeconds(0.5f);

                turncount++;
                firstAttackerAlreadySet = true;
                BattleController.Instance.TurnController.PlayerFirst = false;
                if (BattleUI.Instance.CurrentMenuStatus != MenuStatus.SelectNewCreaturePostDeath)
                    StartCoroutine(StartEnemyAttack(0));
                BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
            }
        }
        else
        {
            yield return null;
        }
    }
    public IEnumerator SkipPlayerAttack()
    {
        Turncount++;
        firstAttackerAlreadySet = true;
        BattleController.Instance.TurnController.PlayerFirst = false;
        StartCoroutine(StartEnemyAttack(0));
        yield return null;
    }
}