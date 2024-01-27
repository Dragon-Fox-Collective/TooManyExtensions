#if UNITY_5_3_OR_NEWER
using System;
using System.Linq;

namespace UnityEngine
{
	public static class RigidbodyExtensions
	{
		public static void Disable(this Rigidbody rigidbody)
		{
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
		}

		public static void Enable(this Rigidbody rigidbody)
		{
			rigidbody.isKinematic = false;
			rigidbody.detectCollisions = true;
		}
		
		public static Vector3 ClosestPointDepenetration(this Collider collider, Vector3 point, SphereCollider dummySphere)
		{
			dummySphere.radius = Vector3.Distance(collider.bounds.center, point);
			if (Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation, dummySphere, point, Quaternion.identity, out Vector3 direction, out float distance))
				return point + direction * (dummySphere.radius - distance);
			throw new Exception("No penetration");
		}
		public static (Collider collider, Vector3 point) ClosestPoint(this Rigidbody rigidbody, Vector3 point) => rigidbody.GetComponentsInChildren<Collider>().Select(collider => (collider, collider.ClosestPoint(point))).MinBy(zip => Vector3.Distance(point, zip.Item2));
		
		public static Bounds Containing(this Bounds a, Bounds b)
		{
			Bounds bounds = new(a.center, a.size);
			bounds.Encapsulate(b);
			return bounds;
		}
		
		public static T GetAttachedComponent<T>(this Collider collider) => collider ? collider.attachedRigidbody ? collider.attachedRigidbody.GetComponent<T>() : default : default;
	}
}
#endif