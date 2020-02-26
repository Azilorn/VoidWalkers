#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using System;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>Handles the drawing, sizing and tinting of the assets.</para>
	/// </summary>
	/// <seealso cref="CompiledStyleDefinition"/>
	public static class AssetIconDrawer
	{
		private struct GeneralDrawInfo
		{
			public bool Display;
			public Color Tint;
			public Rect Position;
		}

		private const bool SELECTION_TINT = true;

		private static GUIStyle textStyle;

		private static GUIStyle TextStyle
		{
			get
			{
				if (textStyle == null)
				{
					textStyle = new GUIStyle
					{
						alignment = TextAnchor.MiddleCenter,
						fontStyle = FontStyle.Normal
					};
				}
				return textStyle;
			}
		}

		/// <summary>
		/// <para>A <see cref="Color"/> that imitates Unity's project window background.</para>
		/// </summary>
		/// <remarks>
		/// <para>This <see cref="Color"/> is used for masking the icon that Unity draws. </para>
		/// <para>This changes with Untiy's pro-skin across differnet versions of Unity.</para>
		/// </remarks>
		public static Color BackgroundColor
		{
			get
			{
#if UNITY_2019_3_OR_NEWER
				return EditorGUIUtility.isProSkin
					? (Color)new Color32(51, 51, 51, 255)
					: (Color)new Color32(190, 190, 190, 255);
#else
				return EditorGUIUtility.isProSkin
					? (Color)new Color32 (56, 56, 56, 255)
					: (Color)new Color32 (194, 194, 194, 255);
#endif
			}
		}

		/// <summary>
		/// <para>Draws an empty background using <see cref="BackgroundColor"/> to hide Unity's original icon.</para>
		/// </summary>
		/// <remarks>
		/// <para>This <see cref="Color"/> is used for masking the icon that Unity draws. </para>
		/// <para>This changes with Untiy's pro-skin across differnet versions of Unity.</para>
		/// </remarks>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		public static void DrawBackground(Rect rect)
		{
			EditorGUI.DrawRect(rect, BackgroundColor);
		}

		/// <summary>
		/// <para>Draw a flat <see cref="Color"/>.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="color">A <see cref="Color"/> to draw in the provided <see cref="Rect"/>.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		public static void DrawColor(Rect rect, Color color,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			if (Mathf.Abs(color.a) < 0.005f)
			{
				return;
			}

			var info = ApplyGeneralModifiers(rect, style);
			if (!info.Display)
			{
				return;
			}

			// Take the edge off that color, we still want to resemble our original color.
			var multiplyColor = Color.Lerp(info.Tint, Color.white, 0.5f);
			multiplyColor.a = info.Tint.a;

			// Blend the tint color into our color
			color *= multiplyColor;

			if (selected)
			{
				color = AssetIconsPreferences.SelectionTint.Value.Apply(color, 0.5f);
			}

			EditorGUI.DrawRect(info.Position, color);
		}

		/// <summary>
		/// <para>Attempts to draw an icon for an object.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="objectIcon">The target object to draw a graphic for.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		public static void DrawObject(Rect rect, object objectIcon,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			if (objectIcon == null)
			{
				return;
			}

			if (objectIcon.GetType() == typeof(AssetReference))
			{
				var assetReference = (AssetReference)objectIcon;
				DrawObject(rect, assetReference.ObjectReference, style, selected);
				return;
			}

			var iconType = objectIcon.GetType();

			if (typeof(Sprite).IsAssignableFrom(iconType))
			{
				var spriteIcon = (Sprite)objectIcon;

				if (spriteIcon != null)
				{
					DrawSprite(rect, spriteIcon, style, selected);
				}
			}
			else if (typeof(Texture).IsAssignableFrom(iconType))
			{
				var textureIcon = (Texture)objectIcon;

				if (textureIcon != null)
				{
					DrawTexture(rect, textureIcon, style, selected);
				}
			}
			else if (typeof(Color).IsAssignableFrom(iconType))
			{
				var color = (Color)objectIcon;

				DrawColor(rect, color, style, selected);
			}
			else if (typeof(Color32).IsAssignableFrom(iconType))
			{
				var color = (Color32)objectIcon;

				DrawColor(rect, color, style, selected);
			}
			else if (typeof(string).IsAssignableFrom(iconType))
			{
				string iconText = (string)objectIcon;

				if (!string.IsNullOrEmpty(iconText))
				{
					DrawText(rect, iconText, style, selected);
				}
			}
			else if (typeof(int).IsAssignableFrom(iconType))
			{
				int iconText = (int)objectIcon;

				DrawText(rect, iconText.ToString(), style, selected);
			}
			else if (typeof(GameObject).IsAssignableFrom(iconType))
			{
				var prefab = (GameObject)objectIcon;

				if (prefab != null)
				{
					var col = BackgroundColor;
					col = new Color(col.r, col.g, col.b, 0);

					var setup = new CameraSetup()
					{
						BackgroundColor = col,
						TransparentBackground = true,
						Orthographic = style.Projection == IconProjection.Orthographic,

						PreviewDirection = new Vector3(-1.0f, -1.0f, -1.0f)
					};

					var thumbnail = RenderCache.GetTexture(setup, prefab);

					DrawTexture(rect, thumbnail, style, selected);
				}
			}
			else
			{
				throw new InvalidOperationException("Could not draw icon of type " + iconType);
			}
		}

		/// <summary>
		/// <para>Draws a sprite graphic at a specified <see cref="Rect"/>.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="sprite">The graphic to draw.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		/// <remarks>
		/// <para>This implementation draws a <see cref="Texture2D"/> and crops the boundaries. This may result in
		/// sprites that are tightly packed to draw artifacts.</para>
		/// </remarks>
		public static void DrawSprite(Rect rect, Sprite sprite,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (sprite == null || sprite.Equals(null) || Event.current.type != EventType.Repaint)
			{
				return;
			}

			Texture2D textureIcon = null;
			Rect textureRect;

			if (sprite.packed)
			{
				textureIcon = SpriteUtility.GetSpriteTexture(sprite, false);
			}

			if (textureIcon == null)
			{
				textureIcon = sprite.texture;
			}

			textureRect = sprite.rect;

			DrawTexWithCoords(rect, textureIcon, textureRect, style, selected);
		}

		/// <summary>
		/// <para>Draws text at a specified <see cref="Rect"/>.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="text">The text to draw for the asset icon.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		public static void DrawText(Rect rect, string text,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			var originalColor = GUI.color;

			var guiStyle = new GUIStyle(EditorStyles.label)
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold
			};

			if (style != null)
			{
				bool display;
				rect = style.Filter(rect, out display);

				if (!display)
				{
					return;
				}

				GUI.color = style.Tint;

				TextStyle.normal.textColor = style.Tint;
				TextStyle.alignment = style.TextAnchor;

				TextStyle.fontStyle = style.FontStyle;
			}
			else
			{
				GUI.color = Color.black;

				TextStyle.alignment = TextAnchor.MiddleCenter;
				TextStyle.fontStyle = FontStyle.Normal;
			}

			TextStyle.fontSize = Mathf.FloorToInt(rect.height * 0.3f);

			if (selected)
			{
				GUI.color = AssetIconsPreferences.SelectionTint.Value.Apply(GUI.color);
			}

			EditorGUI.LabelField(rect, text, TextStyle);

			GUI.color = originalColor;
		}

		/// <summary>
		/// <para>Draws a texture graphic at a specified <see cref="Rect"/>.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="texture">The texture to draw.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		public static void DrawTexture(Rect rect, Texture texture,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (texture == null || texture.Equals(null) || Event.current.type != EventType.Repaint)
			{
				return;
			}

			var textureRect = new Rect(0.0f, 0.0f, texture.width, texture.height);

			DrawTexWithCoords(rect, texture, textureRect, style, selected);
		}

		/// <summary>
		/// <para>Draws a portion of a texture at a specified <see cref="Rect"/>.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="texture">The texture to draw.</param>
		/// <param name="textureRect">The rect (in pixels) of the texture to draw.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		public static void DrawTexWithCoords(Rect rect, Texture texture, Rect textureRect,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (texture == null || texture.Equals(null) || Event.current.type != EventType.Repaint)
			{
				return;
			}

			var normalisedTextureRect = RemapRect(textureRect, texture.width, texture.height);

			DrawTexWithUVCoords(rect, texture, normalisedTextureRect, style, selected);
		}

		/// <summary>
		/// <para>Draws a portion of a texture at a specified <see cref="Rect"/>.</para>
		/// </summary>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="texture">The texture to draw.</param>
		/// <param name="uvCoords">The <see cref="Rect"/> (in normalised texture coordinates) of the <see cref="Texture"/> to draw.</param>
		/// <param name="style">A style used to modify the appearance of a graphic.</param>
		/// <param name="selected">Whether the graphic should be rendered with a selection tint.</param>
		public static void DrawTexWithUVCoords(Rect rect, Texture texture, Rect uvCoords,
			CompiledStyleDefinition style = null, bool selected = false)
		{
			if (texture == null || texture.Equals(null) || Event.current.type != EventType.Repaint)
			{
				return;
			}

			var info = ApplyGeneralModifiers(rect, style);
			if (!info.Display)
			{
				return;
			}

			if (style != null)
			{
				// Get the targets aspect ratio
				float aspectRatio = uvCoords.width * texture.width / (uvCoords.height * texture.height);

				// Apply aspect handling
				if (style.Aspect != IconAspect.Stretch)
				{
					info.Position = ForceAspectRatio(info.Position, aspectRatio,
						style.Aspect == IconAspect.Envelop);
				}
			}
			else
			{
				info.Position = ForceAspectRatio(info.Position,
					texture.width * uvCoords.width / (texture.height / uvCoords.height));
			}

			var originalColor = GUI.color;
			GUI.color = info.Tint;

			if (SELECTION_TINT && selected)
			{
				GUI.color = AssetIconsPreferences.SelectionTint.Value.Apply(GUI.color);
			}

			GUI.DrawTextureWithTexCoords(info.Position, texture, uvCoords, true);

			GUI.color = originalColor;
		}

		private static GeneralDrawInfo ApplyGeneralModifiers(Rect rect,
			CompiledStyleDefinition style)
		{
			var info = new GeneralDrawInfo();

			if (style != null)
			{
				info.Position = style.Filter(rect, out info.Display);
				info.Tint = style.Tint;
			}
			else
			{
				info.Position = rect;
				info.Tint = Color.white;
				info.Display = true;
			}

			return info;
		}

		/// <summary>
		/// <para>Makes the bounds fit into the other <see cref="Rect"/> whilst preserving supplied aspect ratio.</para>
		/// </summary>
		/// <returns>
		/// <para>The <see cref="Rect"/> fit inside the bounds with the aspect ratio supplied.</para>
		/// </returns>
		/// <param name="bounds">The <see cref="Rect"/> the result rect should fit in.</param>
		/// <param name="ratio">The desired aspect ratio of the returned <see cref="Rect"/>.</param>
		/// <param name="envelop">If <c>true</c>, the returned <see cref="Rect"/> may stretch outside of the bounds, otherwise it remains inside.</param>
		private static Rect ForceAspectRatio(Rect bounds, float ratio, bool envelop = false)
		{
			var newRect = new Rect(bounds);

			float originalWidth = newRect.width;

			newRect.width = bounds.height * ratio;

			if (newRect.width < bounds.width == envelop)
			{
				float originalHeight = newRect.height;

				newRect.width = bounds.width;
				newRect.height = bounds.width / ratio;

				float heightDifference = originalHeight - newRect.height;
				newRect.y += heightDifference / 2;
			}
			else
			{
				float widthDifference = originalWidth - newRect.width;
				newRect.x += widthDifference / 2;
			}

			return newRect;
		}

		/// <summary>
		/// <para>Remaps the <see cref="Rect"/> to the scale provided.</para>
		/// </summary>
		/// <returns>
		/// <para>The <see cref="Rect"/> remaped to the scale provided.</para>
		/// </returns>
		/// <param name="rect">The source <see cref="Rect"/>.</param>
		/// <param name="width">Maximum width to remap to.</param>
		/// <param name="height">Maximum height to remap to.</param>
		private static Rect RemapRect(Rect rect, float width, float height)
		{
			var remapedRect = new Rect(rect);

			remapedRect.x /= width;
			remapedRect.width /= width;

			remapedRect.y /= height;
			remapedRect.height /= height;

			return remapedRect;
		}
	}
}

#pragma warning restore
#endif
