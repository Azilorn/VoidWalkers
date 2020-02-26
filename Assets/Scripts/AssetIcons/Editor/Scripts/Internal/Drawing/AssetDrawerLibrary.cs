#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Manages all AssetIcon Attributes across the project and uses <see cref="AssetIconDrawer"/> to draw them.</para>
	/// </summary>
	[InitializeOnLoad]
	internal static class AssetDrawerLibrary
	{
		/// <summary>
		/// <para>A mapping of a type to an <c>AssetDrawer</c> for that type.</para>
		/// </summary>
		public static Dictionary<Type, AssetDrawer> AssetDrawers { get; private set; }

		/// <summary>
		/// <para>The binding flags used to search for the AssetIconAttribute.</para>
		/// </summary>
		private static readonly BindingFlags attributeBindingflags = BindingFlags.Instance | BindingFlags.Static |
			BindingFlags.Public | BindingFlags.NonPublic;

		private static MethodInfo unityErrorInvoker;

		/// <summary>
		/// <para>Initializes the <see cref="AssetIconTools"/> class.</para>
		/// </summary>
		static AssetDrawerLibrary()
		{
			AssetDrawers = BuildAssetDrawers();
		}

		/// <summary>
		/// <para>Forces a rebuild of the icon database.</para>
		/// </summary>
		public static void RebuildIconDatabase()
		{
			AssetDrawers = BuildAssetDrawers();
		}

		private static Dictionary<Type, AssetDrawer> BuildAssetDrawers()
		{
			var iconProviders = new Dictionary<Type, AssetDrawer>();

			var checkAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in checkAssemblies)
			{
				Type[] assemblyTypes;
				try
				{
					assemblyTypes = assembly.GetTypes();
				}
				catch
				{
					continue;
				}

				var newProviders = new List<IIconProvider>();

#if !UNITY_2017_1_OR_NEWER
				string assemblyName = assembly.FullName.Substring (0, assembly.FullName.IndexOf (','));

				if (assemblyName != "Assembly-UnityScript" &&
					assemblyName != "Assembly-CSharp" &&
					assemblyName != "Assembly-CSharp-Editor")
					continue;
#endif

				foreach (var type in assemblyTypes)
				{
					if (!typeof(ScriptableObject).IsAssignableFrom(type))
					{
						continue;
					}

					newProviders.Clear();

					var typeFields = type.GetFields(attributeBindingflags);
					var typeProperties = type.GetProperties(attributeBindingflags);
					var typeMethods = type.GetMethods(attributeBindingflags);

					foreach (var field in typeFields)
					{
						object[] attributeObjects = field.GetCustomAttributes(typeof(AssetIconAttribute), true);

						foreach (object attributeObject in attributeObjects)
						{
							var attribute = (AssetIconAttribute)attributeObject;

							if (!IsSupportedType(field.FieldType))
							{
								UnsupportedTypeError(type, attribute, field);
								continue;
							}

							newProviders.Add(new FieldIconProvider(new CompiledStyleDefinition(attribute.Style), field));
						}
					}

					foreach (var property in typeProperties)
					{
						object[] attributeObjects = property.GetCustomAttributes(typeof(AssetIconAttribute), true);

						foreach (object attributeObject in attributeObjects)
						{
							var attribute = (AssetIconAttribute)attributeObject;

							if (!IsSupportedType(property.PropertyType))
							{
								UnsupportedTypeError(type, attribute, property);
								continue;
							}

							newProviders.Add(new MethodIconProvider(new CompiledStyleDefinition(attribute.Style), property.GetGetMethod(true)));
						}
					}

					foreach (var method in typeMethods)
					{
						object[] attributeObjects = method.GetCustomAttributes(typeof(AssetIconAttribute), true);

						foreach (object attributeObject in attributeObjects)
						{
							var attribute = (AssetIconAttribute)attributeObject;

							if (!IsSupportedType(method.ReturnType))
							{
								UnsupportedTypeError(type, attribute, method);
								continue;
							}

							if (IsSupportedParameters(method.GetParameters()))
							{
								UnsupportedParametersError(type, attribute, method);
								continue;
							}

							newProviders.Add(new MethodIconProvider(new CompiledStyleDefinition(attribute.Style), method));
						}
					}

					newProviders.Sort();

					if (newProviders.Count != 0)
					{
						iconProviders.Add(type, new AssetDrawer(newProviders.ToArray()));
					}
				}
			}

			return iconProviders;
		}

		private static void Error(AssetIconAttribute attribute, string message)
		{
			try
			{
				string errorPath = attribute.FilePath;
				int errorLine = attribute.LineNumber;

				if (unityErrorInvoker == null)
				{
					unityErrorInvoker = typeof(Debug).GetMethod("LogPlayerBuildError", BindingFlags.NonPublic | BindingFlags.Static);
				}

				var errorStringBuilder = new StringBuilder();

				errorStringBuilder.Append(message);
				errorStringBuilder.Append('\n');
				errorStringBuilder.Append(errorPath);
				errorStringBuilder.Append(':');
				errorStringBuilder.Append(errorLine.ToString());

				unityErrorInvoker.Invoke(null, new object[] { errorStringBuilder.ToString(), errorPath, errorLine, 0 });
			}
			catch
			{
				Debug.LogError(message);
			}
		}

		private static bool IsSupportedParameters(ParameterInfo[] parameterInfos)
		{
			var parameterTypes = new Type[parameterInfos.Length];

			for (int k = 0; k < parameterInfos.Length; k++)
			{
				parameterTypes[k] = parameterInfos[k].ParameterType;
			}

			return IsSupportedParameters(parameterTypes);
		}

		private static bool IsSupportedParameters(Type[] parameterTypes)
		{
			return parameterTypes.Length != 0;
		}

		private static bool IsSupportedType(Type type)
		{
			return type == typeof(Sprite)
				|| type == typeof(GameObject)
				|| type == typeof(string)
				|| type == typeof(int)
				|| typeof(Texture).IsAssignableFrom(type)
				|| typeof(Color).IsAssignableFrom(type)
				|| typeof(Color32).IsAssignableFrom(type);
		}

		private static void UnsupportedParametersError(Type type, AssetIconAttribute attribute, MethodInfo method)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + type.Name + "\"s method \"" + method.Name + "\" with parameters that is not supported by the AssetIcon attribute.\n" +
				"Please use the AssetIcon attribute on a method with no parameters";

			Error(attribute, message);
		}

		private static void UnsupportedTypeError(Type type, AssetIconAttribute attribute, MethodInfo member)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + type.Name + "\"s method \"" + member.Name + "\" of type \"" + member.ReturnType + "\".";

			Error(attribute, message);
		}

		private static void UnsupportedTypeError(Type type, AssetIconAttribute attribute, FieldInfo member)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + type.Name + "\"s field \"" + member.Name + "\" of type \"" + member.FieldType + "\".";

			Error(attribute, message);
		}

		private static void UnsupportedTypeError(Type type, AssetIconAttribute attribute, PropertyInfo member)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + type.Name + "\"s property \"" + member.Name + "\" of type \"" + member.PropertyType + "\".";

			Error(attribute, message);
		}
	}
}

#pragma warning restore
#endif
