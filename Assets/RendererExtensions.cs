using UnityEngine;

//code from http://wiki.unity3d.com/index.php?title=IsVisibleFrom
public static class RendererExtensions
{
	public static bool IsVisibleFrom(Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
}