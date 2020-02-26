#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>A concrete implementation of the <see cref="EventField"/> for <see cref="bool"/>.</para>
	/// </summary>
	/// <remarks>
	/// <para>This is nessesary for Unity to be able to serialize generic types.</para>
	/// </remarks>
	/// <seealso cref="IntEventField"/>
	/// <seealso cref="ColorTintEventField"/>
	[Serializable]
	public sealed class BoolEventField : EventField<bool>
	{
		/// <summary>
		/// <para>Constructs a new instance of this <see cref="BoolEventField"/>.</para>
		/// </summary>
		public BoolEventField()
		{
		}

		/// <summary>
		/// <para>Constructs a new instance of this with a default value.</para>
		/// </summary>
		/// <param name="defaultValue">The default value for this <see cref="BoolEventField"/>.</param>
		public BoolEventField(bool defaultValue) : base(defaultValue)
		{
		}
	}
}

#pragma warning restore
#endif
