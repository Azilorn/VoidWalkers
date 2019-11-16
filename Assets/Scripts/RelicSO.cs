using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "R.", menuName = "Elements/Relic", order = 0)]
public class RelicSO : ScriptableObject
{
    public string relicName;
    [ResizableTextArea]
    public string relicDescription;
    public Sprite icon;
    public int cost = 50;

}