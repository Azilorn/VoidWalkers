using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;

public enum StatPrioity { Normal, Low, High }
public enum StatLevelUpEnum {MaxHp, Strength, Defence, Speed, CriticalAttack, CriticalDefence }
[CreateAssetMenu(fileName = "C.", menuName = "Elements/Creature", order = 0)]
public class CreatureSO : ScriptableObject
{
    [BoxGroup("Details")]
    public string creatureName;
    [ShowAssetPreview(64, 64)]
    [BoxGroup("Details")]
    public Sprite creatureEnemyIcon;
    [BoxGroup("Details")]
    [ShowAssetPreview(64, 64)]
    public Sprite creaturePlayerIcon;
    [BoxGroup("Details")]
    [TextArea()]
    public string creatureBio;
    [BoxGroup("Image Size")]
    public int width = 500;
    [BoxGroup("Image Size")]
    public int height = 500;
    [BoxGroup("Characteristics")]
    public ElementType primaryElement;
    [BoxGroup("Characteristics")]
    public ElementType secondaryElement;
    [BoxGroup("Characteristics")]
    public CreatureCharacteristics characteristics;
    [BoxGroup("Evolution")]
    public List<EvolutionRequirements> evolutions = new List<EvolutionRequirements>();
    [BoxGroup("Abilities")]
    public List<Ability> startingAbilities = new List<Ability>();
    [BoxGroup("Abilities")]
    public List<Ability> illegalAbilities = new List<Ability>();
    [BoxGroup("Abilities")]
    public List<LearnableAbility> learnableAbility = new List<LearnableAbility>();
    [BoxGroup("BaseStats")]
    [InfoBox("Set Stats Up Based On Priority")]
    public float StatMultiplier;
    [BoxGroup("BaseStats")]
    public BaseStats baseStats;

    public CreatureSO()
    {
        this.creatureName = "To Change";
        StatMultiplier = 1;
    }

    public CreatureSO(int width, int height, List<EvolutionRequirements> evolutions, BaseStats baseStats)
    {
        this.width = width;
        this.height = height;
        this.evolutions = evolutions;
        this.baseStats = baseStats;
    }

    public CreatureSO GetCopy() {

        CreatureSO so = new CreatureSO( width, height, evolutions, baseStats);
        return so;
    }
    public int AddLevelUpStat(StatLevelUpEnum statLevelUpEnum, CreatureStats stats, out int additionalInt) {

        int value = 0;

        if (statLevelUpEnum == StatLevelUpEnum.MaxHp)
        {
            value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.maxHP.prioity));
        }
        else if (statLevelUpEnum == StatLevelUpEnum.Strength)
        {
            value += (int)(UnityEngine.Random.Range(1,3) * ReturnStatMultiplier(baseStats.strength.prioity));
        }
        else if (statLevelUpEnum == StatLevelUpEnum.Defence)
        {
            value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.defence.prioity));
        }
        else if (statLevelUpEnum == StatLevelUpEnum.Speed)
        {
            value += (int)(UnityEngine.Random.Range(1, 3) *  ReturnStatMultiplier(baseStats.speed.prioity));
        }
        else if (statLevelUpEnum == StatLevelUpEnum.CriticalAttack)
        {
            value += (int)(UnityEngine.Random.Range(1, 3) *  ReturnStatMultiplier(baseStats.criticalAttack.prioity));
        }
        else if (statLevelUpEnum == StatLevelUpEnum.CriticalDefence)
        {
            value += (int)(UnityEngine.Random.Range(1, 3) *  ReturnStatMultiplier(baseStats.criticalDefence.prioity));
        }
        additionalInt = value;
        return (int)(value * StatMultiplier * UnityEngine.Random.Range(0.9f, 1.1f));
    }
    public int ReturnLevelUpStat(StatLevelUpEnum statLevelUpEnum, CreatureStats stats) {

        int value = 0;
        if (statLevelUpEnum == StatLevelUpEnum.MaxHp)
        {
            value += baseStats.maxHP.stat;
            for (int i = 0; i < stats.level; i++) {
                value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.maxHP.prioity));
            }
            
        }
        else if (statLevelUpEnum == StatLevelUpEnum.Strength)
        {
            value += baseStats.strength.stat;
            for (int i = 0; i < stats.level; i++)
            {
                value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.strength.prioity));
            }
        }
        else if (statLevelUpEnum == StatLevelUpEnum.Defence)
        {
            value += baseStats.defence.stat;
            for (int i = 0; i < stats.level; i++)
            {
                value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.defence.prioity));
            }
        }
        else if (statLevelUpEnum == StatLevelUpEnum.Speed)
        {
            value += baseStats.speed.stat;
            for (int i = 0; i < stats.level; i++)
            {
                value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.speed.prioity));
            }
        }
        else if (statLevelUpEnum == StatLevelUpEnum.CriticalAttack)
        {
            value += baseStats.criticalAttack.stat;
            for (int i = 0; i < stats.level; i++)
            {
                value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.criticalAttack.prioity));
            }
        }
        else if (statLevelUpEnum == StatLevelUpEnum.CriticalDefence)
        {
            value += baseStats.criticalDefence.stat;
            for (int i = 0; i < stats.level; i++)
            {
                value += (int)(UnityEngine.Random.Range(1, 3) * ReturnStatMultiplier(baseStats.criticalDefence.prioity));
            }
        }
        return (int)(value * StatMultiplier * UnityEngine.Random.Range(0.9f, 1.1f));
    }

    private float ReturnStatMultiplier(StatPrioity prioity)
    {
        switch (prioity)
        {
            case StatPrioity.Normal:
                return 1;
            case StatPrioity.Low:
                return 0.75f;
            case StatPrioity.High:
                return 1.25f;
            default:
                return 1;
        }
    }

    public void SetEvolutionSize(int size) {

        if (size > evolutions.Count) {
            evolutions.Add(new EvolutionRequirements());
        }
        else if(size < evolutions.Count)
        {
            int index = evolutions.Count - size;
            for (int i = evolutions.Count - 1; i >= size ; i--) {
                evolutions.RemoveAt(i);
            }
        }
    }
}
[Serializable]
public class BaseStats {

    public int TotalSpend;
    public CreatureStat maxHP;
    public CreatureStat strength;
    public CreatureStat defence;
    public CreatureStat speed;
    public CreatureStat criticalAttack;
    public CreatureStat criticalDefence;
}
[Serializable]
public class CreatureStat {

    public int stat;
    public StatPrioity prioity;
}
[Serializable]
public class EvolutionRequirements
{
    public CreatureSO evolutionSO;
    public int levelRequirement;
}
[Serializable]
public class CreatureCharacteristics {

    //Height in CM
    public string height;
    //Weight in Kilo
    public string weight;
}
[Serializable]
public class LearnableAbility
{
    public int levelToLearn;
    public Ability abilityToLearn;
}

