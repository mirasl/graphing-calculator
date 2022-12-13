using Godot;
using System;
using System.Collections;

public class Gridlines : Spatial
{
	[Signal] delegate Transform GetCameraPosition();

	const int LINE_NUMBER = 15;

	public Vector3 GlobalCameraOrigin {set; get;}

	private ArrayList gridlines = new ArrayList();
	private int strongInterval;
	private int thinInterval;

	private PackedScene line3dScene;

	public override void _Ready()
	{
		line3dScene = GD.Load<PackedScene>("res://Line3D.tscn");

		gridlines.Add(new ArrayList());
		gridlines.Add(new ArrayList());
		
		EmitSignal("GetCameraPosition");
		strongInterval = (int)Mathf.Pow(10, (int)(GlobalCameraOrigin.y / 5));
		
		// draw strong lines
		for (int i = -LINE_NUMBER; i <= LINE_NUMBER; i++)
		{
			if (i == 0)
			{
				continue;
			}
			float grayscale = (float)((LINE_NUMBER*0.75f) - (Mathf.Abs(i) / 2)) / (float)LINE_NUMBER;

			Line3D line = line3dScene.Instance<Line3D>();
			line.Vertex1 = new Vector3(LINE_NUMBER*strongInterval, 0, i*strongInterval);
			line.Vertex2 = new Vector3(-LINE_NUMBER*strongInterval, 0, i*strongInterval);
			line.color = new Color(grayscale, grayscale, grayscale);
			AddChild(line);
			((ArrayList)gridlines[0]).Add(line);
		}

		for (int i = -LINE_NUMBER; i <= LINE_NUMBER; i++)
		{
			if (i == 0)
			{
				continue;
			}
			float grayscale = (float)((LINE_NUMBER*0.75f) - (Mathf.Abs(i) / 2)) / (float)LINE_NUMBER;

			Line3D line = line3dScene.Instance<Line3D>();
			line.Vertex1 = new Vector3(i*strongInterval, 0, LINE_NUMBER*strongInterval);
			line.Vertex2 = new Vector3(i*strongInterval, 0, -LINE_NUMBER*strongInterval);
			line.color = new Color(grayscale, grayscale, grayscale);
			AddChild(line);
			((ArrayList)gridlines[1]).Add(line);
		}
	}
	// public override void _PhysicsProcess(float delta)
	// {
	//     foreach (Line3D line in (ArrayList)gridlines[0]) // gridlines which lie along x axis
	//     {
	//         if (Mathf.Abs(line.Vertex1.x - GlobalCameraOrigin.x) > strongInterval*LINE_NUMBER)
	//         {

	//         }
	//     }
	//     foreach (Line3D line in (ArrayList)gridlines[1]) // gridlines which lie along z axis
	//     {

	//     }
	// }
}
