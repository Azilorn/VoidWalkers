#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Processing
{
	/// <summary>
	/// <para>A code representing the type of operation that an <see cref="IntermediateOperation"/> represents.</para>
	/// </summary>
	internal enum IntermediateOperationCode
	{
		None,

		// Maths
		Add,
		Subtract,
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

		Percentage,

		Invoke,
		Copy
	}
}

#pragma warning restore
#endif
