using Godot;
using System;

/// <summary>
/// Represents a one-dimensional line segment in a three-dimensional 
/// environment, with two end locations (as sets of 3D coordinates) and an 
/// albedo color.
/// </summary>
public class Line3D : ImmediateGeometry
{
	/// <summary>
	/// Coordinates of the first end vertex of the line segment.
	/// </summary>
	[Export] public Vector3 Vertex1;

	/// <summary>
	/// Coordinates of the second end vertex of the line segment.
	/// </summary>
	[Export] public Vector3 Vertex2;

	/// <summary>
	/// Color of the line.
	/// </summary>
	[Export] public Color color;
	
	/// <summary>
	/// Draws the line with the given vertices/color upon scene start.
	/// </summary>
	public override void _Ready()
	{
		Clear();

		Begin(Mesh.PrimitiveType.Lines);

		SetColor(color);

		SetNormal(Vector3.Back);
		SetUv(Vector2.Zero);
		AddVertex(Vertex1);

		SetNormal(Vector3.Back);
		SetUv(Vector2.Zero);
		AddVertex(Vertex2);
		
		End();
	}
}
