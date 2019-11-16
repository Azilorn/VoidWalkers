using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System;

public class PlayerParty : MonoBehaviour
{
    public int selectedCreature;

    [Dropdown("InspectorValue")]
    private int[] inspectorValue = new int[] { 0, 1, 2, 3, 4, 5 };
    
    public PlayerCreatureStats[] party;

    public int[] InspectorValue { get => inspectorValue; set => inspectorValue = value; }

    public void LevelUp(int index)
    {
        party[index].SetLevel(party[index].creatureStats.level + 1, true);
    }
    public void LevelDown(int index)
    {
        party[index].SetLevel(party[index].creatureStats.level - 1, true);
    }

    public void UpdateStats(int index)
    {
      party[index].SetLevel(party[index].creatureStats.level, true);
    }
    [Button("Set Max Count")]
    public void SetMaxCount()
    {
        for (int i = 0; i < party.Length; i++)
        {
            for (int l = 0; l < party[i].creatureAbilities.Length; l++) {
                if(party[i].creatureAbilities[l].ability != null)
                    party[i].creatureAbilities[l].remainingCount = party[i].creatureAbilities[l].ability.abilityStats.maxCount;
            }
            party[i].creatureStats.Xp = XPMatrix.xpLevelList[party[i].creatureStats.level];
        }
    }
    public void AddToParty(PlayerCreatureStats playerCreatureStats, int partyPosition) {
        party[partyPosition] = playerCreatureStats;
    }
    public void RemoveFromParty(int partyPosition) {
        party[partyPosition] = null;
    }
    public void ReArrangeParty(int firstSelected, int lastSelected) {
        PlayerCreatureStats f = party[firstSelected];
        PlayerCreatureStats l = party[lastSelected];

        party[firstSelected] = l;
        party[lastSelected] = f;
    }
    public PlayerCreatureStats ReturnCreatureStats(int index) {

        return party[index];
    }
}
