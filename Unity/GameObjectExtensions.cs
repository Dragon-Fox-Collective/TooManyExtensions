#if UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public static class GameObjectExtensions
	{
		public static bool IsOnLayer(this GameObject gameObject, int layerMask)
		{
			return ((1 << gameObject.layer) & layerMask) != 0;
		}
		
		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			foreach (Transform child in gameObject.transform)
				child.gameObject.SetLayerRecursively(layer);
		}
		
		public static bool TryGetComponentInChildren<T>(this Component thisComponent, out T component) where T : Component => component = thisComponent.GetComponentInChildren<T>();
		
		public static void PerformOnMaterial(this IEnumerable<Renderer> renderers, Material material, Action<Material> action)
		{
			foreach (Renderer renderer in renderers)
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					string materialName = renderer.materials[i].name;
					if (materialName.EndsWith(" (Instance)") && materialName[..^11] == material.name)
						action.Invoke(renderer.materials[i]);
				}
		}
	}
}
#endif