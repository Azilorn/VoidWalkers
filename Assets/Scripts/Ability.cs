using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using System;
using AssetIcons;
using UnityEditor;

public enum NegativeAilment {
    None, Burnt, Shocked, Poisoned, Frozen, Confused, Etheral, Sleep, SpeedDown, SpeedDownDown, AttackDown, AttackDownDown, DefenceDown, DefenceDownDown, AccuracyDown, AccuracyDownDown, DodgeDown,
    DodgeDownDown, CritATKDown, CritATKDownDown, CritDEFDown, CritDEFDownDown, All, Bleeding
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
    [AssetIcon]
    [ShowIf("ShowIfElement")]
    public Sprite icon;

    [BoxGroup("Details")]
    public string abilityName;
    [ResizableTextArea]
    [BoxGroup("Details")]
    public string abilityBio;
    [BoxGroup("Details")]
    public int cost = 20;
    [BoxGroup("Details")]
    public FloorAvailable floorAvailable;
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
    public bool ShowIfElement() {


        switch (elementType)
        {
            case ElementType.Normal:
                icon = Resources.Load<Sprite>("ElementType/Normal");
                break;
            case ElementType.Fire:
                icon = Resources.Load<Sprite>("ElementType/Fire");
                break;
            case ElementType.Water:
                icon = Resources.Load<Sprite>("ElementType/Water");
                break;
            case ElementType.Nature:
                icon = Resources.Load<Sprite>("ElementType/Nature");
                break;
            case ElementType.Electric:
                icon = Resources.Load<Sprite>("ElementType/Electric");
                break;
            case ElementType.Spectre:
                icon = Resources.Load<Sprite>("ElementType/Spectre");
                break;
            case ElementType.Fighting:
                icon = Resources.Load<Sprite>("ElementType/Fighting");
                break;
            case ElementType.Ice:
                icon = Resources.Load<Sprite>("ElementType/Ice");
                break;
            case ElementType.Wind:
                icon = Resources.Load<Sprite>("ElementType/Wind");
                break;
            case ElementType.Earth:
                icon = Resources.Load<Sprite>("ElementType/Earth");
                break;
            case ElementType.Metal:
                icon = Resources.Load<Sprite>("ElementType/Metal");
                break;
            case ElementType.Insect:
                icon = Resources.Load<Sprite>("ElementType/Insect");
                break;
            case ElementType.Unholy:
                icon = Resources.Load<Sprite>("ElementType/Unholy");
                break;
            case ElementType.Holy:
                icon = Resources.Load<Sprite>("ElementType/Holy");
                break;
            case ElementType.Ancient:
                icon = Resources.Load<Sprite>("ElementType/Ancient");
                break;
            case ElementType.None:
                icon = null;
                break;
            default:
                icon = null;
                break;
        }
        EditorUtility.SetDirty(icon);
        if (icon == null)
            return false;
        else return true;
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
