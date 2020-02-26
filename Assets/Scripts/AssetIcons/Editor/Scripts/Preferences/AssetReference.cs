#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEditor;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>Used to reference assets inside the Unity project inside the editor.</para>
	/// </summary>
	/// <remarks>
	/// <para>This is used inside the <see cref="PreferencesPreset"/> to select assets to render an icon.</para>
	/// </remarks>
	[Serializable]
	public sealed class AssetReference
	{
		/// <summary>
		/// <para>A path, relative to the Unity project root, used to reference the asset.</para>
		/// </summary>
		public string AssetPath;

		/// <summary>
		/// <para>The <see cref="UnityEngine.Object.name"/> of the asset.</para>
		/// </summary>
		public string AssetName;

		[NonSerialized]
		private string objectAssetPath;

		[NonSerialized]
		private string objectAssetName;

		[NonSerialized]
		private UnityEngine.Object objectReference;

		/// <summary>
		/// <para>Load or assign assets from the <see cref="AssetDatabase"/>.</para>
		/// </summary>
		public UnityEngine.Object ObjectReference
		{
			get
			{
				if (string.IsNullOrEmpty(AssetPath))
				{
					return null;
				}

				if (objectReference == null
					|| objectReference.Equals(null)
					|| objectAssetPath != AssetPath
					|| objectAssetName != AssetName)
				{
					objectAssetPath = AssetPath;
					objectAssetName = AssetName;
					objectReference = GetFromPath(AssetPath, AssetName);
				}

				return objectReference;
			}
			set
			{
				objectAssetPath = null;
				objectAssetName = null;
				objectReference = null;

				if (value != null || value.Equals(null))
				{
					AssetPath = AssetDatabase.GetAssetPath(value);
					AssetName = value.name;
				}
			}
		}

		/// <summary>
		/// <para>Loads an asset from the <see cref="AssetDatabase"/> with the specified name.</para>
		/// </summary>
		/// <param name="assetPath">A path, relative to the Unity project root, used to reference the asset.</param>
		/// <param name="assetName">The <see cref="UnityEngine.Object.name"/> of the asset.</param>
		/// <returns>
		/// <para>This the same logic as an <see cref="AssetReference"/> would use to locate an asset, used in editor scripting.</para>
		/// </returns>
		public static UnityEngine.Object GetFromPath(string assetPath, string assetName)
		{
			if (assetPath.EndsWith(".unity"))
			{
				return null;
			}

			var references = AssetDatabase.LoadAllAssetsAtPath(assetPath);

			if (references.Length == 0)
			{
				return null;
			}

			foreach (var reference in references)
			{
				if (reference.name == assetName)
				{
					return reference;
				}
			}

			return references[0];
		}
	}
}

#pragma warning restore
#endif
