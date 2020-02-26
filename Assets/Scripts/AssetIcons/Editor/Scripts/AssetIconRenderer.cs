#if UNITY_EDITOR

#if UNITY_2017_1_OR_NEWER
#define USE_PREVIEW_SCENE
#endif

#pragma warning disable

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>Used by AssetIcons to render Prefabs to a <see cref="Texture2D"/>.</para>
	/// </summary>
	/// <seealso cref="CameraSetup"/>
	public static class AssetIconRenderer
	{
		private const int PREVIEW_LAYER = 22;

		private static readonly List<Renderer> prefabRenderers = new List<Renderer>(64);

		private static Scene renderScene;
		private static Camera renderCamera;

		private static float minX, maxX, minY, maxY;
		private static Plane projectionPlaneHorizontal, projectionPlaneVertical;

		private static Camera RenderCamera
		{
			get
			{
				if (renderCamera == null)
				{
#if USE_PREVIEW_SCENE
					renderScene = EditorSceneManager.NewPreviewScene();
#endif

					renderCamera = EditorUtility.CreateGameObjectWithHideFlags("AssetIcons Camera",
						HideFlags.HideAndDontSave).AddComponent<Camera>();

					renderCamera.enabled = false;
					renderCamera.cullingMask = 1 << PREVIEW_LAYER;
					renderCamera.nearClipPlane = 0.01f;

#if USE_PREVIEW_SCENE
					renderCamera.scene = renderScene;
#endif
					renderCamera.cameraType = CameraType.Preview;
				}

				return renderCamera;
			}
		}

		static AssetIconRenderer()
		{
#if UNITY_5_5_OR_NEWER
			var light0 = CreateLight("AssetIcons Light 0");
			light0.color = new Color(1.0f, 0.95f, 0.9f);
			light0.transform.rotation = Quaternion.Euler(77, 180, 0);
			light0.intensity = 1.15f;

			var light1 = CreateLight("AssetIcons Light 1");
			light1.color = new Color(.4f, .4f, .45f, 0f) * .7f;
			light1.transform.rotation = Quaternion.Euler(340, 218, 177);
			light1.intensity = 1;
#endif
		}

		private static Light CreateLight(string name)
		{
			var lightGameObject = EditorUtility.CreateGameObjectWithHideFlags(name,
				HideFlags.HideAndDontSave, typeof(Light));

#if USE_PREVIEW_SCENE
			SceneManager.MoveGameObjectToScene(lightGameObject, RenderCamera.scene);
#endif

			var light = lightGameObject.GetComponent<Light>();
			light.type = LightType.Directional;
			light.intensity = 1.0f;
			light.enabled = true;
			light.cullingMask = 1 << PREVIEW_LAYER;

			return light;
		}

		/// <summary>
		/// <para>Creates a preview <see cref="Texture2D"/> of a model.</para>
		/// </summary>
		/// <param name="model">The object to render a preview of.</param>
		/// <param name="style">The style to render the model with.</param>
		/// <param name="width">The width (in pixels) of the rendered <see cref="Texture2D"/>.</param>
		/// <param name="height">The height (in pixels) of the rendered <see cref="Texture2D"/>.</param>
		/// <returns>
		/// <para>A rendered preview <see cref="Texture2D"/>.</para>
		/// </returns>
		public static Texture2D RenderModel(GameObject model, CameraSetup style, int width = 64, int height = 64)
		{
			if (model == null || model.Equals(null))
			{
				return null;
			}

#if USE_PREVIEW_SCENE
			var previewObject = (GameObject)PrefabUtility.InstantiatePrefab(model, RenderCamera.scene);
#else
			var previewObject = (GameObject)PrefabUtility.InstantiatePrefab(model);
#endif

			previewObject.gameObject.hideFlags = HideFlags.HideAndDontSave;
			SetLayerRecursively(previewObject.transform);
			previewObject.SetActive(true);

			Texture2D result = null;

			style.ApplyToCamera(RenderCamera);

			try
			{
				var previewDir = previewObject.transform.rotation * style.PreviewDirection.normalized;

				prefabRenderers.Clear();
				previewObject.GetComponentsInChildren(prefabRenderers);

				var previewBounds = new Bounds();
				bool anyRenderers = false;
				for (int i = 0; i < prefabRenderers.Count; i++)
				{
					var renderer = prefabRenderers[i];
					if (!renderer.enabled)
					{
						continue;
					}

					if (!anyRenderers)
					{
						previewBounds = renderer.bounds;
						anyRenderers = true;
					}
					else
					{
						previewBounds.Encapsulate(renderer.bounds);
					}
				}

				if (!anyRenderers)
				{
					return null;
				}

				var boundsCenter = previewBounds.center;
				var boundsExtents = previewBounds.extents;
				var boundsSize = 2f * boundsExtents;

				float aspect = (float)width / height;
				RenderCamera.aspect = aspect;
				RenderCamera.transform.rotation = Quaternion.LookRotation(previewDir, previewObject.transform.up);

				float distance;
				if (style.Orthographic)
				{
					RenderCamera.transform.position = boundsCenter;

					minX = minY = Mathf.Infinity;
					maxX = maxY = Mathf.NegativeInfinity;

					var point = boundsCenter + boundsExtents;
					ProjectBoundingBoxMinMax(point);
					point.x -= boundsSize.x;
					ProjectBoundingBoxMinMax(point);
					point.y -= boundsSize.y;
					ProjectBoundingBoxMinMax(point);
					point.x += boundsSize.x;
					ProjectBoundingBoxMinMax(point);
					point.z -= boundsSize.z;
					ProjectBoundingBoxMinMax(point);
					point.x -= boundsSize.x;
					ProjectBoundingBoxMinMax(point);
					point.y += boundsSize.y;
					ProjectBoundingBoxMinMax(point);
					point.x += boundsSize.x;
					ProjectBoundingBoxMinMax(point);

					distance = boundsExtents.magnitude + 1f;
					RenderCamera.orthographicSize = (1f + style.Padding * 2f) * Mathf.Max(maxY - minY, (maxX - minX) / aspect) * 0.5f;
				}
				else
				{
					projectionPlaneHorizontal = new Plane(RenderCamera.transform.up, boundsCenter);
					projectionPlaneVertical = new Plane(RenderCamera.transform.right, boundsCenter);

					float maxDistance = Mathf.NegativeInfinity;

					var point = boundsCenter + boundsExtents;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.x -= boundsSize.x;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.y -= boundsSize.y;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.x += boundsSize.x;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.z -= boundsSize.z;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.x -= boundsSize.x;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.y += boundsSize.y;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);
					point.x += boundsSize.x;
					maxDistance = RecalculateMaxDistance(maxDistance, point, boundsCenter, aspect, style);

					distance = (1f + style.Padding * 2f) * Mathf.Sqrt(maxDistance);
				}

				RenderCamera.transform.position = boundsCenter - previewDir * distance;
				RenderCamera.farClipPlane = distance * 4f;

				var temp = RenderTexture.active;
				var renderTex = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGB32);
				RenderTexture.active = renderTex;

				if (style.TransparentBackground)
				{
					GL.Clear(false, true, style.BackgroundColor);
				}

				RenderCamera.targetTexture = renderTex;

				RenderCamera.Render();
				RenderCamera.targetTexture = null;

				var textureFormat = style.TransparentBackground ? TextureFormat.ARGB32 : TextureFormat.RGB24;

				result = new Texture2D(width, height, textureFormat, false)
				{
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Point
				};
				result.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
				result.Apply(false, true);

				RenderTexture.active = temp;
				RenderTexture.ReleaseTemporary(renderTex);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				UnityEngine.Object.DestroyImmediate(previewObject);
			}

			return result;
		}

		private static float RecalculateMaxDistance(float maxDistance, Vector3 point, Vector3 boundsCenter, float aspect, CameraSetup style)
		{
			var intersectionPoint = ClosestPointOnPlane(projectionPlaneHorizontal, point);

			float horizontalDistance = projectionPlaneHorizontal.GetDistanceToPoint(point);
			float verticalDistance = projectionPlaneVertical.GetDistanceToPoint(point);

			float halfFrustumHeight = Mathf.Max(verticalDistance, horizontalDistance / aspect);
			float distance = halfFrustumHeight / Mathf.Tan(RenderCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

			float distanceToCenter = (intersectionPoint - style.PreviewDirection.normalized * distance - boundsCenter).sqrMagnitude;
			if (distanceToCenter > maxDistance)
			{
				maxDistance = distanceToCenter;
			}
			return maxDistance;
		}

		private static Vector3 ClosestPointOnPlane(Plane plane, Vector3 point)
		{
			var pointToPlaneDistance = Vector3.Dot(plane.normal, point) + plane.distance;
			return point - (plane.normal * pointToPlaneDistance);
		}

		private static void ProjectBoundingBoxMinMax(Vector3 point)
		{
			var localPoint = RenderCamera.transform.InverseTransformPoint(point);
			if (localPoint.x < minX)
			{
				minX = localPoint.x;
			}

			if (localPoint.x > maxX)
			{
				maxX = localPoint.x;
			}

			if (localPoint.y < minY)
			{
				minY = localPoint.y;
			}

			if (localPoint.y > maxY)
			{
				maxY = localPoint.y;
			}
		}

		private static void SetLayerRecursively(Transform obj)
		{
			obj.gameObject.layer = PREVIEW_LAYER;
			for (int i = 0; i < obj.childCount; i++)
			{
				SetLayerRecursively(obj.GetChild(i));
			}
		}
	}
}

#pragma warning restore
#endif
