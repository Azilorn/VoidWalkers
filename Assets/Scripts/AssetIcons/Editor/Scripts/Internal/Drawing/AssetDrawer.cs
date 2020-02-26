#if UNITY_EDITOR
#pragma warning disable

using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Describes a collection of graphics that make up a GUI draw.</para>
	/// </summary>
	internal class AssetDrawer
	{
		/// <summary>
		/// <para>The collection of graphics that make up a GUI draw.</para>
		/// </summary>
		public readonly IIconProvider[] Graphics;

		private object lastTarget;
		private readonly object[] providedGraphics;

		/// <summary>
		/// <para>Constructs a new AssetDrawer with a collection of graphics.</para>
		/// </summary>
		/// <param name="graphics">A collection of graphics that make up this drawer.</param>
		public AssetDrawer(IIconProvider[] graphics)
		{
			Graphics = graphics;

			providedGraphics = new object[Graphics.Length];
		}

		/// <summary>
		/// <para>Determines whether this drawer is usable to draw a graphic for the targeted object.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if this drawer is valid for drawing for the targeted object; otherwise <c>false</c>.</para>
		/// </returns>
		public bool CanDraw(object target)
		{
			UpdateCachedProvidedGraphicsForTarget(target);

			for (int i = 0; i < providedGraphics.Length; i++)
			{
				object providedGraphic = providedGraphics[i];

				if (providedGraphic != null && !providedGraphic.Equals(null))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// <para>Renders the graphic for this drawer.</para>
		/// </summary>
		/// <param name="rect">The are in which to draw the icon.</param>
		/// <param name="target">The object to draw graphics for.</param>
		/// <param name="selected">Whether the selected graphic should appear selected.</param>
		public void Draw(Rect rect, object target, bool selected)
		{
			UpdateCachedProvidedGraphicsForTarget(target);

			for (int i = 0; i < providedGraphics.Length; i++)
			{
				var graphicProvider = Graphics[i];
				object providedGraphic = providedGraphics[i];

				if (providedGraphic != null && !providedGraphic.Equals(null))
				{
					var drawRect = AssetIconTools.AreaToIconRect(rect, graphicProvider.Style.MaxSize);
					AssetIconDrawer.DrawObject(drawRect, providedGraphic, graphicProvider.Style, selected);
				}
			}
		}

		/// <summary>
		/// <para>Clears the chaced value from this <see cref="AssetDrawer"/>.</para>
		/// </summary>
		public void ClearCache()
		{
			lastTarget = null;
		}

		private void UpdateCachedProvidedGraphicsForTarget(object target)
		{
			if (lastTarget != target)
			{
				for (int i = 0; i < Graphics.Length; i++)
				{
					var graphic = Graphics[i];
					providedGraphics[i] = graphic.SourceGraphic(target);
				}
				lastTarget = target;
			}
		}
	}
}

#pragma warning restore
#endif
