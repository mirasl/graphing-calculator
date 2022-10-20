using Godot;
using System;
using Godot.Collections;

public class Calculator2D : Node2D
{
    private PackedScene pLine;

    public float f(float x) // x^2
    {
        return -Mathf.Sqrt(100 - Mathf.Pow(x,2));
    }

    public void graph()
    {
        bool inRange = true;

        for (int i = -50; i < 50; i++)
        {
            Line2D line = pLine.Instance<Line2D>();
            AddChild(line);

            for (int xInt = i * 1000; xInt <= (i+1) * 1000; xInt++)
            {
                float x = xInt / 100.0f;

                if ((f(x) > 150 || f(x) < -150) && inRange) // outside range
                {
                    inRange = false;
                }

                if ((f(x) <= 150 && f(x) >= -150) && !inRange)
                {
                    inRange = true;
                    line = pLine.Instance<Line2D>();
                    AddChild(line);
                }

                if (inRange)
                {
                    line.AddPoint(new Vector2(x, f(x)));
                }
            }
        }
    }

    public override void _Ready()
    {
        pLine = GD.Load<PackedScene>("res://Line2D.tscn");

        graph();
    }
}