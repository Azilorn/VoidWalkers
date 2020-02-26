#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Drawing;
using AssetIcons.Editors.Preferences;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawers
{
	/// <summary>
	/// <para>A custom property drawer for the <see cref="IconMapping"/>.</para>
	/// </summary>
	[CustomPropertyDrawer(typeof(IconMapping), true)]
	internal class IconMappingDrawer : PropertyDrawer
	{
		private static GUIStyle smallPathText;
		private static GUIStyle smallWarningText;
		private static Texture warningIcon;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var keyValuePairsProperty = property.FindPropertyRelative("keyValuePairs");

			return ((EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2) +
				((EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 12) * keyValuePairsProperty.arraySize);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			if (smallPathText == null)
			{
				smallPathText = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
				{
					alignment = TextAnchor.UpperRight,
					fontSize = 8,
					padding = new RectOffset(4, 0, 0, 0),
					margin = new RectOffset()
				};
			}
			if (smallWarningText == null)
			{
				smallWarningText = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
				{
					alignment = TextAnchor.UpperLeft,
					fontSize = 8,
					padding = new RectOffset(0, 4, 0, 0),
					margin = new RectOffset()
				};
				warningIcon = EditorGUIUtility.IconContent("console.warnicon.sml", "").image;
			}

			var keyValuePairsProperty = property.FindPropertyRelative("keyValuePairs");

			var marchingRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.LabelField(marchingRect, label);

			marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

			for (int i = 0; i < keyValuePairsProperty.arraySize; i++)
			{
				var keyValuePairProperty = keyValuePairsProperty.GetArrayElementAtIndex(i);
				var keyProperty = keyValuePairProperty.FindPropertyRelative("key");
				var valueProperty = keyValuePairProperty.FindPropertyRelative("value");

				var deleteRect = new Rect(marchingRect.x, marchingRect.y, marchingRect.height, marchingRect.height);
				var keyRect = new Rect(marchingRect.x + marchingRect.height, marchingRect.y, 120, marchingRect.height);
				var valueRect = new Rect(keyRect.xMax + 2, marchingRect.y, marchingRect.xMax - keyRect.xMax - 6, marchingRect.height);

				if (GUI.Button(deleteRect, "", "OL Minus"))
				{
					keyValuePairsProperty.DeleteArrayElementAtIndex(i);
					return;
				}

				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none);
				if (EditorGUI.EndChangeCheck())
				{
					keyProperty.stringValue = keyProperty.stringValue
						.ToLower()
						.Trim();

					int index = keyProperty.stringValue.LastIndexOf('.');
					if (index == -1)
					{
						keyProperty.stringValue = "." + keyProperty.stringValue;
					}
					else if (index != 0)
					{
						keyProperty.stringValue = keyProperty.stringValue.Substring(index);
					}
				}

				var pathProperty = valueProperty.FindPropertyRelative("AssetPath");
				var nameProperty = valueProperty.FindPropertyRelative("AssetName");

				var loaded = AssetReference.GetFromPath(pathProperty.stringValue, nameProperty.stringValue);

				EditorGUI.BeginChangeCheck();
				loaded = EditorGUI.ObjectField(valueRect, loaded, typeof(Object), false);
				if (EditorGUI.EndChangeCheck())
				{
					if (loaded == null)
					{
						pathProperty.stringValue = string.Empty;
						nameProperty.stringValue = string.Empty;
					}
					else
					{
						pathProperty.stringValue = AssetDatabase.GetAssetPath(loaded);
						nameProperty.stringValue = loaded.name;
					}
				}

				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

				var smallTextRect = new Rect(marchingRect)
				{
					height = 10
				};

				if (string.IsNullOrEmpty(pathProperty.stringValue))
				{
					EditorGUI.LabelField(smallTextRect, "None", smallPathText);
				}
				else
				{
					EditorGUI.LabelField(smallTextRect, pathProperty.stringValue + ", " + nameProperty.stringValue, smallPathText);
				}

				var iconRect = new Rect(smallTextRect)
				{
					width = smallTextRect.height,
					x = smallTextRect.x + marchingRect.height
				};
				smallTextRect.xMin = iconRect.xMax + 2;

				bool alreadyFound = false;
				for (int k = 0; k < keyValuePairsProperty.arraySize; k++)
				{
					if (k == i)
					{
						continue;
					}

					var otherKeyProperty = keyValuePairsProperty.GetArrayElementAtIndex(k).FindPropertyRelative("key");

					if (otherKeyProperty.stringValue == keyProperty.stringValue)
					{
						alreadyFound = true;
						break;
					}
				}
				if (alreadyFound)
				{
					GUI.DrawTexture(iconRect, warningIcon);
					EditorGUI.LabelField(smallTextRect,
						"Duplicate", smallWarningText);
				}
				else if (AssetIconTools.UnsupportedExtensions.Contains(keyProperty.stringValue))
				{
					GUI.DrawTexture(iconRect, warningIcon);
					EditorGUI.LabelField(smallTextRect,
						"Unsupported", smallWarningText);
				}
				else
				{
					EditorGUI.LabelField(smallTextRect,
						"", smallWarningText);
				}

				marchingRect.y += 12;
			}

			var addRect = new Rect(marchingRect.xMax - 20, marchingRect.y, 20, marchingRect.height);

			if (GUI.Button(addRect, "", "OL Plus"))
			{
				keyValuePairsProperty.arraySize++;
			}

			EditorGUI.EndProperty();
		}
	}
}

#pragma warning restore
#endif
