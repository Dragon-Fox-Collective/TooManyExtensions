#nullable enable
using System.Linq;

namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		public static T Pop<T>(this IList<T> source)
		{
			T obj = source[0];
			source.RemoveAt(0);
			return obj;
		}
		
		public static T RandomElement<T>(this IList<T> source) => RandomExtensions.Instance.Choose(source);
		public static void AddRange<T>(this IList<T> source, int count, Func<T> factory) => Enumerable.Range(0, count).Select(_ => factory()).ForEach(source.Add);
		
		public static void Move<T>(this IList<T> source, T item, int index)
		{
			source.Remove(item);
			source.Insert(index, item);
		}
		
		public static void Shuffle<T>(this IList<T?> source)
		{
			for (int n = 0; n < source.Count; n++)
				source.Swap(n, RandomExtensions.Instance.Next(source.Count));
		}
		
		public static void Swap<T>(this IList<T?> source, int i1, int i2)
		{
			if (i1 == i2) return;
			int maxIndex = Math.Max(i1, i2);
			int minIndex = Math.Min(i1, i2);
			source.RemoveAt(maxIndex, out T? maxItem);
			source.RemoveAt(minIndex, out T? minItem);
			source.Insert(minIndex, maxItem);
			source.Insert(maxIndex, minItem);
		}
		
		public static bool RemoveAt<T>(this IList<T?> source, int index, out T? item)
		{
			if (index < 0 || index >= source.Count)
			{
				item = default;
				return false;
			}
			
			item = source[index];
			source.RemoveAt(index);
			return true;
		}
		
		public static bool AddIfDistinct<T>(this IList<T> source, T item)
		{
			if (source.Contains(item)) return false;
			source.Add(item);
			return true;
		}
	}
}