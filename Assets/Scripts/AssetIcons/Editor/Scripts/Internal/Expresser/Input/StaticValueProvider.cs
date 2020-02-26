#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	/// <summary>
	/// <para></para>
	/// </summary>
	internal class StaticValueProvider : IValueProvider
	{
		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public MathValue Value { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public StaticValueProvider(MathValue value)
		{
			Value = value;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public StaticValueProvider(float value)
		{
			Value = new MathValue(value, false);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public StaticValueProvider(bool value)
		{
			Value = new MathValue(value);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}

#pragma warning restore
#endif
