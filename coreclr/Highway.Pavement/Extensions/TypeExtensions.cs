using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Highway.Pavement
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Checks if the <see cref="T:System.Type"/> implements the <see cref="T:System.Collections.ICollection"/> interface.
		/// 
		/// </summary>
		/// <param name="type">The <see cref="T:System.Type"/> to inspect.</param>
		/// <returns>
		/// <see langword="true"/> if the type implements the <see cref="T:System.Collections.ICollection"/> interface; otherwise <see langword="false"/>.
		/// </returns>
		public static bool IsCollection(this Type type)
		{
			return type.GetInterfaces().Where(x => x.GetTypeInfo().IsGenericType).Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>));
		}

		/// <summary>
		/// If the <paramref name="type"/> is a generic enumerable, returns the Type Argument for the given type.  Otherwise, returns the given type.
		/// </summary>
		/// <param name="type">The type to examine.</param>
		/// <returns>The Type Argument for the given type if the <paramref name="type"/> is a generic enumerable.  Otherwise, returns the given type.</returns>
		public static Type ToSingleType(this Type type)
		{
			if (type.GetTypeInfo().IsGenericType && type.IsEnumerable())
			{
				return type.GetGenericArguments().Single();
			}
			return type;
		}

		/// <summary>
		/// Checks if the <see cref="T:System.Type"/> implements the <see cref="T:System.Collections.IEnumerable"/> interface.
		/// 
		/// </summary>
		/// <param name="type">The <see cref="T:System.Type"/> to inspect.</param>
		/// <returns>
		/// <see langword="true"/> if the type implements the <see cref="T:System.Collections.IEnumerable"/> interface; otherwise <see langword="false"/>.
		/// </returns>
		/// 
		/// <remarks>
		/// The method will return <see langword="false"/> for <see cref="T:System.String"/> type.
		/// </remarks>
		public static bool IsEnumerable(this Type type)
		{
			if (type == typeof(string))
			{
				return false;
			}

			return type == typeof(IEnumerable) || type.GetInterfaces().Contains(typeof(IEnumerable));
		}

	}
}