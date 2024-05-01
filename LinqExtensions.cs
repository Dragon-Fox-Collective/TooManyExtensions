#nullable enable
using System.Collections.Generic;

namespace System.Linq
{
	public static class LinqExtensions
	{
		public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source)
		{
			int i = 0;
			foreach (T item in source)
				yield return (i++, item);
		}
		
		public static bool All(this IEnumerable<bool> source) => source.All(b => b);
		public static bool Any(this IEnumerable<bool> source) => source.Any(b => b);
		
		public static TResult InvokeWith<T1, T2, TResult>(this Func<T1, T2, TResult> func, (T1, T2) args) => func(args.Item1, args.Item2);
		public static void InvokeWith<T1, T2>(this Action<T1, T2> func, (T1, T2) args) => func(args.Item1, args.Item2);
		
		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(item => item);
		
		public static IEnumerable<T> Process<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T item in source)
			{
				action(item);
				yield return item;
			}
		}
		
		public static IEnumerable<T> Pivot<T>(this IEnumerable<T> source, int index)
		{
			IEnumerable<T> first = Enumerable.Empty<T>();
			int i = 0;
			foreach (T item in source)
			{
				if (i++ < index)
					first = first.Append(item);
				else
					yield return item;
			}
			foreach (T item in first)
				yield return item;
		}
		
		public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> source, int index)
		{
			IEnumerable<T> first = Enumerable.Empty<T>();
			IEnumerable<T> last = Enumerable.Empty<T>();
			int i = 0;
			foreach (T item in source)
			{
				if (i >= index)
					first = first.Append(item);
				else
					last = last.Append(item);
				i++;
			}
			return (first, last);
		}
		
		public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T newItem)
		{
			int i = 0;
			foreach (T item in enumerable)
			{
				if (i++ == index)
					yield return newItem;
				yield return item;
			}
		}
		
		public static T? ElementAtOrLast<T>(this IEnumerable<T?> enumerable, int index)
		{
			T? last = default;
			int i = 0;
			foreach (T? item in enumerable)
			{
				last = item;
				if (i == index)
					return last;
				i++;
			}
			return last;
		}
		
		public static IEnumerable<(TFirst, TSecond)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
		{
			return first.Zip(second, (firstItem, secondItem) => (firstItem, secondItem));
		}
		
		public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third)
		{
			return Zip(first, second).Zip(third, (firstSecond, thirdItem) => (firstSecond.Item1, firstSecond.Item2, thirdItem));
		}
		
		public static IEnumerable<(TFirst, TSecond, TThird, TFourth)> Zip<TFirst, TSecond, TThird, TFourth>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth)
		{
			return Zip(first, second, third).Zip(fourth, (firstSecondThird, fourthItem) => (firstSecondThird.Item1, firstSecondThird.Item2, firstSecondThird.Item3, fourthItem));
		}
		
		public static (TFirst, TSecond) Zipper<TFirst, TSecond>(TFirst first, TSecond second) => (first, second);
		
		public static IEnumerable<(T?, T?)> ZipOrDefault<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T> generator) => ZipOrDefault(first, second, generator, generator, Zipper);
		public static IEnumerable<(TFirst?, TSecond?)> ZipOrDefault<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second) => ZipOrDefault(first, second, () => default, () => default, Zipper);
		public static IEnumerable<TResult?> ZipOrDefault<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst?> firstGenerator, Func<TSecond?> secondGenerator, Func<TFirst?, TSecond?, TResult> zipper)
		{
			using IEnumerator<TFirst?> firstEnumerator = first.GetEnumerator();
			using IEnumerator<TSecond?> secondEnumerator = second.GetEnumerator();
			
			bool hasFirst = true;
			bool hasSecond = true;
			
			while (true)
			{
				if (hasFirst) hasFirst = firstEnumerator.MoveNext();
				if (hasSecond) hasSecond = secondEnumerator.MoveNext();
				
				if (!hasFirst && !hasSecond)
					break;
				
				yield return zipper(hasFirst ? firstEnumerator.Current : firstGenerator(), hasSecond ? secondEnumerator.Current : secondGenerator());
			}
		}
		
		public static IEnumerable<TResult> Zip<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> source, Func<IEnumerable<TSource>, TResult> zipper)
		{
			// ReSharper disable once NotDisposedResourceIsReturned
			// MustDisposeResourceAttribute isn't in Unity's copy of Jetbrains Annotations, so... ignore for now
			List<IEnumerator<TSource>> enumerators = source.Select(layer => layer.GetEnumerator()).ToList();
			while (true)
			{
				List<IEnumerator<TSource>> currentEnumerators = enumerators.Where(enumerator => enumerator.MoveNext()).ToList();
				if (!currentEnumerators.Any())
				{
					enumerators.ForEach(enumerator => enumerator.Dispose());
					yield break;
				}
				yield return zipper(currentEnumerators.Select(enumerator => enumerator.Current));
			}
		}
		
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T t in source)
				action(t);
		}
		
		public static void ForEach<T1, T2>(this IEnumerable<(T1, T2)> source, Action<T1, T2> action)
		{
			foreach ((T1 item1, T2 item2) in source)
				action(item1, item2);
		}
		
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IList<T> source, int count)
		{
			for (int i = 0; i * count < source.Count; i++)
				yield return source.Skip(i * count).Take(count);
		}
		
		public static bool StartsWith<T>(this IEnumerable<T> source, IEnumerable<T> prefix, IEqualityComparer<T>? comparer = null)
		{
			comparer ??= EqualityComparer<T>.Default;
			
			using IEnumerator<T> sourceEnumerator = source.GetEnumerator();
			using IEnumerator<T> prefixEnumerator = prefix.GetEnumerator();
			while (true)
			{
				if (!sourceEnumerator.MoveNext())
					return !prefixEnumerator.MoveNext();
				
				if (!prefixEnumerator.MoveNext())
					return true;
				
				if (!comparer.Equals(sourceEnumerator.Current, prefixEnumerator.Current))
					return false;
			}
		}
		public static bool StartsWith<T>(this IEnumerable<T> source, T prefix, IEqualityComparer<T>? comparer = null)
		{
			comparer ??= EqualityComparer<T>.Default;
			using IEnumerator<T> sourceEnumerator = source.GetEnumerator();
			return sourceEnumerator.MoveNext() && comparer.Equals(sourceEnumerator.Current, prefix);
		}
		
		public static T? MinBy<T, TSelected>(this IEnumerable<T> source, Func<T, TSelected> selector, bool orDefault = false)
		{
			IComparer<TSelected> comparer = Comparer<TSelected>.Default;
			return source.AggregateBy(selector, (current, item) => comparer.Compare(item, current) < 0, orDefault: orDefault);
		}
		public static T? MaxBy<T, TSelected>(this IEnumerable<T> source, Func<T, TSelected> selector, bool orDefault = false)
		{
			IComparer<TSelected> comparer = Comparer<TSelected>.Default;
			return source.AggregateBy(selector, (current, item) => comparer.Compare(item, current) > 0, orDefault: orDefault);
		}
		public static T? AggregateBy<T, TSelected>(this IEnumerable<T> source, Func<T, TSelected> selector, Func<TSelected, TSelected, bool> comparer, bool orDefault = false)
		{
			bool initialized = false;
			T? min = default;
			TSelected? minSelected = default;
			foreach (T item in source)
			{
				TSelected itemSelected = selector(item);
				if (!initialized || comparer(minSelected!, itemSelected))
				{
					min = item;
					minSelected = itemSelected;
				}
				initialized = true;
			}
			
			if (initialized || orDefault)
				return min;
			throw new ArgumentException("Source is empty", nameof(source));
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
			if (!sourceList.WhereNotNull().IsDistinct())
				throw new InvalidOperationException("Source list contained duplicate items.");
			
			List<TOther?> otherList = other.ToList();
			if (!otherList.WhereNotNull().IsDistinct())
				throw new InvalidOperationException("Other list contained duplicate items.");
			
			Dictionary<TSource, TOther> mapDict = sourceList.WhereNotNull().Pair(map).WhereValueNotNull().ToDictionary();
			
			sourceList.WhereNotNull().Where(sourceItem => !mapDict.ContainsKey(sourceItem)).ForEach(sourceItem =>
			{
				TOther otherItem = add(sourceItem);
				otherList.Add(otherItem);
				mapDict.Add(sourceItem, otherItem);
			});
			otherList.WhereNotNull().Where(otherItem => !mapDict.ContainsValue(otherItem)).ToList().ForEach(otherItem =>
			{
				remove(otherItem);
				otherList.Remove(otherItem);
			});
			
			if (sourceList.WhereNotNull().Count() != otherList.WhereNotNull().Count())
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
			if (source.Skip(index).Enumerate().WhereValueNotNull().TryFirst(out (int Index, T Item) enumeration))
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
		
		public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) => source.Where(item => item is not null)!;
		public static IEnumerable<(T1, T2)> WhereKeyNotNull<T1, T2>(this IEnumerable<(T1?, T2)> source) => source.Where(item => item.Item1 is not null)!;
		public static IEnumerable<(T1, T2)> WhereValueNotNull<T1, T2>(this IEnumerable<(T1, T2?)> source) => source.Where(item => item.Item2 is not null)!;
		
#if !NET8_0_OR_GREATER
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source) where TKey : notnull => source.ToDictionary(tuple => tuple.Key, tuple => tuple.Value);
		
#endif
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
		
		public static IEnumerable<(T1, T2)> Pair<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> map) => source.Select(item => (item, map(item)));
		public static IEnumerable<(T1, T2)> PairKey<T1, T2>(this IEnumerable<T2> source, Func<T2, T1> map) => source.Select(item => (map(item), item));
	}
}