#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Internal.Product
{
	[Serializable]
	internal class ProductStatusModel
	{
#pragma warning disable
		public string CurrentVersion;
		public string WebChangelogSource;
		public string JsonChangelogSource;
		public ProductVersionModel[] VersionHistory;
#pragma warning restore
	}
}

#pragma warning restore
#endif
