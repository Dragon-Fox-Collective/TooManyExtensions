#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TooManyExtensions;

public class BidirectionalDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>> where T1 : notnull where T2 : notnull
{
	public Dictionary<T1, T2> Forward { get; } = new();
	public Dictionary<T2, T1> Reverse { get; } = new();
	
	public void Add(T1 t1, T2 t2)
	{
		Forward[t1] = t2;
		Reverse[t2] = t1;
	}
	
	public void Remove(T1 t1)
	{
		Reverse.Remove(Forward[t1]);
		Forward.Remove(t1);
	}
	
	public void Remove(T2 t2)
	{
		Forward.Remove(Reverse[t2]);
		Reverse.Remove(t2);
	}
	
	public bool TryGetValue(T1 t1, [NotNullWhen(true)] out T2? t2) => Forward.TryGetValue(t1, out t2);
	public bool TryGetValue(T2 t2, [NotNullWhen(true)] out T1? t1) => Reverse.TryGetValue(t2, out t1);
	
	public T2 this[T1 t1]
	{
		get => Forward[t1];
		set
		{
			Forward[t1] = value;
			Reverse[value] = t1;
		}
	}
	
	public T1 this[T2 t2]
	{
		get => Reverse[t2];
		set
		{
			Reverse[t2] = value;
			Forward[value] = t2;
		}
	}
	
	public void Clear()
	{
		Forward.Clear();
		Reverse.Clear();
	}
	
	public bool ContainsKey(T1 t1) => Forward.ContainsKey(t1);
	public bool ContainsKey(T2 t2) => Reverse.ContainsKey(t2);
	
	public int Count => Forward.Count;
	
	public IEnumerable<T1> Keys => Forward.Keys;
	public IEnumerable<T2> Values => Forward.Values;
	
	public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => Forward.GetEnumerator();
	
	public bool Remove(KeyValuePair<T1, T2> kvp)
	{
		if (Forward.Remove(kvp.Key) && Reverse.Remove(kvp.Value))
		{
			return true;
		}
		return false;
	}
	
	public bool Remove(KeyValuePair<T2, T1> kvp)
	{
		if (Reverse.Remove(kvp.Key) && Forward.Remove(kvp.Value))
		{
			return true;
		}
		return false;
	}
	
	public bool TryAdd(T1 t1, T2 t2)
	{
		if (Forward.TryAdd(t1, t2))
		{
			Reverse.TryAdd(t2, t1);
			return true;
		}
		return false;
	}
	
	public bool TryRemove(T1 t1, [NotNullWhen(true)] out T2? t2)
	{
		if (ContainsKey(t1))
		{
			t2 = Forward[t1];
			Forward.Remove(t1);
			Reverse.Remove(t2);
			return true;
		}
		t2 = default;
		return false;
	}
	
	public bool TryRemove(T2 t2, [NotNullWhen(true)] out T1? t1)
	{
		if (ContainsKey(t2))
		{
			t1 = Reverse[t2];
			Forward.Remove(t1);
			Reverse.Remove(t2);
			return true;
		}
		t1 = default;
		return false;
	}
	
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}