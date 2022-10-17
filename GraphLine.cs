using Godot;
using System;
using Godot.Collections;

public class GraphLine : Line2D
{
    public float f(float x) // x^2
    {
        return Mathf.Log(x);
    }

    public async void graph()
    {
        for (int x = -200; x < 200; x++)
        {
            AddPoint(new Vector2(x, -f(x) * 10));
        }
        await ToSignal(GetTree().CreateTimer(0), "timeout");
        RemovePoint(GetPointCount() - 1);
    }

    public override void _Ready()
    {
        graph();
    }
}
