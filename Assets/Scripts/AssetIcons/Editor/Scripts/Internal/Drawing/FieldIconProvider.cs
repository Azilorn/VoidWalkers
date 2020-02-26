#if UNITY_EDITOR
#pragma warning disable

using System.Reflection;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Returns an object from a <see cref="FieldInfo"/> in a given context.</para>
	/// </summary>
	internal class FieldIconProvider : IIconProvider
	{
		private readonly FieldInfo field;
		private readonly CompiledStyleDefinition style;

		public CompiledStyleDefinition Style
		{
			get
			{
				return style;
			}
		}

		public FieldIconProvider(CompiledStyleDefinition style, FieldInfo field)
		{
			this.style = style;
			this.field = field;
		}

		public object SourceGraphic(object target)
		{
			return field.GetValue(target);
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
			return string.Format("Icon from {0}", field.Name);
		}
	}
}

#pragma warning restore
#endif
