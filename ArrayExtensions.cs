#nullable enable
namespace System.Linq
{
	public static class ArrayExtensions
	{
		public static T[] Fill<T>(this T[] array, T item)
		{
			T[] newArray = new T[array.Length];
			Array.Fill(newArray, item);
			return newArray;
		}
	}
}