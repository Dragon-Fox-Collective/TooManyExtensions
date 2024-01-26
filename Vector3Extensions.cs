namespace Godot;

public static class Vector3Extensions
{
	public static Vector3 WithMagnitude(this Vector3 vector, float magnitude) => vector.Normalized() * magnitude;
}