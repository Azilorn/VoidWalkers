#if UNITY_EDITOR
#pragma warning disable

using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawers
{
	/// <summary>
	/// <para>A custom property drawer for the <see cref="AssetIconAttribute"/>.</para>
	/// <para>The project window repaints whenever the value of the property is changed by this property drawer.</para>
	/// </summary>
	[CustomPropertyDrawer(typeof(AssetIconAttribute))]
	internal class AssetIconAttributeDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.PropertyField(position, property, label);

			EditorGUI.EndProperty();
			if (EditorGUI.EndChangeCheck())
			{
				EditorApplication.RepaintProjectWindow();
			}
		}
	}
}

#pragma warning restore
#endif
