#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TooManyExtensions
{
	public static class Option
	{
		public static Option<T> Some<T>(T value) => new(true, value);
		public static Option<T> None<T>() => new(false, default);
		
		public static T Expect<T>(this Option<T> option, string message) => option.TrySome(out T? some) ? some : throw new InvalidOperationException(message);
		public static T UnwrapOr<T>(this Option<T> option, T defaultValue) => option.TrySome(out T? some) ? some : defaultValue;
		public static T UnwrapOrDefault<T>(this Option<T> option) => option.TrySome(out T? some) ? some : default!;
		public static T UnwrapOrElse<T>(this Option<T> option, Func<T> func) => option.TrySome(out T? some) ? some : func();
		
		public static Result<T, TErr> OkOr<T, TErr>(this Option<T> option, TErr err) => option.TrySome(out T? some) ? Result.Ok<T, TErr>(some) : Result.Err<T, TErr>(err);
		public static Result<T, TErr> OkOrElse<T, TErr>(this Option<T> option, Func<TErr> func) => option.TrySome(out T? some) ? Result.Ok<T, TErr>(some) : Result.Err<T, TErr>(func());
		public static Result<Option<T>, TErr> Transpose<T, TErr>(this Option<Result<T, TErr>> option) => option.TrySome(out Result<T, TErr>? some) ? some.TryOk(out T? ok, out TErr? err) ? Result.Ok<Option<T>, TErr>(Some(ok)) : Result.Err<Option<T>, TErr>(err) : Result.Ok<Option<T>, TErr>(None<T>());
		
		public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate) => option.TrySome(out T? some) && predicate(some) ? option : None<T>();
		public static Option<T> Flatten<T>(this Option<Option<T>> option) => option.TrySome(out Option<T>? some) ? some : None<T>();
		public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapper) => option.TrySome(out T? some) ? Some(mapper(some)) : None<TResult>();
		public static Option<TResult> MapOr<T, TResult>(this Option<T> option, TResult @default, Func<T, TResult> mapper) => option.TrySome(out T? some) ? Some(mapper(some)) : Some(@default);
		public static Option<TResult> MapOrElse<T, TResult>(this Option<T> option, Func<TResult> func, Func<T, TResult> mapper) => option.TrySome(out T? some) ? Some(mapper(some)) : Some(func());
		
		public static Option<(T1, T2)> Zip<T1, T2>(this Option<T1> option1, Option<T2> option2) => option1.TrySome(out T1? some1) && option2.TrySome(out T2? some2) ? Some((some1, some2)) : None<(T1, T2)>();
		public static Option<TResult> ZipWith<T1, T2, TResult>(this Option<T1> option1, Option<T2> option2, Func<T1, T2, TResult> zipper) => option1.TrySome(out T1? some1) && option2.TrySome(out T2? some2) ? Some(zipper(some1, some2)) : None<TResult>();
		
		public static Option<T> And<T>(this Option<T> option1, Option<T> option2) => option1.IsSome ? option2 : option1;
		public static Option<TResult> AndThen<T, TResult>(this Option<T> option, Func<T, Option<TResult>> mapper) => option.TrySome(out T? some) ? mapper(some) : None<TResult>();
		public static Option<T> Or<T>(this Option<T> option1, Option<T> option2) => option1.IsSome ? option1 : option2;
		public static Option<T> OrElse<T>(this Option<T> option, Func<Option<T>> func) => option.IsSome ? option : func();
		public static Option<T> Xor<T>(this Option<T> option1, Option<T> option2) =>
			option1.IsSome && option2.IsSome ? None<T>()
			: option1.IsSome ? option1
			: option2.IsSome ? option2
			: None<T>();
	
	}

	public class Option<T>(bool hasValue, T? value)
	{
		public bool IsSome => hasValue;
		public bool IsNone => !hasValue;
		
		public bool TrySome([NotNullWhen(true)] out T? outValue)
		{
			if (hasValue)
			{
				outValue = value!;
				return true;
			}
			outValue = default!;
			return false;
		}
		public bool TryNone([NotNullWhen(false)] out T? outValue)
		{
			if (hasValue)
			{
				outValue = value!;
				return false;
			}
			outValue = default!;
			return true;
		}
	}
	
	public static class DictionaryOptionExtensions
	{
		public static Option<TValue> Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull =>
			dictionary.TryGetValue(key, out TValue? value) ? Option.Some(value) : Option.None<TValue>();
	}
	
	public static class ListOptionExtensions
	{
		public static IEnumerable<T> WhereSome<T>(this IEnumerable<Option<T>> source) => source
			.Where(option => option.IsSome)
			.Select(option => option.Expect("Option is None"));
		
		public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source.TryFirst(predicate, out T? item) ? Option.Some(item) : Option.None<T>();
		public static Option<T> FirstOrNone<T>(this IEnumerable<T> source) => source.TryFirst(out T? item) ? Option.Some(item) : Option.None<T>();
		public static Option<T> FirstSome<T>(this IEnumerable<Option<T>> source) => source.TryFirst(opt => opt.IsSome, out Option<T>? item) ? item : Option.None<T>();
	}
}