#if UNITY_EDITOR
#pragma warning disable

using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Contains the information of the target used in the modules.</para>
	/// </summary>
	internal struct AssetTarget
	{
		private Object objectCache;
		private readonly string extension;
		private readonly string filename;
		private readonly string filePath;
		private readonly string guid;

		/// <summary>
		/// <para>The file extension of the asset.</para>
		/// </summary>
		public string Extension
		{
			get
			{
				return extension;
			}
		}

		/// <summary>
		/// <para>The file name of the asset.</para>
		/// </summary>
		public string Filename
		{
			get
			{
				return filename;
			}
		}

		/// <summary>
		/// <para>The full file path of the asset.</para>
		/// </summary>
		public string FilePath
		{
			get
			{
				return filePath;
			}
		}

		/// <summary>
		/// <para>The GUID of the asset.</para>
		/// </summary>
		public string GUID
		{
			get
			{
				return guid;
			}
		}

		/// <summary>
		/// <para>A cached load of the Asset from the Unity AssetDatabase.</para>
		/// </summary>
		public Object UnityObject
		{
			get
			{
				if (objectCache == null)
				{
					objectCache = AssetDatabase.LoadAssetAtPath<Object>(FilePath);
				}

				return objectCache;
			}
		}

		private AssetTarget(string extension, string filename, string filePath, string guid)
		{
			this.extension = extension;
			this.filename = filename;
			this.filePath = filePath;
			this.guid = guid;

			objectCache = null;
		}

		/// <summary>
		/// <para>Creates an <c>AssetTarget</c> from a guid.</para>
		/// </summary>
		/// <param name="guid">The guid of the asset.</param>
		/// <returns>
		/// <para>An <c>AssetTarget</c> representing an asset with the specified guid.</para>
		/// </returns>
		public static AssetTarget CreateFromGUID(string guid)
		{
			string filePath = AssetDatabase.GUIDToAssetPath(guid);

			return new AssetTarget(Path.GetExtension(filePath), Path.GetFileName(filePath), filePath, guid);
		}

		/// <summary>
		/// <para>Creates an <c>AssetTarget</c> from a local file path.</para>
		/// </summary>
		/// <param name="path">The local file path of the asset.</param>
		/// <returns>
		/// <para>An <c>AssetTarget</c> representing an asset at the specified path.</para>
		/// </returns>
		public static AssetTarget CreateFromPath(string path)
		{
			return new AssetTarget(Path.GetExtension(path), Path.GetFileName(path), path, AssetDatabase.AssetPathToGUID(path));
		}
	}
}

#pragma warning restore
#endif
