#if UNITY_5_3_OR_NEWER
namespace UnityEngine
{
	public static class TransformExtensions
	{
		public static void SetPositionAndRotation(this Transform transform, Transform other) => transform.SetPositionAndRotation(other.position, other.rotation);
		public static void SetLocalPositionAndRotation(this Transform transform, Transform other) => transform.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
		public static void SetLocalTransforms(this Transform transform, Transform other)
		{
			transform.transform.localPosition = other.localPosition;
			transform.transform.localRotation = other.localRotation;
			transform.transform.localScale = other.localScale;
		}
		public static void Replace(this Transform transform, Transform other)
		{
			transform.SetParent(other.parent, false);
			transform.SetLocalTransforms(other);
			foreach (Transform child in other)
				child.SetParent(transform, true);
			Object.Destroy(other.gameObject);
		}
	}
}
#endif