#if UNITY_EDITOR
#pragma warning disable

using UnityEngine;

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>A collection of resources required for AssetIcons in editor.</para>
	/// </summary>
	/// <remarks>
	/// <para>This class inherits from a utility for providing simple singleton instances for Unity's <see cref="ScriptableObject"/>.</para>
	/// </remarks>
	internal class AssetIconResources : ThemedResourceCollection<AssetIconResources>
	{
#pragma warning disable

		[Header("Preferences Window")]
		[Tooltip("A sprite drawn in the Preferences window to preview effects.")]
		[SerializeField] private Sprite sampleImage;

		[AssetIcon]
		[Tooltip("The icon to use on the updater window.")]
		[SerializeField] private Texture2D updaterIcon;
		[Tooltip("The icon to use to show a version on the update checker.")]
		[SerializeField] private Texture2D circleTexture;

		[Space]
		[Tooltip("The color of the version node passively.")]
		[SerializeField] private Color normalVersionColor;
		[SerializeField] private Color futureVersionColor;

		[Space]
		[Tooltip("The color of the version node when it's not currently installed.")]
		[SerializeField] private Color normalVersionLineColor;
		[Tooltip("The color of the version node connections when one of them are currently installed.")]
		[SerializeField] private Color futureVersionLineColor;

		[Space]
		[Tooltip("The color of the version row passively.")]
		[SerializeField] private Color normalVersionBackgroundColor;
		[Tooltip("The color of the version row it's the current version.")]
		[SerializeField] private Color currentVersionBackgroundColor;
		[Tooltip("The color of the version row when it's not currently installed.")]
		[SerializeField] private Color futureVersionBackgroundColor;

		[Space]
		[Tooltip("The color of the version row passively.")]
		[SerializeField] private Color normalVersionBackgroundHoverColor;
		[Tooltip("The color of the version row when it's the current version.")]
		[SerializeField] private Color currentVersionBackgroundHoverColor;
		[Tooltip("The color of the version row when it's not currently installed.")]
		[SerializeField] private Color futureVersionBackgroundHoverColor;

#pragma warning restore

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for usage with the Light theme.</para>
		/// </summary>
		public override string LightThemePath
		{
			get
			{
				return "AssetIcon Resources (Light)";
			}
		}

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for usage with the Dark theme.</para>
		/// </summary>
		public override string DarkThemePath
		{
			get
			{
				return "AssetIcon Resources (Dark)";
			}
		}

		/// <summary>
		/// <para>A sprite drawn in the Preferences window to preview effects.</para>
		/// </summary>
		public Sprite SampleImage
		{
			get
			{
				return sampleImage;
			}
		}

		/// <summary>
		/// <para>The icon to use on the updater window.</para>
		/// </summary>
		public Texture2D UpdaterIcon
		{
			get
			{
				return updaterIcon;
			}
		}

		/// <summary>
		/// <para>The icon to use to show a version on the update checker.</para>
		/// </summary>
		public Texture2D CircleTexture
		{
			get
			{
				return circleTexture;
			}
		}

		/// <summary>
		/// <para>The color of the version node passively.</para>
		/// </summary>
		public Color NormalVersionColor
		{
			get
			{
				return normalVersionColor;
			}
		}

		/// <summary>
		/// <para>The color of the version node when it's not currently installed.</para>
		/// </summary>
		public Color FutureVersionColor
		{
			get
			{
				return futureVersionColor;
			}
		}

		/// <summary>
		/// <para>The color of the version node connections when both of them are currently installed.</para>
		/// </summary>
		public Color NormalVersionLineColor
		{
			get
			{
				return normalVersionLineColor;
			}
		}

		/// <summary>
		/// <para>The color of the version node connections when one of them are currently installed.</para>
		/// </summary>
		public Color FutureVersionLineColor
		{
			get
			{
				return futureVersionLineColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row passively.</para>
		/// </summary>
		public Color NormalVersionBackgroundColor
		{
			get
			{
				return normalVersionBackgroundColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row it's the current version.</para>
		/// </summary>
		public Color CurrentVersionBackgroundColor
		{
			get
			{
				return currentVersionBackgroundColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row when it's not currently installed.</para>
		/// </summary>
		public Color FutureVersionBackgroundColor
		{
			get
			{
				return futureVersionBackgroundColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row passively.</para>
		/// </summary>
		public Color NormalVersionBackgroundHoverColor
		{
			get
			{
				return normalVersionBackgroundHoverColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row when it's the current version.</para>
		/// </summary>
		public Color CurrentVersionBackgroundHoverColor
		{
			get
			{
				return currentVersionBackgroundHoverColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row when it's not currently installed.</para>
		/// </summary>
		public Color FutureVersionBackgroundHoverColor
		{
			get
			{
				return futureVersionBackgroundHoverColor;
			}
		}
	}
}

#pragma warning restore
#endif
