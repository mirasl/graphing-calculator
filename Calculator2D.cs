using Godot;
using System;
using Godot.Collections;

public class Calculator2D : Node2D
{
    // The dimensions of the part of the screen currently visible to the camera:
    public Rect2 Screen {get; private set;}

    private PackedScene pLine;


    public override void _Ready()
    {
        GD.Print(Screen);

        pLine = GD.Load<PackedScene>("res://Line2D.tscn");

        Graph();
    }

    // Sample function to graph:
    public float f(float x)
    {
        return -1/x;
    }

    // Sets the line to reflect a graph of the function:
    public void Graph()
    {
        bool inRange = true;

        // The graph needs to be broken up into 100 seperate lines because one 
        // line should not have over 1000 verticies.
        for (int i = -200; i < 200; i++) 
        {
            Line2D line = pLine.Instance<Line2D>();
            AddChild(line);

            for (int xInt = i * 1000; xInt <= (i+1) * 1000; xInt++)
            {
                float x = xInt / 400f;//(Screen.Size.x / 50f);\
                GD.Print(x);
                x += Screen.Position.x;

                // if (inRange)
                // {
                //     if (f(x) > 150 + Screen.Position.y) // too high
                //     {
                //         inRange = false;
                //         line.AddPoint(new Vector2(x - 0.005f, 100000000));
                //     }
                //     else if (f(x) < -150 + Screen.Position.y) // too low
                //     {
                //         inRange = false;
                //         line.AddPoint(new Vector2(x - 0.005f, -100000000));
                //     }
                // }
                if ((f(x) > 150 + Screen.Position.y || f(x) < -150 + Screen.Position.y) && inRange) // outside range
                {
                    inRange = false;
                    if (f(x) > 150 + Screen.Position.y) // too high
                    {
                        inRange = false;
                        line.AddPoint(new Vector2(x, 10000000));
                    }
                    else if (f(x) < -150 + Screen.Position.y) // too low
                    {
                        inRange = false;
                        line.AddPoint(new Vector2(x, -10000000));
                    }
                }

                if ((f(x) <= 150 + Screen.Position.y && f(x) >= -150 + Screen.Position.y) && !inRange)
                {
                    inRange = true;
                    line = pLine.Instance<Line2D>();
                    AddChild(line);

                    if (TakeDerivative(x) > 0) // slope is positive
                    {
                        line.AddPoint(new Vector2(x, 10000000));
                    }
                    else if (TakeDerivative(x) < 0) // slope is negative
                    {
                        line.AddPoint(new Vector2(x, -10000000));
                    }
                    //if (f(x - 0.005f))

                    //line.AddPoint(new Vector2(x, -10000000000));
                }

                if (inRange)
                {
                    line.AddPoint(new Vector2(x, f(x)));
                }
            }
        }
    }

    public float TakeDerivative(float x)
    {
        float rise = f(x) - f(x + 0.00001f);
        float run = 0.00001f;
        return rise / run;
    }

    // Sets the Screen variable to reflect what the camera is currently viewing:
    public void RefreshScreen()
    {
        Camera2D camera = GetNode<Camera2D>("Camera2D");
        Screen = new Rect2(camera.Position, camera.Zoom * 
                GetViewportRect().Size / Scale);
    }

    public override void _PhysicsProcess(float delta)
    {
        ///
        if (Input.IsActionJustPressed("ui_accept"))
        {
            RefreshScreen();
            Graph();
            GD.Print(Screen);
        }
    }
}