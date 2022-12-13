using Godot;
using System;

public class Calculator3D : Spatial
{
	public void sig_GetCameraPosition()
	{
		GetNode<Gridlines>("Gridlines").GlobalCameraOrigin = 
				GetNode<Camera>("CameraPivot/Camera").GlobalTransform.origin;
	}
}
