#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>A camera setup used in <see cref="AssetIconRenderer"/> to render a Prefab.</para>
	/// </summary>
	/// <seealso cref="AssetIconRenderer"/>
	/// <seealso cref="RenderCache"/>
	[Serializable]
	public struct CameraSetup : IEquatable<CameraSetup>
	{
		/// <summary>
		/// <para>A set of default settings to render a Prefab with.</para>
		/// </summary>
		public static readonly CameraSetup Default = new CameraSetup()
		{
			Padding = 0.0f,

			TransparentBackground = false,
			BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f),

			Orthographic = false,
			PreviewDirection = new Vector3(-1f, -1f, -1f)
		};

		/// <summary>
		/// <para>Padding around the edges of a rendered Prefab.</para>
		/// </summary>
		[Header("Positioning")]
		public float Padding;

		/// <summary>
		/// <para>The vector direction to render a Prefab from.</para>
		/// </summary>
		public Vector3 PreviewDirection;

		/// <summary>
		/// <para>Indicates whether the background should be rendered as transparent.</para>
		/// </summary>
		[Header("Camera Settings")]
		public bool TransparentBackground;

		/// <summary>
		/// <para>The background color of the rendered graphic.</para>
		/// </summary>
		public Color BackgroundColor;

		/// <summary>
		/// <para>Indicates whether projection should be orthographic.</para>
		/// </summary>
		public bool Orthographic;

		/// <summary>
		/// <para>Applies this <see cref="CameraSetup"/> to a Unity <see cref="Camera"/>.</para>
		/// </summary>
		/// <param name="camera">The Unity <see cref="Camera"/> to be modified.</param>
		public void ApplyToCamera(Camera camera)
		{
			camera.orthographic = Orthographic;
			camera.backgroundColor = BackgroundColor;

			camera.clearFlags = TransparentBackground ? CameraClearFlags.Depth : CameraClearFlags.Color;
		}

		/// <summary>
		/// <para>Evaluates whether this <see cref="CameraSetup"/> is equal to an <see cref="object"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="object"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="object"/> is a <see cref="CameraSetup"/>s and is equal to this; otherwise <c>false</c>.</para>
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is CameraSetup && Equals((CameraSetup)obj);
		}

		/// <summary>
		/// <para>Evaluates whether this <see cref="CameraSetup"/> is equal to another <see cref="CameraSetup"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="CameraSetup"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="CameraSetup"/>s are equal; otherwise <c>false</c>.</para>
		/// </returns>
		public bool Equals(CameraSetup other)
		{
			return Padding == other.Padding &&
				   EqualityComparer<Vector3>.Default.Equals(PreviewDirection, other.PreviewDirection) &&
				   TransparentBackground == other.TransparentBackground &&
				   EqualityComparer<Color>.Default.Equals(BackgroundColor, other.BackgroundColor) &&
				   Orthographic == other.Orthographic;
		}

		/// <summary>
		/// <para>Returns a unique hash for this of <see cref="CameraSetup"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>An <see cref="int"/> that represents a unique has of this instance.</para>
		/// </returns>
		public override int GetHashCode()
		{
			int hashCode = -1977805934;
			hashCode = hashCode * -1521134295 + Padding.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(PreviewDirection);
			hashCode = hashCode * -1521134295 + TransparentBackground.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(BackgroundColor);
			hashCode = hashCode * -1521134295 + Orthographic.GetHashCode();
			return hashCode;
		}

		/// <summary>
		/// <para>Evaluates whether a <see cref="CameraSetup"/> is equal to another <see cref="CameraSetup"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="CameraSetup"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="CameraSetup"/>s are equal; otherwise <c>false</c>.</para>
		/// </returns>
		public static bool operator ==(CameraSetup left, CameraSetup right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// <para>Evaluates whether a <see cref="CameraSetup"/> is not equal to another <see cref="CameraSetup"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="CameraSetup"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="CameraSetup"/>s are not equal; otherwise <c>false</c>.</para>
		/// </returns>
		public static bool operator !=(CameraSetup left, CameraSetup right)
		{
			return !(left == right);
		}
	}
}

#pragma warning restore
#endif
