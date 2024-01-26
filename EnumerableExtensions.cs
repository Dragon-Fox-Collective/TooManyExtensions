using System.Collections.Generic;
using NotALegoClone.Utils;

namespace System.Linq;

public static class EnumerableExtensions
{
	public static string ToDelimString<T>(this IEnumerable<T> source, string delim = ", ") => "[" + string.Join(delim, source) + "]";
	
	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T item in source)
			action(item);
	}
	
	public static void UpdateContentsOf<TSource, TOther>(
		this IEnumerable<TSource?> source,
		IEnumerable<TOther?> other,
		Func<TSource, TOther?> map,
		Func<TSource, TOther> add,
		Action<TOther> remove,
		Action<TOther, int> move)
		where TSource : notnull
		where TOther : notnull
	{
		List<TSource?> sourceList = source.ToList();
		if (!sourceList.NotNull().IsDistinct())
			throw new InvalidOperationException("Source list contained duplicate items.");
		
		List<TOther?> otherList = other.ToList();
		if (!otherList.NotNull().IsDistinct())
			throw new InvalidOperationException("Other list contained duplicate items.");
		
		Dictionary<TSource, TOther> mapDict = sourceList.NotNull().Pair(map).ValueNotNull().ToDictionary();
		
		sourceList.NotNull().Where(sourceItem => !mapDict.ContainsKey(sourceItem)).ForEach(sourceItem =>
		{
			TOther otherItem = add(sourceItem);
			otherList.Add(otherItem);
			mapDict.Add(sourceItem, otherItem);
		});
		otherList.NotNull().Where(otherItem => !mapDict.ContainsValue(otherItem)).ToList().ForEach(otherItem =>
		{
			remove(otherItem);
			otherList.Remove(otherItem);
		});
		
		if (sourceList.NotNull().Count() != otherList.NotNull().Count())
			throw new Exception($"Source and other lists were not the same length once items were added/removed.\n{sourceList.ToDelimString()} vs {otherList.ToDelimString()} with mapDict {mapDict.ToDelimString()}");
		
		for (int sourceIndex = 0, otherIndex = 0; sourceIndex < sourceList.Count; sourceIndex++, otherIndex++)
		{
			if (!sourceList.NextNonNullItem(ref sourceIndex, out TSource sourceItem)) break;
			if (!otherList.NextNonNullItem(ref otherIndex, out TOther actualOtherItem)) break;
			TOther theoreticalOtherItem = mapDict[sourceItem];
			
			if (!theoreticalOtherItem.Equals(actualOtherItem))
			{
				move(theoreticalOtherItem, otherIndex);
				otherList.Move(theoreticalOtherItem, otherIndex);
			}
		}
	}
	
	private static bool NextNonNullItem<T>(this IEnumerable<T?> source, ref int index, out T item)
	{
		if (source.Skip(index).Enumerate().ValueNotNull().TryFirst(out (int Index, T Item) enumeration))
		{
			index += enumeration.Index;
			item = enumeration.Item;
			return true;
		}
		else
		{
			item = default!;
			return false;
		}
	}
	
	public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source) => source.Where(item => item is not null)!;
	public static IEnumerable<(T1, T2)> KeyNotNull<T1, T2>(this IEnumerable<(T1?, T2)> source) => source.Where(item => item.Item1 is not null)!;
	public static IEnumerable<(T1, T2)> ValueNotNull<T1, T2>(this IEnumerable<(T1, T2?)> source) => source.Where(item => item.Item2 is not null)!;
	
	public static IEnumerable<(int Index, T Item)> Enumerate<T>(this IEnumerable<T> source)
	{
		int index = 0;
		foreach (T item in source)
			yield return (index++, item);
	}
	
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> source) where TKey : notnull => source.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
	
	public static BidirectionalDictionary<T1, T2> ToBidirectionalDictionary<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> map)
	where T1 : notnull
	where T2 : notnull
	{
		BidirectionalDictionary<T1, T2> result = new();
		foreach (T1 item in source)
			result.Add(item, map(item));
		return result;
	}
	
	public static bool TryFirst<T>(this IEnumerable<T?> source, out T? item)
	{
		foreach (T? sourceItem in source)
		{
			item = sourceItem;
			return true;
		}
		
		item = default;
		return false;
	}
	
	public static bool TryFirst<T>(this IEnumerable<T?> source, Predicate<T?> predicate, out T? item)
	{
		foreach (T? sourceItem in source)
		{
			if (predicate(sourceItem))
			{
				item = sourceItem;
				return true;
			}
		}
		
		item = default;
		return false;
	}
	
	public static bool IsDistinct<T>(this IEnumerable<T> source)
	{
		IList<T> sourceList = source.ToAsList();
		return sourceList.Distinct().Count() == sourceList.Count;
	}
	
	public static IList<T> ToAsList<T>(this IEnumerable<T> source) => source as IList<T> ?? source.ToList();
	
	public static void Move<T>(this IList<T> source, T item, int index)
	{
		source.Remove(item);
		source.Insert(index, item);
	}
	
	public static IEnumerable<(T1, T2)> Pair<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> map) => source.Select(item => (item, map(item)));
	public static IEnumerable<(T1, T2)> PairKey<T1, T2>(this IEnumerable<T2> source, Func<T2, T1> map) => source.Select(item => (map(item), item));
}