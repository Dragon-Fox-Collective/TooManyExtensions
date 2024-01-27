#if UNITY_5_3_OR_NEWER
using System;

namespace UnityEngine
{
	public static class QuaternionExtensions
	{
		// Note that lhs * rhs means rotating by lhs and then by rhs
		public static Quaternion TransformRotation(this Transform from, Quaternion delta) => from.rotation * delta; // from * delta = to
		public static Quaternion InverseTransformRotation(this Transform from, Quaternion to) => from.rotation.Inverse() * to; // delta = from-1 * to
		
		public static Quaternion Inverse(this Quaternion quaternion) => Quaternion.Inverse(quaternion);
		
		public static Vector3 ToEulersAngleAxis(this Quaternion quaternion)
		{
			if (quaternion.w < 0) quaternion = quaternion.Select(x => -x);
			quaternion.ToAngleAxis(out float angle, out Vector3 axis);
			return angle * Mathf.Deg2Rad * axis;
		}
		
		public static Quaternion Select(this Quaternion quaternion, Func<float, float> func) => new(func(quaternion.x), func(quaternion.y), func(quaternion.z), func(quaternion.w));
	}
}
#endif