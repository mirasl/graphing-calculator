using Godot;
using System;

public class Line3D : ImmediateGeometry
{
    [Export] public Vector3 Vertex1;
    [Export] public Vector3 Vertex2;
    [Export] public Color color;
    
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
