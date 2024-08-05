#if UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
	public static class VectorExtensions
	{
		public static Vector3 Add(Vector3 a, Vector3 b) => a + b;
		public static Vector3 Max(Vector3 a, Vector3 b) => new(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		public static Vector3 Multiply(Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
		
		public static Vector3 CenterOfRotation(Vector3 pos1, Vector3 up1, Vector3 pos2, Vector3 up2)
		{
			float theta = Vector3.Angle(up1, up2) * Mathf.Deg2Rad;
			float height = Mathf.Sqrt((pos2 - pos1).sqrMagnitude / (1 - Mathf.Cos(theta)) / 2f); // cosine rule frickery
			bool centerIsDown = ((pos1 + up1) - (pos2 + up2)).sqrMagnitude > (pos2 - pos1).sqrMagnitude;
			if (centerIsDown)
				height *= -1;
			return pos2 + height * up2; // biased toward pos2 but idc
		}
		
		public static Vector3 Average(this IEnumerable<Vector3> source)
		{
			int count = 0;
			Vector3 sum = Vector3.zero;
			foreach (Vector3 vector in source)
			{
				count++;
				sum += vector;
			}
			return sum / count;
		}
		
		public static IEnumerable<float> AsEnumerable(this Vector3 vector)
		{
			yield return vector.x;
			yield return vector.y;
			yield return vector.z;
		}
		
		public static Vector3 ToVector3(this IEnumerable<float> enumerable)
		{
			Vector3 vector = new();
			int i = 0;
			foreach (float x in enumerable)
			{
				vector[i++] = x;
				if (i == 3)
					break;
			}
			return vector;
		}
		
		public static Vector3 Select(this Vector3 vector, Func<float, float> func) => new(func(vector.x), func(vector.y), func(vector.z));
		public static Vector3 SelectIndex(this Vector3 vector, Func<int, float, float> func) => new(func(0, vector.x), func(1, vector.y), func(2, vector.z));
		public static Vector3 SelectVectorIndex(Func<int, float> func) => new(func(0), func(1), func(2));
		
		public static float Aggregate(this Vector3 vector, Func<float, float, float> func) => vector.Aggregate(0, func);
		public static float Aggregate(this Vector3 vector, float seed, Func<float, float, float> func) => func(func(func(seed, vector.x), vector.y), vector.z);
		
		public static bool Any(this Vector3 vector, Func<float, bool> func) => vector.AsEnumerable().Any(func);
		public static bool All(this Vector3 vector, Func<float, bool> func) => vector.AsEnumerable().All(func);
		
		public static Vector3 Sum<T>(this IEnumerable<T> source, Func<T, Vector3> func) => source.Aggregate(Vector3.zero, (sum, x) => sum + func(x));
		public static Vector3 Sum(this IEnumerable<Vector3> source) => source.Aggregate(Vector3.zero, (sum, x) => sum + x);
		
		public static Matrix4x4 OuterSquared(this Vector3 a) => new(
			new Vector4(a.x * a.x, a.x * a.y, a.x * a.z, 0),
			new Vector4(a.y * a.x, a.y * a.y, a.y * a.z, 0),
			new Vector4(a.z * a.x, a.z * a.y, a.z * a.z, 0),
			new Vector4(0, 0, 0, 1));
		
		public static float InnerSquared(this Vector3 a) => Vector3.Dot(a, a);
		
		public static Vector3 WithMagnitude(this Vector3 vector, float magnitude) => vector.normalized * magnitude;
		public static Vector3 ClampMagnitude(this Vector3 vector, float maxLength) => vector.magnitude > maxLength ? vector.normalized * maxLength : vector;
		public static Vector3 WithMagnitudeRelativeTo(this Vector3 vector, Vector3 relativeVector, float magnitude) => (vector - relativeVector).WithMagnitude(magnitude) + relativeVector;
	}
}
#endif