using Godot;
using System;
using Godot.Collections;

public class GraphLine : Line2D
{
    public float f(float x) // x^2
    {
        return Mathf.Sin(x);
    }

    public void graph()
    {
        for (int x = -200; x < 200; x++)
        {
            AddPoint(new Vector2(x, -f(x) * 10));
        }
    }

    public override void _Ready()
    {
        graph();
    }
}
