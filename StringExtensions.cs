#nullable enable
namespace TooManyExtensions;

public static class StringExtensions
{
	public static (string, string) SplitOnce(this string str, char separator)
	{
		int index = str.IndexOf(separator);
		return index == -1 ? (str, string.Empty) : (str[..index], str[(index + 1)..]);
	}
	
	public static bool IsEmpty(this string? str)
	{
		return string.IsNullOrEmpty(str);
	}
}