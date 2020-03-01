using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewCreatureToPartyUI : MonoBehaviour
{
    [SerializeField] private AccountRewardUI[] creatures;

    public AccountRewardUI[] CreatureImages { get => creatures; set => creatures = value; }

    public void SetUI(CreatureSO[] creatureSO) {

        for (int i = 0; i < creatures.Length; i++) {
            creatures[i].creature = creatureSO[i];
            creatures[i].creatureName.text = creatureSO[i].creatureName;
            CreatureImages[i].icon.sprite = creatures[i].creature.creaturePlayerIcon;
        }
    }
    public void AddToParty(int index) {

        var party = BattleController.Instance.MasterPlayerParty.party;

        int partySlot = PreBattleSelectionController.Instance.GameDetails.Floor + 3;

        BattleController.Instance.MasterPlayerParty.party = new PlayerCreatureStats[partySlot];
        BattleController.Instance.MasterPlayerParty.party[partySlot - 1] = new PlayerCreatureStats();
        BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureSO = creatures[index].creature;
        BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureStats = new CreatureStats();
        BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureAbilities = new CreatureAbility[4];
        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureAbilities.Length; i++)
        {
            BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureAbilities[i] = new CreatureAbility();
            BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureAbilities[i].ability = null;
        }
        for (int i = 0; i < BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureSO.startingAbilities.Count; i++)
        {
            BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureAbilities[i].ability = BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureSO.startingAbilities[i];
            BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureAbilities[i].remainingCount = BattleController.Instance.MasterPlayerParty.party[partySlot - 1].creatureSO.startingAbilities[i].abilityStats.maxCount;
        }

        switch (PreBattleSelectionController.Instance.GameDetails.Floor) {

            case 1:
               
                BattleController.Instance.MasterPlayerParty.party[partySlot - 1].SetLevel(10, true);
                break;
            case 2:
                BattleController.Instance.MasterPlayerParty.party[partySlot- 1].SetLevel(20, true);
                break;
            case 3:
                BattleController.Instance.MasterPlayerParty.party[partySlot - 1].SetLevel(30, true);
                break;
        }

        for (int i = 0; i < partySlot - 1; i++) {

            BattleController.Instance.MasterPlayerParty.party[i] = party[i];
        }

    }
}
