#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>A basic interface for supplying an object return type in a given context.</para>
	/// </summary>
	internal interface IIconProvider : IComparable<IIconProvider>
	{
		CompiledStyleDefinition Style { get; }

		object SourceGraphic(object target);
	}
}

#pragma warning restore
#endif
