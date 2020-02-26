#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using UnityEditor;
using UnityEngine;

#if UNITY_2018_3_OR_NEWER
using System.Collections.Generic;
#endif

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>Integrates support for Unity's preferences menu.</para>
	/// </summary>
	internal static class AssetIconsPreferencesMenu
	{
		private static SerializedObject serializedPreferences;
		private static GUIStyle linkLable;
		private static GUIStyle pageStyle;

		private static readonly string versionString = "Version " + ProductInformation.Version;

		private static SerializedObject SerializedPreferences
		{
			get
			{
				if (serializedPreferences == null
					|| serializedPreferences.targetObject == null
					|| serializedPreferences.targetObject.Equals(null)
					|| AssetIconsPreferences.CurrentPreferences != serializedPreferences.targetObject)
				{
					serializedPreferences = new SerializedObject(AssetIconsPreferences.CurrentPreferences);
				}
				return serializedPreferences;
			}
		}

		private static readonly string[] propertyStrings = new string[]
		{
			"enabled",
			"drawGUIStyles",
			"prefabResolution",
			"selectionTint",
			"typeIcons"
		};

		private static void DrawPreferences(SerializedObject preferences)
		{
			if (linkLable == null)
			{
				linkLable = new GUIStyle(EditorStyles.label);
				linkLable.normal.textColor = new Color(0.0f, 0.0f, 1.0f);
				linkLable.hover.textColor = new Color(0.0f, 0.0f, 1.0f);
				linkLable.active.textColor = new Color(0.5f, 0.0f, 0.5f);
			}
			if (pageStyle == null)
			{
				pageStyle = new GUIStyle
				{
					padding = new RectOffset(12, 12, 0, 0)
				};
			}

			EditorGUILayout.BeginVertical(pageStyle);

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(versionString);

				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Issue Tracker", linkLable))
				{
					Application.OpenURL(ProductInformation.IssueTracker);
				}
			}

			GUILayout.Space(10);

			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Ask a Question"))
				{
					EditorProductInformation.SubmitIssue(EditorProductInformation.IssueType.Question);
				}
				if (GUILayout.Button("Report a Bug"))
				{
					EditorProductInformation.SubmitIssue(EditorProductInformation.IssueType.Bug);
				}
				if (GUILayout.Button("Request a Feature"))
				{
					EditorProductInformation.SubmitIssue(EditorProductInformation.IssueType.Feature);
				}
			}
			if (GUILayout.Button("Check for Updates"))
			{
				AssetIconsUpdateCheckerWindow.Open();
			}

			GUILayout.Space(10);

			preferences.Update();

			EditorGUI.BeginChangeCheck();

			foreach (string propertyString in propertyStrings)
			{
				var property = preferences.FindProperty(propertyString);
				EditorGUILayout.PropertyField(property, new GUIContent(property.displayName, property.tooltip));
			}

			if (EditorGUI.EndChangeCheck())
			{
				preferences.ApplyModifiedProperties();

				EditorPrefs.SetString(AssetIconsPreferences.EditorPrefsKey,
					JsonUtility.ToJson(AssetIconsPreferences.CurrentPreferences));
			}

			EditorGUILayout.EndVertical();
		}

#if UNITY_2018_3_OR_NEWER

		/// <summary>
		/// <para>Implements <see cref="SettingsProvider"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>A settings provider configured to draw AssetIcons preferences.</para>
		/// </returns>
		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			var provider = new SettingsProvider("Preferences/AssetIcons", SettingsScope.User)
			{
				label = "AssetIcons",
				guiHandler = (searchContext) =>
				{
					DrawPreferences(SerializedPreferences);
				},
				keywords = new HashSet<string>(new[] { "AssetIcons", "Icons" })
			};

			return provider;
		}

#else

		/// <summary>
		/// <para>Implements <see cref="PreferenceItem"/>.</para>
		/// </summary>
		[PreferenceItem ("AssetIcons")]
		public static void DrawPreferencesItem()
		{
			DrawPreferences (SerializedPreferences);
		}

#endif
	}
}
#pragma warning restore
#endif
