#if UNITY_EDITOR && (NET_2_0 || NET_2_0_SUBSET || !UNITY_2017_1_OR_NEWER)
#pragma warning disable

namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// <para>Allows you to obtain the method or property name of the caller to the method.</para>
	/// </summary>
	/// <seealso cref="CallerFilePathAttribute"/>
	/// <seealso cref="CallerLineNumberAttribute"/>
	[AttributeUsage (AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerMemberNameAttribute : Attribute
	{
	}

	/// <summary>
	/// <para>Allows you to obtain the line number in the source file at which the method is called.</para>
	/// </summary>
	/// <seealso cref="CallerMemberNameAttribute"/>
	/// <seealso cref="CallerLineNumberAttribute"/>
	[AttributeUsage (AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerFilePathAttribute : Attribute
	{
	}

	/// <summary>
	/// <para>Allows you to obtain the method or property name of the caller to the method.</para>
	/// </summary>
	/// <seealso cref="CallerMemberNameAttribute"/>
	/// <seealso cref="CallerFilePathAttribute"/>
	[AttributeUsage (AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerLineNumberAttribute : Attribute
	{
	}
}

#pragma warning restore
#endif
