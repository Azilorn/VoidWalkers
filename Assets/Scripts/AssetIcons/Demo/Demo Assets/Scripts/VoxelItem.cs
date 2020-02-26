#pragma warning disable

using AssetIcons;
using UnityEngine;

public class VoxelItem : ScriptableObject
{
	[AssetIcon(width: "85%", height: "85%", maxSize: 128, projection: IconProjection.Orthographic)]
	public GameObject Icon;

	[AssetIcon(maxSize: 128, layer: -1)]
	public Sprite Background;

	[AssetIcon(width: "28%", height: "20%", maxSize: 128, x: "-8%", y: "8%", anchor: IconAnchor.BottomRight, display: "Width > 24")]
	public Color ItemColor;
}

#pragma warning restore
