using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using System;

public enum NegativeAilment {
    None, Burnt, Shocked, Poisoned, Frozen, Confused, Etheral, Sleep, SpeedDown, SpeedDownDown, AttackDown, AttackDownDown, DefenceDown, DefenceDownDown, AccuracyDown, AccuracyDownDown, DodgeDown,
    DodgeDownDown, CritATKDown, CritATKDownDown, CritDEFDown, CritDEFDownDown, All
}
public enum PositiveAilment {
    None, SpeedUp, SpeedUpUp, AttackUp, AttackUpUp, DefenceUp, DefenceUpUp, AccuracyUp, AccuracyUpUp, DodgeUp, DodgeUpUp, Heal, CritATKUp, CritATKUpUp, CritDEFUp, CritDEFUpUp
}
public enum AbilityType {None, Attack, Weather, Buff, Debuff, Other, AttackSelf }
public enum TargetType {None, Self, Target, Both }
public enum AnimationLocation {Center, Target, Self }
[CreateAssetMenu(fileName = "A.", menuName = "Elements/Ability", order = 0)]
public class Ability : ScriptableObject
{
    [BoxGroup("Details")]
    public string abilityName;
    [ResizableTextArea]
    [BoxGroup("Details")]
    public string abilityBio;
    [BoxGroup("Details")]
    public int cost = 20;
    [BoxGroup("Ability Action Type")]
    public AbilityType type;
    [BoxGroup("Ability Action Type")]
    public ElementType elementType;
    [BoxGroup("Ability Action Type")]
    public PositiveAilment positiveAilment;
    [BoxGroup("Ability Action Type")]
    public NegativeAilment negativeAilment;
    [BoxGroup("Ability Stats")]
    public AbilityStats abilityStats;
    [BoxGroup("Animations")]
    public List<AnimationDetail> animations = new List<AnimationDetail>();

    public Ability()
    {
        abilityName = "To Be Changed";
        type = AbilityType.Attack;
        elementType = ElementType.Normal;
        abilityStats = new AbilityStats();
        this.abilityStats.accuracy = 100;
        this.abilityStats.power = 30;
        this.abilityStats.maxCount = 20;
        this.abilityStats.percentage = 100;
    }
    public Ability(AbilityType type)
    {
        this.type = type;
    }

    public Ability(bool attackself, ElementType primaryElementType)
    {
        if (attackself)
        {
            type = AbilityType.AttackSelf;
            elementType = primaryElementType;
            abilityStats = new AbilityStats();
            abilityStats.accuracy = 100;
            abilityStats.power = 30;
            abilityStats.maxCount = 20;
            abilityStats.percentage = 100;
            animations = new List<AnimationDetail>();
            animations.Add(new AnimationDetail());
            animations[0].animation = ImageAnimation.MoveHorizontal;
            animations[0].targetType = TargetType.Self;
            animations[0].duration = 0.75f;
            animations.Add(new AnimationDetail());
            animations[1].animation = ImageAnimation.TakeDamage;
            animations[1].targetType = TargetType.Self;
            animations[1].duration = 0.75f;
        }
    }
}
[Serializable]
public class AnimationDetail {

    public ImageAnimation animation;
    public TargetType targetType;
    [ShowIf("ShowAnimationLocation")] 
    public AnimationLocation animationLocation;
    public GameObject animSprite;
    public float duration;
    public float delay;
    public bool SkipCoroutineWait = false;

    public bool ShowAnimationLocation() {

        if (animation == ImageAnimation.SpawnAnimSprite)
            return true;
        return false;
    }
}
[Serializable]
public class AbilityStats {

    public int maxCount;
    public int power;
    [MinMaxSlider(0, 255)]
    public int accuracy;
    [Label("Use this based on the Ability Type")]
    public float percentage;
}
