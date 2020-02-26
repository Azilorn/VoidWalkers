#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>The central hub of AssetIcons that ties all of the smaller components together to draw them.</para>
	/// </summary>
	[InitializeOnLoad]
	internal static class AssetIconTools
	{
		public static HashSet<string> UnsupportedExtensions = new HashSet<string>()
		{
			".asset",
			".bmp",
			".exr",
			".gif",
			".hdr",
			".iff",
			".jpeg",
			".jpg",
			".pict",
			".png",
			".psd",
			".tga",
			".tiff"
		};

		static AssetIconTools()
		{
			AssetIconProjectHooks.OnInternalDrawIcon += ItemOnGUI;
		}

		/// <summary>
		/// <para>Remaps a <see cref="Rect"/> to conform with Unity's UI control positions.</para>
		/// </summary>
		/// <returns>
		/// <para>The <see cref="Rect"/> repositioned over the original icon.</para>
		/// </returns>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="maxSize">The max size of the <see cref="Rect"/>.</param>
		public static Rect AreaToIconRect(Rect rect, float maxSize = 64.0f)
		{
			bool isSmall = IsIconSmall(rect);

			if (isSmall)
			{
				rect.width = rect.height;
			}
			else
			{
				rect.height = rect.width;
#if UNITY_2019_3_OR_NEWER
				rect.width += 1.0f;
#endif
			}

			if (rect.width <= maxSize && rect.height <= maxSize)
			{
#if UNITY_5_5
				if (isSmall)
				{
					rect = new Rect (rect.x + 3, rect.y, rect.width, rect.height);
				}
#elif UNITY_5_6_OR_NEWER
				if (isSmall && !IsTreeView(rect))
				{
					rect = new Rect(rect.x + 3, rect.y, rect.width, rect.height);
				}
#endif
			}
			else
			{
				float offset = (rect.width - maxSize) * 0.5f;
				rect = new Rect(rect.x + offset, rect.y + offset, maxSize, maxSize);
			}

			return rect;
		}

		/// <summary>
		/// <para>Determines if the <see cref="Rect"/> should be drawn using a small icon.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if the icon is small; otherwise, <c>false</c>.</para>
		/// </returns>
		/// <param name="rect">The rect to check if it's small.</param>
		private static bool IsIconSmall(Rect rect)
		{
			return rect.width > rect.height;
		}

		/// <summary>
		/// <para>Determines if the <see cref="Rect"/> is being drawn in Tree View.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if is the specified <see cref="Rect"/> is in <c>TreeView</c>; otherwise, <c>false</c>.</para>
		/// </returns>
		/// <param name="rect">The <see cref="Rect"/> used to check if it's being drawn in treeview.</param>
		private static bool IsTreeView(Rect rect)
		{
			return (rect.x - 16) % 14 == 0;
		}

		/// <summary>
		/// <para>Paints the item in the project window.</para>
		/// </summary>
		/// <param name="guid">The GUID of the asset to check.</param>
		/// <param name="rect">The Rect in which the item is drawn.</param>
		private static void ItemOnGUI(string guid, Rect rect)
		{
			if (Event.current.type != EventType.Repaint || string.IsNullOrEmpty(guid))
			{
				return;
			}

			if (!AssetIconsPreferences.Enabled.Value)
			{
				return;
			}

			var assetTarget = AssetTarget.CreateFromGUID(guid);

			if (string.IsNullOrEmpty(assetTarget.Extension))
			{
				return;
			}

			if (assetTarget.Extension == ".asset")
			{
				var obj = AssetDatabase.LoadAssetAtPath(assetTarget.FilePath, typeof(Object)) as Object;

				if (obj == null)
				{
					return;
				}

				var type = obj.GetType();

				AssetDrawer assetDrawer;
				bool result = AssetDrawerLibrary.AssetDrawers.TryGetValue(type, out assetDrawer);

				if (result)
				{
					if (assetDrawer.CanDraw(obj))
					{
						bool selected = Selection.Contains(obj);

						var backgroundRect = AssetIconTools.AreaToIconRect(rect, 64);
						AssetIconDrawer.DrawBackground(backgroundRect);

						assetDrawer.Draw(rect, obj, selected);
					}
					assetDrawer.ClearCache();
				}
			}
			else if (AssetIconsPreferences.DrawGUIStyles.Value && assetTarget.Extension == ".guiskin")
			{
				if (assetTarget.UnityObject is GUISkin)
				{
					bool selected = Selection.Contains(assetTarget.UnityObject);
					var skin = (GUISkin)assetTarget.UnityObject;

					rect = AreaToIconRect(rect);

					AssetIconDrawer.DrawBackground(rect);
					skin.box.Draw(rect, new GUIContent("Style"), 0, selected);
				}
				return;
			}

			var icon = AssetIconsPreferences.TypeIcons[assetTarget.Extension];
			if (icon != null && icon.ObjectReference != null)
			{
				bool selected = Selection.Contains(assetTarget.UnityObject);
				rect = AreaToIconRect(rect);

				AssetIconDrawer.DrawBackground(rect);

				AssetIconDrawer.DrawObject(rect, icon, CompiledStyleDefinition.Default, selected);
			}
		}
	}
}

#pragma warning restore
#endif
