#nullable enable
using System.Linq;

namespace System.Collections.Generic
{
	public static class EnumerableExtensions
	{
		public static string Join<T>(this IEnumerable<T> source, string delimiter) => string.Join(delimiter, source);
		public static string Join(this IEnumerable<string> source) => string.Join("", source);
		
		public static string ToDelimString<T>(this IEnumerable<T> source) => "[" + source.Join(", ") +  "]";
		
		public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();
		
		public static IEnumerable<T> Without<T>(this IEnumerable<T> source, T obj, IEqualityComparer<T> comparer) => source.Where(item => !comparer.Equals(item, obj));
		public static IEnumerable<T> Without<T>(this IEnumerable<T> source, T obj) => source.Without(obj, EqualityComparer<T>.Default);
	}
	
	public static class EnumerableOf
	{
		public static IEnumerable<T> Of<T>(T item) { yield return item; }
		public static IEnumerable<T> Of<T>(params T[] items) => items;
	}
}