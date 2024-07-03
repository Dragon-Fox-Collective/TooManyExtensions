#nullable enable

namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		public static TValue GetEnsured<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValueFactory) where TKey : notnull
		{
			if (!dictionary.ContainsKey(key))
				dictionary.Add(key, newValueFactory());
			return dictionary[key];
		}
		
		public static TValue GetEnsured<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new() where TKey : notnull =>
			GetEnsured(dictionary, key, () => new TValue());
	}
}