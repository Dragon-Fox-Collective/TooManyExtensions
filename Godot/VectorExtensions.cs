#if GODOT
namespace Godot
{
	public static class VectorExtensions
	{
		public static Vector3 Add(Vector3 a, Vector3 b) => a + b;
		public static Vector3 Max(Vector3 a, Vector3 b) => new(Mathf.Max(a.X, b.X), Mathf.Max(a.Y, b.Y), Mathf.Max(a.Z, b.Z));
		public static Vector3 Multiply(Vector3 a, Vector3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		
		public static Vector3 WithMagnitude(this Vector3 vector, double magnitude) => vector.Normalized() * magnitude;
	}
}
#endif