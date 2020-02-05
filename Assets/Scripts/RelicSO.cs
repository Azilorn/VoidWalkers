using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using System;

public enum RelicUseable {No, Yes }
public enum RelicLostOnUse {No, Yes }
[CreateAssetMenu(fileName = "R.", menuName = "Elements/Relic", order = 0)]
public class RelicSO : ScriptableObject
{
    public string relicName;
    public RelicName relicNameID;
    [TextArea]
    public string relicDescription;
    public Sprite icon;
    public int cost = 50;
    public RelicUseable relicUseable;
    public RelicLostOnUse relicLostOnUse;

}