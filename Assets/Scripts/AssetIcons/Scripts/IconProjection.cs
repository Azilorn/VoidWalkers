#pragma warning disable

namespace AssetIcons
{
	/// <summary>
	/// <para>An enum that represents camera projections when rendering Prefabs.</para>
	/// </summary>
	/// <seealso cref="AssetIconAttribute"/>
	/// <seealso cref="StyleDefinition"/>
	public enum IconProjection
	{
		/// <summary>
		/// <para>Represents a perspective camera projection.</para>
		/// </summary>
		/// <remarks>
		/// <para>This is the default value for the styling of <see cref="AssetIconAttribute"/>.</para>
		/// </remarks>
		/// <seealso cref="Orthographic"/>
		Perspective,

		/// <summary>
		/// <para>Represents an orthographic camera projection.</para>
		/// </summary>
		/// <seealso cref="Perspective"/>
		Orthographic
	}
}

#pragma warning restore
