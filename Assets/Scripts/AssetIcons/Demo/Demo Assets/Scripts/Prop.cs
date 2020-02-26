#pragma warning disable

using UnityEngine;
using UnityEngine.Serialization;

namespace AssetIcons.Demo
{
	/// <summary>
	/// <para>A sample <see cref="ScriptableObject"/> that draws a <see cref="Sprite"/> icon and a <see cref="Texture2D"/> background.</para>
	/// </summary>
	public sealed class Prop : ScriptableObject
	{
		/// <summary>
		/// <para>An icon to render the for this <see cref="ScriptableObject"/>.</para>
		/// </summary>
		[AssetIcon(aspect: IconAspect.Fit, maxSize: 64)]
		[FormerlySerializedAs("icon")]
		public Sprite Icon;

		/// <summary>
		/// <para>An icon to render the for this <see cref="ScriptableObject"/>'s background.</para>
		/// </summary>
		[AssetIcon(layer: -1)]
		[FormerlySerializedAs("border")]
		public Texture2D Border;
	}
}

#pragma warning restore
