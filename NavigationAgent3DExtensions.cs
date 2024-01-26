namespace Godot;

public static class NavigationAgent3DExtensions
{
	public static bool IsNavigationReady(this NavigationAgent3D agent) => NavigationServer3D.MapIsActive(agent.GetNavigationMap());
}