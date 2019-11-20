using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCreatureStats
{
    public CreatureSO creatureSO;
    public CreatureStats creatureStats;
    public CreatureAbility[] creatureAbilities;
    public List<Ailment> ailments = new List<Ailment>();

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
                if (ailment == NegativeAilment.Poisoned)
                {
                    s = "is no longer poisoned.";
                    ailments.Remove(a);
                    BattleUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Burnt)
            {
                if (ailment == NegativeAilment.Burnt)
                {
                    s = "is no longer burnt.";
                    ailments.Remove(a);
                    BattleUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Sleep)
            {

                if (ailment == NegativeAilment.Sleep)
                {
                    s = "has been awoken.";
                    ailments.Remove(a);
                    BattleUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Frozen)
            {
                if (ailment == NegativeAilment.Frozen)
                {
                    s = "is no longer frozen.";
                    ailments.Remove(a);
                    BattleUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Confused)
            {
                if (ailment == NegativeAilment.Confused)
                {
                    s = "is no longer confused.";
                    ailments.Remove(a);
                    BattleUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
            else if (a is Ethereal)
            {
                if (ailment == NegativeAilment.Etheral)
                {
                    s = "is no longer in the etheral realm.";
                    ailments.Remove(a);
                    BattleUI.Instance.SetPlayerBattleUI();
                    return true;
                }
            }
        }
        s = "is ineffective.";
        return false;
    }

    public void CheckIfDead()
    {
        if (creatureStats.HP <= 0)
        {
            ailments.Clear();
        }
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

    public int HP { get => hp; set => hp = value; }
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
}