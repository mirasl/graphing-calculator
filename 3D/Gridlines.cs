using Godot;
using System;
using System.Collections;

/// <summary>
/// Draws a grid of lines denoting graphical units on the xy-plane.
/// </summary>
public class Gridlines : Spatial
{
	/// <summary>
	/// The number of gridlines to draw starting from the origin in all four
	/// directions.
	/// </summary>
	const int LINE_NUMBER = 15;

	/// <summary>
	/// ArrayList to be populated with each line object in the grid.
	/// </summary>
	private ArrayList gridlines = new ArrayList();

	/// <summary>
	/// Space (in meters) between each gridline.
	/// </summary>
	private int interval;

	/// <summary>
	/// Onready reference to res://3D/Line3D.tscn.
	/// </summary>
	private PackedScene line3dScene;

	public override void _Ready()
	{
		line3dScene = GD.Load<PackedScene>("res://3D/Line3D.tscn");

		gridlines.Add(new ArrayList());
		gridlines.Add(new ArrayList());
		
		interval = 1;
		
		// draw strong lines
		for (int i = -LINE_NUMBER; i <= LINE_NUMBER; i++)
		{
			if (i == 0)
			{
				continue;
			}
			float grayscale = (float)((LINE_NUMBER*0.75f) - 
					(Mathf.Abs(i) / 2)) / (float)LINE_NUMBER;

			Line3D line = line3dScene.Instance<Line3D>();
			line.Vertex1 = new Vector3(LINE_NUMBER*interval, 0, 
					i*interval);
			line.Vertex2 = new Vector3(-LINE_NUMBER*interval, 0, 
					i*interval);
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
			float grayscale = (float)((LINE_NUMBER*0.75f) - 
					(Mathf.Abs(i) / 2)) / (float)LINE_NUMBER;

			Line3D line = line3dScene.Instance<Line3D>();
			line.Vertex1 = new Vector3(i*interval, 0, 
					LINE_NUMBER*interval);
			line.Vertex2 = new Vector3(i*interval, 0, 
					-LINE_NUMBER*interval);
			line.color = new Color(grayscale, grayscale, grayscale);
			AddChild(line);
			((ArrayList)gridlines[1]).Add(line);
		}
	}
}
