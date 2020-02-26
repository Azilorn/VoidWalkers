#if UNITY_EDITOR
#pragma warning disable

using System.Reflection;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Used to abstract getting of a value from an object and drawing of graphics.</para>
	/// </summary>
	internal class MethodIconProvider : IIconProvider
	{
		private readonly MethodInfo method;

		/// <summary>
		/// <para></para>
		/// </summary>
		public CompiledStyleDefinition Style { get; private set; }

		/// <summary>
		/// <para>Constructs a new instance of the <c>MethodIconProvider</c>.</para>
		/// </summary>
		/// <param name="style">A style to use to draw graphics provided by this <see cref="IIconProvider"/>.</param>
		/// <param name="method">A method that's invoked to provide the graphic.</param>
		public MethodIconProvider(CompiledStyleDefinition style, MethodInfo method)
		{
			Style = style;
			this.method = method;
		}

		public object SourceGraphic(object target)
		{
			return method.Invoke(target, null);
		}

		public int CompareTo(IIconProvider other)
		{
			if (other.Style == null)
			{
				return 1;
			}
			if (Style == null)
			{
				return -1;
			}

			if (Style.Layer != other.Style.Layer)
			{
				if (Style.Layer <= other.Style.Layer)
				{
					return -1;
				}
				return 1;
			}
			return 0;
		}

		public override string ToString()
		{
			return string.Format("Icon from {0}", method.Name);
		}
	}
}

#pragma warning restore
#endif
