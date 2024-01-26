using NotALegoClone.Utils;

namespace Godot;

public static class NodeExtensions
{
	public static bool TryGetNode<T>(this Node node, string path, out T result) where T : class => (result = (node.GetNodeOrNull(path) as T)!) != null;
	
	public static void AddToGame(this Node node) => Locations.Instance.Game.AddChild(node);

	public static Node AddToSceneUnder(this Node node, Node parent)
	{
		parent.AddChild(node);
		node.Owner = parent.Owner;
		return node;
	}
}