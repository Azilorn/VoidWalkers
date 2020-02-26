#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser
{
	internal enum SyntaxTokenKind : byte
	{
		None,

		// Maths
		Plus,
		Minus,
		Multiply,
		Divide,
		Power,

		// Logic
		And,
		Or,
		Not,
		Equal,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,

		// Suffix
		Percentage,

		// Data
		Value,
		Source,

		// Structure
		OpenParentheses,
		CloseParentheses,
		Comma,
	}
}

#pragma warning restore
#endif
