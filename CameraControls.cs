using Godot;
using System;

public class CameraControls : Camera2D
{
    private const int SPEED = 10;

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_right"))
        {
            Position = new Vector2(Position.x + SPEED, Position.y);
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            Position = new Vector2(Position.x - SPEED, Position.y);
        }

        if (Input.IsActionPressed("ui_up"))
        {
            Position = new Vector2(Position.x, Position.y - SPEED);
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            Position = new Vector2(Position.x, Position.y + SPEED);
        }
    }
}
