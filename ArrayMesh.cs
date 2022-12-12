using Godot;
using System;
using Godot.Collections;

public class ArrayMesh : MeshInstance
{
    public override void _Ready()
    {
        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(0, 1, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(0, 0, 1);
        // Initialize ArrayMesh:
        ArrayMesh am = new ArrayMesh();
        Godot.Collections.Array arrays = new Godot.Collections.Array();
    }
}
