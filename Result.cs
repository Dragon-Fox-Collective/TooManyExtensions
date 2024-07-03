#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace TooManyExtensions
{
	public static class Result
	{
		public static Result<TOk, TErr> Ok<TOk, TErr>(TOk value) => new(true, value, default!);
		public static Result<TOk, TErr> Err<TOk, TErr>(TErr error) => new(false, default!, error);
	
		public static bool IsOkAnd<TOk, TErr>(this Result<TOk, TErr> result, Func<TOk, bool> predicate) => result.TryOk(out TOk? ok, out TErr? _) && predicate(ok);
	
		public static Option<TOk> Ok<TOk, TErr>(this Result<TOk, TErr> result) => result.TryOk(out TOk? ok, out TErr? _) ? Option.Some(ok) : Option.None<TOk>();
		public static Option<TErr> Err<TOk, TErr>(this Result<TOk, TErr> result) => result.TryErr(out TOk? _, out TErr? err) ? Option.Some(err) : Option.None<TErr>();
	
		public static Result<TOkResult, TErr> Map<TOk, TErr, TOkResult>(this Result<TOk, TErr> result, Func<TOk, TOkResult> mapper) => result.TryOk(out TOk? ok, out TErr? err) ? Ok<TOkResult, TErr>(mapper(ok)) : Err<TOkResult, TErr>(err);
		public static Result<TOkResult, TErr> MapOr<TOk, TErr, TOkResult>(this Result<TOk, TErr> result, TOkResult defaultValue, Func<TOk, TOkResult> mapper) => result.TryOk(out TOk? ok, out TErr? _) ? Ok<TOkResult, TErr>(mapper(ok)) : Ok<TOkResult, TErr>(defaultValue);
		public static Result<TOkResult, TErr> MapOrElse<TOk, TErr, TOkResult>(this Result<TOk, TErr> result, Func<TOkResult> func, Func<TOk, TOkResult> mapper) => result.TryOk(out TOk? ok, out TErr? _) ? Ok<TOkResult, TErr>(mapper(ok)) : Ok<TOkResult, TErr>(func());
		public static Result<TOk, TErrResult> MapErr<TOk, TErr, TErrResult>(this Result<TOk, TErr> result, Func<TErr, TErrResult> mapper) => result.TryErr(out TOk? ok, out TErr? err) ? Err<TOk, TErrResult>(mapper(err)) : Ok<TOk, TErrResult>(ok);
	
		public static Result<TOk, TErr> Inspect<TOk, TErr>(this Result<TOk, TErr> result, Action<TOk> okAction)
		{
			if (result.TryOk(out TOk? ok, out TErr? _))
				okAction(ok);
			return result;
		}
		public static Result<TOk, TErr> InspectErr<TOk, TErr>(this Result<TOk, TErr> result, Action<TErr> errAction)
		{
			if (!result.TryOk(out TOk? _, out TErr? err))
				errAction(err);
			return result;
		}
	
		public static TOk Expect<TOk, TErr>(this Result<TOk, TErr> result, string message) => result.TryOk(out TOk? ok, out TErr? _) ? ok : throw new InvalidOperationException(message);
		public static TOk UnwrapOr<TOk, TErr>(this Result<TOk, TErr> result, TOk defaultValue) => result.TryOk(out TOk? ok, out TErr? _) ? ok : defaultValue;
		public static TOk UnwrapOrDefault<TOk, TErr>(this Result<TOk, TErr> result) => result.TryOk(out TOk? ok, out TErr? _) ? ok : default!;
		public static TOk UnwrapOrElse<TOk, TErr>(this Result<TOk, TErr> result, Func<TErr, TOk> func) => result.TryOk(out TOk? ok, out TErr? err) ? ok : func(err);
		public static TErr ExpectErr<TOk, TErr>(this Result<TOk, TErr> result, string message) => result.TryErr(out TOk? _, out TErr? err) ? err : throw new InvalidOperationException(message);
	
		public static Result<TOkResult, TErr> And<TOk, TErr, TOkResult>(this Result<TOk, TErr> result, Result<TOkResult, TErr> other) => result.TryOk(out TOk? _, out TErr? err) ? other : Err<TOkResult, TErr>(err);
		public static Result<TOkResult, TErr> AndThen<TOk, TErr, TOkResult>(this Result<TOk, TErr> result, Func<TOk, Result<TOkResult, TErr>> mapper) => result.TryOk(out TOk? ok, out TErr? err) ? mapper(ok) : Err<TOkResult, TErr>(err);
		public static Result<TOk, TErr> Or<TOk, TErr>(this Result<TOk, TErr> result, Result<TOk, TErr> other) => result.IsOk ? result : other;
		public static Result<TOk, TErr> OrElse<TOk, TErr>(this Result<TOk, TErr> result, Func<TErr, Result<TOk, TErr>> func) => result.TryOk(out TOk? _, out TErr? err) ? result : func(err);
	
		public static Option<Result<TOk, TErr>> Transpose<TOk, TErr>(this Result<Option<TOk>, TErr> result) => result.TryOk(out Option<TOk>? ok, out TErr? err) ? ok.TrySome(out TOk? some) ? Option.Some(Ok<TOk, TErr>(some)) : Option.None<Result<TOk, TErr>>() : Option.Some(Err<TOk, TErr>(err));
	
		public static Result<TOk, TErr> Flatten<TOk, TErr>(this Result<Result<TOk, TErr>, TErr> result) => result.TryOk(out Result<TOk, TErr>? ok, out TErr? err) ? ok : Err<TOk, TErr>(err);
	}

	public class Result<TOk, TErr>(bool isOk, TOk okValue, TErr errValue)
	{
		public bool IsOk => isOk;
		public bool IsErr => !isOk;
	
		public bool TryOk([NotNullWhen(true)] out TOk? outValue, [NotNullWhen(false)] out TErr? outError)
		{
			if (isOk)
			{
				outValue = okValue!;
				outError = default!;
				return true;
			}
			outValue = default!;
			outError = errValue!;
			return false;
		}
		public bool TryErr([NotNullWhen(false)] out TOk? outValue, [NotNullWhen(true)] out TErr? outError)
		{
			if (isOk)
			{
				outValue = okValue!;
				outError = default!;
				return false;
			}
			outValue = default!;
			outError = errValue!;
			return true;
		}
	}
}