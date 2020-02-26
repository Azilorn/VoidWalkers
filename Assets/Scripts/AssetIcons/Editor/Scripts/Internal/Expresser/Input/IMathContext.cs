#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal interface IMathContext
	{
		IValueProvider ImplicitReference { get; }

		bool TryGetTerm(string key, out IValueProvider provider);

		bool TryGetUnit(string key, out IValueProvider provider);

	}
}

#pragma warning restore
#endif
