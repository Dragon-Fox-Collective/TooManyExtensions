using System.Linq;

namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		public static T Pop<T>(this IList<T> list)
		{
			T obj = list[0];
			list.RemoveAt(0);
			return obj;
		}
		
		public static T RandomElement<T>(this IList<T> source) => RandomExtensions.Instance.Choose(source);
		public static void AddRange<T>(this IList<T> source, int count, Func<T> factory) => Enumerable.Range(0, count).Select(_ => factory()).ForEach(source.Add);
		
		public static void Move<T>(this IList<T> source, T item, int index)
		{
			source.Remove(item);
			source.Insert(index, item);
		}
	}
}