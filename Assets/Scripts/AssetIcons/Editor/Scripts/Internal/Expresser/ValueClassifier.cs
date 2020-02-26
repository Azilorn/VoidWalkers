#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser
{
	internal enum ValueClassifier : byte
	{
		None,
		Boolean,
		Float,
		FloatFractional,
		Int,
		IntFractional
	}
}

#pragma warning restore
#endif
