#nullable enable
namespace System.Collections.Generic
{
	public static class EnumerableExtensions
	{
		public static string Join<T>(this IEnumerable<T> source, string delimiter) => string.Join(delimiter, source);
		public static string ToDelimString<T>(this IEnumerable<T> source) => "[" + source.Join(", ") +  "]";
	}
	
	public static class EnumerableOf
	{
		public static IEnumerable<T> Of<T>(T item) { yield return item; }
		public static IEnumerable<T> Of<T>(params T[] items) => items;
	}
}