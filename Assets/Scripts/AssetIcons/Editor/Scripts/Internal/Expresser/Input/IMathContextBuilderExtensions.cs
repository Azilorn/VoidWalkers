#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal static class IMathContextBuilderExtensions
	{
		public static IMathContextBuilder WithTerm(this IMathContextBuilder builder, string term, MathValue mathValue)
		{
			return builder.WithTerm(term, new StaticValueProvider(mathValue));
		}
	}
}

#pragma warning restore
#endif
