namespace System;

public static class RandomExtensions
{
	public static bool NextBool(this Random random) => random.Next(2) == 0;
	
	public static T Choose<T>(this Random random, params T[] array) => array[random.Next(array.Length)];
}