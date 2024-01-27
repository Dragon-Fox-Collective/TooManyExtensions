#if GODOT
namespace Godot
{
	public static class NodeExtensions
	{
		public static bool TryGetNode<T>(this Node node, string path, out T result) where T : class => (result = (node.GetNodeOrNull(path) as T)!) != null;
	
		public static Node AddToSceneUnder(this Node node, Node parent)
		{
			parent.AddChild(node);
			node.Owner = parent.Owner;
			return node;
		}
	
		public static void Replace(this Node node, Node other)
		{
			node.Reparent(other.GetParent(), false);
		
			if (node is Node3D node3D && other is Node3D other3D)
				node3D.Transform = other3D.Transform;
			else if (node is Node2D node2D && other is Node2D other2D)
				node2D.Transform = other2D.Transform;
		
			foreach (Node child in other.GetChildren())
				child.Reparent(node, true);
		
			other.QueueFree();
		}
	}
}
#endif