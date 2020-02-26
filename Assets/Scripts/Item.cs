using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using AssetIcons;

public enum ItemMasterType {Potion, Shard, Scrolls, Relics }
public enum ItemType {Potion, Revive, AntiAilment, Escape, APUp }
public enum FloorAvailable {Zero, One, Two, Three, Four }
[CreateAssetMenu(fileName = "I.", menuName = "Elements/Item", order = 0)]
public class Item : ScriptableObject
{
    [BoxGroup("MasterType")]
    public ItemMasterType itemMasterType;
    [BoxGroup("Item Details")]
    public string itemName;
    [BoxGroup("Item Details")]
    [ResizableTextArea()]
    public string bio;
    [BoxGroup("Item Details")]
    public int cost = 10;
    [BoxGroup("Item Details")]
    public FloorAvailable floorAvailable;
    [BoxGroup("Item Details")]
    [ShowAssetPreview(32,32)]
    [AssetIcon]
    public Sprite itemIcon;
    [BoxGroup("Item Stats")]
    [ShowIf("ShowEffectAmount")]
    public int effectAmount;
    [BoxGroup("Item Stats")]
    public ItemType itemType;
    [BoxGroup("Item Stats")]
    [ShowIf("ShowAilmentType")]
    public NegativeAilment negativeAilment;
    public bool ShowEffectAmount() {

        switch (itemType)
        {
            case ItemType.AntiAilment:
                effectAmount = 0;
                return false;
            case ItemType.Escape:
                effectAmount = 0;
                return false;
        }
        return true;
    }
    public bool ShowAilmentType()
    {
        if (itemType == ItemType.AntiAilment)
            return true;
        else {
            negativeAilment = NegativeAilment.None;
            return false;
        }
    }
}
