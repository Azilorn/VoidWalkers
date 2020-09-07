using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCreatureStats
{

    public PlayerCreatureStats() {
        this.creatureAbilities = new CreatureAbility[4];
        for (int i = 0; i < creatureAbilities.Length; i++)
        {
            creatureAbilities[i] = new CreatureAbility();
        }
    }
    public CreatureSO creatureSO;
    public CreatureStats creatureStats;
    public CreatureAbility[] creatureAbilities;
    public List<Ailment> ailments = new List<Ailment>();

    public PlayerCreatureStatsSaveData creatureSaveData;

    public void ClampHP() {
        creatureStats.HP = Mathf.Clamp(creatureStats.HP, 0, creatureStats.MaxHP);
    }
    public void AddLevel()
    {
        int i = 0;
        creatureStats.MaxHP += creatureSO.AddLevelUpStat(StatLevelUpEnum.MaxHp, creatureStats, out i);
        creatureStats.HP += i;
        ClampHP();
        creatureStats.strength += creatureSO.AddLevelUpStat(StatLevelUpEnum.Strength, creatureStats, out i);
        creatureStats.defence += creatureSO.AddLevelUpStat(StatLevelUpEnum.Defence, creatureStats, out i);
        creatureStats.speed += creatureSO.AddLevelUpStat(StatLevelUpEnum.Speed, creatureStats, out i);
        creatureStats.criticalAttack += creatureSO.AddLevelUpStat(StatLevelUpEnum.CriticalAttack, creatureStats, out i);
        creatureStats.criticalDefence += creatureSO.AddLevelUpStat(StatLevelUpEnum.CriticalDefence, creatureStats, out i);
    }
    public void SetLevel(int l, bool updateStats)
    {
        creatureStats.level = l;
        if (creatureSO == null) {
            creatureStats.level = 0;
            return;
        }
        if (updateStats)
        {
            creatureStats.MaxHP = creatureSO.ReturnLevelUpStat(StatLevelUpEnum.MaxHp, creatureStats);
            creatureStats.HP = creatureStats.MaxHP;
            creatureStats.strength = creatureSO.ReturnLevelUpStat(StatLevelUpEnum.Strength,creatureStats);
            creatureStats.defence = creatureSO.ReturnLevelUpStat(StatLevelUpEnum.Defence,creatureStats);
            creatureStats.speed = creatureSO.ReturnLevelUpStat(StatLevelUpEnum.Speed,creatureStats);
            creatureStats.criticalAttack = creatureSO.ReturnLevelUpStat(StatLevelUpEnum.CriticalAttack,creatureStats);
            creatureStats.criticalDefence = creatureSO.ReturnLevelUpStat(StatLevelUpEnum.CriticalDefence,creatureStats);
        }
        else return;
    }
    public bool CheckForAilment(NegativeAilment ailment, out string s)
    {
        s = "";

        if (ailments.Count < 1) {
            s = "is ineffective.";
            return false;
        }
        foreach (Ailment a in ailments)
        {
            if (a is Poison)
            {
                if (ailment == NegativeAilment.Poisoned || ailment == NegativeAilment.All)
                {
                    s = "is no longer poisoned.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Burnt)
            {
                if (ailment == NegativeAilment.Burnt || ailment == NegativeAilment.All)
                {
                    s = "is no longer burnt.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Sleep)
            {

                if (ailment == NegativeAilment.Sleep || ailment == NegativeAilment.All)
                {
                    s = "has been awoken.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Frozen)
            {
                if (ailment == NegativeAilment.Frozen || ailment == NegativeAilment.All)
                {
                    s = "is no longer frozen.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Confused)
            {
                if (ailment == NegativeAilment.Confused || ailment == NegativeAilment.All)
                {
                    s = "is no longer confused.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Ethereal)
            {
                if (ailment == NegativeAilment.Etheral || ailment == NegativeAilment.All)
                {
                    s = "is no longer in the etheral realm.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Bleeding)
            {
                if (ailment == NegativeAilment.Bleeding || ailment == NegativeAilment.All)
                {
                    s = "is no longer bleeding.";
                    ailments.Remove(a);
                    CoreUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
        }
        s = "is ineffective.";
        return false;
    }

    public void SetCreatureDataFromLoad(PlayerCreatureStatsSaveData playerCreatureStatsSaveData)
    {
        creatureSaveData = playerCreatureStatsSaveData;
        CreatureTable creatureTable = CreatureTable.Instance;
        GameObject go2 = Resources.Load("AbilityTable") as GameObject;
        AbilityTable at = go2.GetComponent<AbilityTable>();
        creatureSO = creatureTable.Creatures[creatureSaveData.CreatureSO];
        creatureStats = new CreatureStats();
        creatureStats = creatureSaveData.creatureStat;
        creatureAbilities = new CreatureAbility[4];
        SetAbilityData(creatureSaveData.abilityData);
    }

    private void SetAbilityData(List<CreatureAbilitySaveData> abilityData)
    {
        GameObject go2 = Resources.Load("AbilityTable") as GameObject;
        AbilityTable at = go2.GetComponent<AbilityTable>();
        for (int i = 0; i < abilityData.Count; i++)
        {
            if (abilityData[i].abilityID == 9999)
                continue;
            Ability ability = at.Abilities[abilityData[i].abilityID];
            creatureAbilities[i] = new CreatureAbility(ability, abilityData[i].remainingCount);
        }
    }

    public void CheckIfDead()
    {
        if (creatureStats.HP <= 0)
        {
            ailments.Clear();
        }
    }
    public void SetCreatureSaveData() {

        CreatureTable creatureTable = CreatureTable.Instance;
        GameObject go2 = Resources.Load("AbilityTable") as GameObject;
        AbilityTable at = go2.GetComponent<AbilityTable>();
        if (creatureSaveData == null)
            creatureSaveData = new PlayerCreatureStatsSaveData();
        creatureSaveData.CreatureSO = creatureTable.ReturnCreatureID(creatureSO);
        creatureSaveData.creatureStat = creatureStats;
        creatureSaveData.abilityData = GetAbilityData(at);
    }
    public List<CreatureAbilitySaveData> GetAbilityData(AbilityTable abilityTable) {

        List<CreatureAbilitySaveData> data = new List<CreatureAbilitySaveData>();

        for (int i = 0; i < creatureAbilities.Length; i++)
        {
            if (creatureAbilities[i] == null)
            {
            }
            else if (creatureAbilities[i].ability == null || creatureAbilities[i].remainingCount == null)
            {
                data.Add(new CreatureAbilitySaveData(9999, 9999));
            }
            else {

                data.Add(new CreatureAbilitySaveData(abilityTable.ReturnAbilityID(creatureAbilities[i].ability), creatureAbilities[i].remainingCount));
            }
        }
        return data;
    }

   
}
[Serializable]
public class CreatureStats
{
    [SerializeField]private int xp;
    public int level;
    [SerializeField] private int hp;
    [SerializeField] private int maxHP;
    public int strength;
    private int battleStrength;
    public int defence;
    private int battleDefence;
    public int speed;
    private int battleSpeed;
    [HideInInspector] public int accuracy = 100;
    [SerializeField] private int battleAccuracy = 100;
    [HideInInspector] public int dodge = 100;
    [SerializeField] private int battleDodge = 100;
    public int criticalAttack;
    private int battleCritATK;
    public int criticalDefence;
    private int battleCritDef;

    public int HP {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public int BattleStrength { get => battleStrength; set => battleStrength = value; }
    public int BattleDefence { get => battleDefence; set => battleDefence = value; }
    public int BattleSpeed { get => battleSpeed; set => battleSpeed = value; }
    public int BattleCritATK { get => battleCritATK; set => battleCritATK = value; }
    public int BattleCritDef { get => battleCritDef; set => battleCritDef = value; }
    public int BattleAccuracy { get => battleAccuracy; set => battleAccuracy = value; }
    public int BattleDodge { get => battleDodge; set => battleDodge = value; }
    public int Xp { get => xp; set => xp = value; }

    public void SetCreatureBattleStats()
    {
        battleStrength = strength;
        battleDefence = defence;
        battleSpeed = speed;
        accuracy = 100;
        dodge = 100;
        battleAccuracy = accuracy;
        battleDodge = dodge;
        battleCritATK = criticalAttack;
        battleCritDef = criticalDefence;
    }
    public void EditorSetLevelXP()
    {
        xp = XPMatrix.xpLevelList[level];
    }
}
[Serializable]
public class CreatureAbility {

    public Ability ability;
    public int remainingCount;

    public CreatureAbility() { }

    public CreatureAbility(Ability ability, int remainingCount)
    {
        this.ability = ability;
        this.remainingCount = remainingCount;
    }
}

[Serializable]
public class PlayerCreatureStatsSaveData
{
    public int CreatureSO;
    public CreatureStats creatureStat;
    public List<CreatureAbilitySaveData> abilityData;
}
[Serializable]
public class CreatureAbilitySaveData
{
    public int abilityID;
    public int remainingCount;

    public CreatureAbilitySaveData(int abilityID, int remainingCount)
    {
        this.abilityID = abilityID;
        this.remainingCount = remainingCount;
    }
}
