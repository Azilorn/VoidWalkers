#pragma warning disable

using UnityEngine;

namespace AssetIcons
{
	/// <summary>
	/// <para>An enum that represents how the aspect ratio of a graphic should be used.</para>
	/// </summary>
	/// <seealso cref="AssetIconAttribute"/>
	/// <seealso cref="StyleDefinition"/>
	public enum IconAspect
	{
		/// <summary>
		/// <para>The rendered graphic should fit inside the <see cref="Rect"/> without any stretching.</para>
		/// </summary>
		/// <remarks>
		/// <para>This is the default value for the <see cref="AssetIconAttribute"/>.</para>
		/// </remarks>
		/// <seealso cref="Envelop"/>
		/// <seealso cref="Stretch"/>
		Fit,

		/// <summary>
		/// <para>The rendered graphic should envelop the <see cref="Rect"/> without any stretching.</para>
		/// </summary>
		/// <seealso cref="Fit"/>
		/// <seealso cref="Stretch"/>
		Envelop,

		/// <summary>
		/// <para>The rendered graphic will stretch to the <see cref="Rect"/> dimensions.</para>
		/// </summary>
		/// <seealso cref="Fit"/>
		/// <seealso cref="Envelop"/>
		Stretch
	}
}

#pragma warning restore
