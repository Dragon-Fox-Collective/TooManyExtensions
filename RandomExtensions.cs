#nullable enable
using System.Collections.Generic;

namespace System
{
	public static class RandomExtensions
	{
		public static bool NextBool(this Random random) => random.Next(2) == 0;
		
		public static T Choose<T>(this Random random, params T[] array) => array[random.Next(array.Length)];
		public static T Choose<T>(this Random random, IList<T> list) => list[random.Next(list.Count)];
		
		public static readonly Random Instance = new();
	}
}