#if GODOT
using System.Diagnostics.CodeAnalysis;

namespace Godot
{
	public static class GodotObjectExtensions
	{
		public static bool IsValid([NotNullWhen(true)] this GodotObject? obj) => GodotObject.IsInstanceValid(obj);
	
		public static T? OrNull<T>(this T? obj) where T : GodotObject => obj.IsValid() ? obj : null;

		public static void ConnectIfValid(this GodotObject? obj, StringName signal, Callable callable, GodotObject.ConnectFlags flags = 0)
		{
			if (!obj.IsValid()) return;
			if (obj.IsConnected(signal, callable)) return;
			obj.Connect(signal, callable, (uint)flags);
		}

		public static void DisconnectIfValid(this GodotObject? obj, StringName signal, Callable callable)
		{
			if (!obj.IsValid()) return;
			if (!obj.IsConnected(signal, callable)) return;
			obj.Disconnect(signal, callable);
		}
	}
}
#endif