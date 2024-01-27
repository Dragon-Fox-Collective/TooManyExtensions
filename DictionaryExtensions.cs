namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		public static TValue GetEnsured<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValueFactory)
		{
			if (!dictionary.ContainsKey(key))
				dictionary.Add(key, newValueFactory());
			return dictionary[key];
		}
		
		public static TValue GetEnsured<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new() => GetEnsured(dictionary, key, () => new TValue());
	}
}