#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Product;
using AssetIcons.Editors.Preferences;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawers
{
	/// <summary>
	/// <para>A custom property drawer for the <see cref="ColorTint"/>.</para>
	/// </summary>
	[CustomPropertyDrawer(typeof(ColorTint))]
	internal class ColorTintDrawer : PropertyDrawer
	{
		private CompiledStyleDefinition tintedStyle;
		private CompiledStyleDefinition untintedStyle;

#if UNITY_2017_3_OR_NEWER
		public override bool CanCacheInspectorGUI(SerializedProperty property)
		{
			return false;
		}
#endif

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 108;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			if (untintedStyle == null)
			{
				untintedStyle = new CompiledStyleDefinition(new StyleDefinition()
				{
					MaxSize = 64
				});

				tintedStyle = new CompiledStyleDefinition(new StyleDefinition()
				{
					MaxSize = 64
				});
			}

			var sliderRect = new Rect(position.x, position.y + 12, position.width, EditorGUIUtility.singleLineHeight);
			var previewArea = new Rect(position.x, sliderRect.yMax + 12, position.width, position.yMax - 24 - sliderRect.yMax);

			var untintedRect = new Rect(previewArea.x, previewArea.y, (previewArea.width * 0.5f) - 3.0f, previewArea.height);
			var tintedRect = new Rect(untintedRect.xMax + 6.0f, previewArea.y, untintedRect.width, previewArea.height);

			var tintStrengthProperty = property.FindPropertyRelative("tintStrength");

			tintStrengthProperty.floatValue = EditorGUI.Slider(sliderRect, label,
				tintStrengthProperty.floatValue, 0.0f, 1.0f);

			if (Event.current.type == EventType.Repaint)
			{
				var tint = new ColorTint(tintStrengthProperty.floatValue * 1.0f);
				tintedStyle.Tint = tint.Apply(Color.white);

				AssetIconDrawer.DrawSprite(untintedRect, AssetIconResources.CurrentTheme.SampleImage, untintedStyle, false);
				AssetIconDrawer.DrawSprite(tintedRect, AssetIconResources.CurrentTheme.SampleImage, tintedStyle, false);
			}

			EditorGUI.EndProperty();
		}
	}
}

#pragma warning restore
#endif
