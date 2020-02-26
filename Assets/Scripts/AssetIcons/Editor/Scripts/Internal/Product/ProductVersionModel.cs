#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Internal.Product
{
	[Serializable]
	internal class ProductVersionModel
	{
#pragma warning disable
		public string Version;
		public long Timestamp;
#pragma warning restore
	}
}

#pragma warning restore
#endif
