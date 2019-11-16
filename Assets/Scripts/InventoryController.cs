using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    public List<Item> gameItems = new List<Item>();
    public List<RelicSO> relics = new List<RelicSO>();
    public List<Ability> abilities = new List<Ability>();

    public Dictionary<int, int> ownedItems = new Dictionary<int, int>();
    public Dictionary<int, bool> ownedRelics = new Dictionary<int, bool>();
    public Dictionary<int, int> ownedAbilities = new Dictionary<int, int>();

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
    public void Start()
    {
        GameObject go = Resources.Load("AbilityTable") as GameObject;
        AbilityTable at = go.GetComponent<AbilityTable>();
        abilities = at.Abilities;

        if (!CoreGameInformation.isLoadedGame)
        {
            for (int i = 0; i < gameItems.Count; i++)
                ownedItems.Add(i, 1);
            //for (int i = 0; i < relics.Count; i++)
            //    ownedRelics.Add(i, true);
        }
    }
    public RelicSO ReturnRelic(int i)
    {
        return relics[i];
    }
    public int ReturnRelic(RelicSO relic) {

        int relicInt = 0;
        for (int i = 0; i < relics.Count; i++)
        {
            if (relic == relics[i])
            {
                relicInt = i;
                return relicInt;
            }
        }
        return relicInt = 0;
    }
    public Ability ReturnAbility(int i) {
        return abilities[i];
    }
    public int ReturnAbility(Ability a) {
        int abilityInt = 0;
        for (int i = 0; i < abilities.Count; i++)
        {
            if (a == abilities[i])
            {
                abilityInt = i;
                return abilityInt;
            }
        }
        return abilityInt = 0;
    }
    public Item ReturnItem(int i) {

        return gameItems[i];
    }
    public int ReturnItem(Item itm) {

        int itmInt = 0;
        for (int i = 0; i < gameItems.Count; i++)
        {
            if (itm == gameItems[i]) {
                itmInt = i;
                return itmInt;
            }
        }
        return itmInt = 0;
    }
    public void RemoveItem(int i) {
        ownedItems[i]--;
        if (ownedItems[i] == 0) {
            ownedItems.Remove(i);
        }
    }
    public void RemoveAbility(int i) {
        ownedAbilities[i]--;
        if (ownedAbilities[i] == 0) {
            ownedAbilities.Remove(i);
        }
    }
    public void AddAbility(Ability a) {

        if (ownedAbilities.ContainsKey(ReturnAbility(a)))
        {
            ownedAbilities[ReturnAbility(a)]++;
        }
        else {
            ownedAbilities.Add(ReturnAbility(a), 1);
        }
    }
    public void AddItem(Item itm)
    {
        if (ownedItems.ContainsKey(ReturnItem(itm)))
        {
            ownedItems[ReturnItem(itm)]++;
        }
        else
        {
            ownedItems.Add(ReturnItem(itm), 1);
        }
    }
    public void AddRelic(RelicSO relic) {
        if (ownedRelics.ContainsKey(ReturnRelic(relic)))
        {
            ownedRelics[ReturnRelic(relic)] = true;
        }
        else
        {
            ownedRelics.Add(ReturnRelic(relic), true);
        }
    }
}
