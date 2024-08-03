#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System
{
	public static class StringExtensions
	{
		public static (string, string) SplitFirst(this string str, char separator)
		{
			int index = str.IndexOf(separator);
			return index == -1 ? (str, string.Empty) : (str[..index], str[(index + 1)..]);
		}
	
		public static (string, string) SplitLast(this string str, char separator)
		{
			int index = str.LastIndexOf(separator);
			return index == -1 ? (string.Empty, str) : (str[..index], str[(index + 1)..]);
		}
	
		public static bool IsEmpty([NotNullWhen(false)] this string? str)
		{
			return string.IsNullOrEmpty(str);
		}
	
		public static string Indent(this string str, int levels = 1)
		{
			return str.Split("\n").Join("\n" + Enumerable.Repeat("\t", levels).Join());
		}
	}
}
