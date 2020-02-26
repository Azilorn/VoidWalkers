#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal interface IMathContextBuilder
	{
		IMathContext Build();

		IMathContextBuilder ImplicitlyReferences(IValueProvider value);

		IMathContextBuilder WithTerm(string term, IValueProvider value);

		IMathContextBuilder WithUnit(string unit, IValueProvider value);
	}
}

#pragma warning restore
#endif
