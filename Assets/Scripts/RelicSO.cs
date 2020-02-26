using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using System;
using AssetIcons;

public enum RelicUseable {No, Yes }
public enum RelicLostOnUse {No, Yes }
[CreateAssetMenu(fileName = "R.", menuName = "Elements/Relic", order = 0)]
public class RelicSO : ScriptableObject
{
    [BoxGroup("Relic Image")]
    [AssetIcon]
    public Sprite icon;
    [BoxGroup("Relic Details")]
    public string relicName;
    [BoxGroup("Relic Details")]
    public RelicName relicNameID;
    [TextArea]
    [BoxGroup("Relic Details")]
    public string relicDescription;
    [BoxGroup("Relic Details")]
    public int cost = 50;
    [BoxGroup("Relic Details")]
    public FloorAvailable floorAvailable;
    [BoxGroup("Relic Details")]
    public RelicUseable relicUseable;
    [BoxGroup("Relic Details")]
    public RelicLostOnUse relicLostOnUse;

}